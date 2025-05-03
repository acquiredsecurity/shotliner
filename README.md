# shotliner

<img width="475" alt="image" src="https://github.com/user-attachments/assets/a79e95e8-a996-47f5-a6a0-30ad36a19840" />

# Shotliner

**Shotliner** is a malware forensic timeline diffing tool. Inspired by [Regshot](https://github.com/Seabreg/Regshot), it compares a clean baseline forensic timeline against a post-infection timeline to identify newly introduced artifact activity. Ideal for malware triage, reverse engineering, and forensic investigations. Always take a snapshot of your clean VM! Take a baseline collection and process your raw forensic artifacts with the tools below. Once you have a baseline you shouldn;t have take one again if you always revert to a clean snapshot on your analysis VM.

Tools you should use for triage collection and data processing supported by ForensicTimeliner
- Kape/EZ tools 
- Axiom
- Chainsaw
- Hayabusa

- Run [ForensicTimeliner](https://github.com/acquiredsecurity/forensic-timeliner) on the output from your processing tools and create a forensic timeline. Now you have a baseline of your VM for comparison to your output post Malware Execution!

Execute your malware sample and let it run for the duration of your intended analysis and then take a second forensic artifact collection of the VM/Host. 
Rerun your variaition of EZ Tools, Axiom, Chainsaw, Hayabusa and then again run ForensicTimeliner.

Now you have TWO timelines one pre execution and one post execution. 
Use shotliner to run a dif between the two forensictimeliner outputs to more easily find malware based activities and elminate all the known behaviors from your timeline to quickly get to the bad!

```sample commandline
.\shotliner.exe --Base C:\Users\admin0x\Desktop\shotliner\test\base\20250502_235216_ForensicTimeliner.csv --New C:\Users\admin0x\Desktop\shotliner\test\infected\20250502_235455_ForensicTimeliner.csv --Output diff.csv
```
| Argument   | Description                                  |
| ---------- | -------------------------------------------- |
| `--Base`   | Path to the clean baseline timeline CSV      |
| `--New`    | Path to the infected/post-event timeline CSV |
| `--Output` | (Optional) Custom output path for diff CSV   |
| `--Help`   | Displays this help menu                      |

---

## Features

- **Artifact Diffing**: Compares two CSV timeline exports from ForensicTimeliner and highlights only newly introduced rows.
- **No Date-Based Filtering**: Diffing is performed on key behavioral fields, not timestamps.
- **Modular Fields**: Supports standard ForensicTimeliner output headers.
- **Portable & Lightweight**: Single EXE, no dependencies.
- **CSV Output**: Generates a timestamped diff CSV in standard timeline format.

---

## Timeline Format

Shotliner expects timeline input files in the [ForensicTimeliner](https://github.com/acquiredsecurity/forensic-timeliner) format, which aggregates artifact data collected from:

- [Eric Zimmerman's KAPE and EZ Tools](https://ericzimmerman.github.io/)
- [Magnet Axiom](https://www.magnetforensics.com/)
- [Chainsaw](https://github.com/WithSecureLabs/chainsaw)
- [Hayabusa](https://github.com/Yamato-Security/hayabusa)

```csv
DateTime,TimestampInfo,ArtifactName,Tool,Description,DataDetails,DataPath,FileExtension,EventId,User,Computer,FileSize,IPAddress,SourceAddress,DestinationAddress,SHA1,Count,EvidencePath
```


