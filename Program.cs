using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shotliner.Utils;

namespace Shotliner
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = null;
            string newPath = null;
            string outputPath = null;

            // Parse CLI arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--base":
                        basePath = args[++i];
                        break;
                    case "--new":
                        newPath = args[++i];
                        break;
                    case "--output":
                        outputPath = args[++i];
                        break;
                    case "--help":
                        PrintHelp();
                        return;
                }
            }

            // If missing args, show help
            if (string.IsNullOrEmpty(basePath) || string.IsNullOrEmpty(newPath) || string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("[!] Missing required arguments.\n");
                PrintHelp();
                return;
            }

            // Run the diff logic
            try
            {
                TimelineDiff.RunDiff(basePath, newPath, outputPath);
                Console.WriteLine($"\n[✓] Diff completed. Results written to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[X] Error: {ex.Message}");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine(@"
Shotliner - Timeline Diff Tool for Malware Analysis

Required Arguments:
  --Base <path>       Path to the clean (baseline) timeline CSV
  --New <path>        Path to the post-infection timeline CSV
  --Output <path>     Path to write the diff CSV result

Optional:
  --Help              Show this help menu

Example:
  Shotliner.exe --Base baseline.csv --New infected.csv --Output diff.csv
");
        }
    }
}
