<p align="center">
  <img src="ChecksumGen/chicky.ico" alt="Chicksum Generator icon" width="96" />
</p>

<h1 align="center">Chicksum Generator</h1>

<p align="center">
  A small Windows app for generating checksum files for a batch of files.
</p>

<p align="center">
  <a href="https://github.com/EveAeternam/ChicksumGen/actions/workflows/build.yml"><img src="https://github.com/EveAeternam/ChicksumGen/actions/workflows/build.yml/badge.svg" alt="Build status" /></a>
  <img src="https://img.shields.io/badge/.NET-10-512BD4" alt=".NET 10" />
  <img src="https://img.shields.io/badge/platform-Windows-0078D6" alt="Windows" />
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-green" alt="MIT license" /></a>
</p>

<!--
  Screenshot: save a capture of the main window as docs/screenshot.png,
  then uncomment the line below.
  <p align="center"><img src="docs/screenshot.png" alt="Chicksum Generator main window" width="640" /></p>
-->

## Features

- **Add files your way.** Pick files from the file dialog, or drag files and whole folders onto the window (folders are scanned recursively).
- **Four hash algorithms.** MD5, SHA-1, SHA-256 and SHA-512, chosen by the file type you save as (`.md5`, `.sha1`, `.sha256`, `.sha512`).
- **One checksum file for the whole batch.** Each line holds the hash and the file's full path.
- **Review before saving.** Hashes appear next to each file in the list, and checked files can be removed before you save.

## Usage

1. Launch Chicksum Generator.
2. Click **Add**, or drag files and folders onto the list.
3. Click **Save**, choose where to write the checksum file, and pick the algorithm from the *Save as type* list.

The saved file looks like this:

```text
9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08 C:\Users\you\Downloads\installer.exe
2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae C:\Users\you\Downloads\readme.txt
```

## Building from source

Requirements: Windows and the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
git clone https://github.com/EveAeternam/ChicksumGen.git
cd ChicksumGen
dotnet run --project ChecksumGen
```

To produce a standalone build:

```powershell
dotnet publish ChecksumGen -c Release -o publish
```

You can also open `ChecksumGen.slnx` in Visual Studio 2026.

Every push to `main` is built by [GitHub Actions](.github/workflows/build.yml), and the compiled app is attached to each run as a downloadable artifact.

## Project layout

```text
ChecksumGen/
├── Program.cs             Entry point
├── MainForm.cs            File list, hashing and saving
├── MainForm.Designer.cs   WinForms layout
├── Resources/             Toolbar icons
└── chicky.ico             App icon
```

## Roadmap

- CRC-32 (`.sfv`) and SHA3-256/512 output (stubbed in `MainForm.cs`, not yet implemented)
- Verifying files against an existing checksum file

## License

[MIT](LICENSE) © 2026 Evangeline "Evie" Minet
