using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Shotliner.Utils
{
    public static class TimelineDiff
    {
        private static readonly string[] DefaultCompareFields = new[] { "DataPath", "DataDetails", "Description" };

        public static List<Dictionary<string, string>> GetDifferences(string baselinePath, string infectedPath, string[] compareFields = null)
        {
            compareFields ??= DefaultCompareFields;

            var baselineRows = LoadCsv(baselinePath);
            var infectedRows = LoadCsv(infectedPath);

            var baselineSet = new HashSet<string>(baselineRows.Select(row => GetComparisonKey(row, compareFields)));
            var newRows = infectedRows.Where(row => !baselineSet.Contains(GetComparisonKey(row, compareFields))).ToList();

            return newRows;
        }

        public static void WriteDiffToCsv(List<Dictionary<string, string>> rows, string outputPath)
        {
            if (rows.Count == 0)
            {
                File.WriteAllText(outputPath, "No differences found.\n");
                return;
            }

            var headers = rows[0].Keys.ToList();
            using var writer = new StreamWriter(outputPath);
            writer.WriteLine(string.Join(",", headers.Select(EscapeCsv)));

            foreach (var row in rows)
            {
                writer.WriteLine(string.Join(",", headers.Select(h => EscapeCsv(row[h] ?? ""))));
            }
        }

        public static void RunDiff(string baseFile, string newFile, string outputFile = null, string[] fields = null)
        {
            var diffs = GetDifferences(baseFile, newFile, fields);
            outputFile ??= $"diff_output_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            WriteDiffToCsv(diffs, outputFile);
            Console.WriteLine($"\n[✓] Diff complete. Output written to: {outputFile}");
        }

        private static List<Dictionary<string, string>> LoadCsv(string path)
        {
            var rows = new List<Dictionary<string, string>>();
            var lines = File.ReadAllLines(path);
            if (lines.Length < 2) return rows;

            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = SplitCsvLine(lines[i]);
                var row = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    row[headers[j]] = values[j];
                }
                rows.Add(row);
            }

            return rows;
        }

        private static string GetComparisonKey(Dictionary<string, string> row, string[] fields)
        {
            return string.Join("|", fields.Select(f => row.ContainsKey(f) ? row[f]?.Trim() : ""));
        }

        private static string EscapeCsv(string input)
        {
            if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }
            return input;
        }

        private static string[] SplitCsvLine(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            string current = "";

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current += '"';
                        i++;
                    }
                    else if (c == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        current += c;
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        values.Add(current);
                        current = "";
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        current += c;
                    }
                }
            }

            values.Add(current);
            return values.ToArray();
        }
    }
}
