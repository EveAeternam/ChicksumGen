# Chicksum Generator

A Windows desktop application for generating and verifying file checksums.

## Application Details

| Property | Value |
|----------|-------|
| **Name** | Chicksum Generator |
| **Version** | 1.0.0 |
| **Build Date** | 2026 |
| **Platform** | Windows |
| **Framework** | .NET 10 |
| **Language** | C# |
| **Type** | Windows Forms (WinForms) Application |

## Description

Chicksum Generator is a lightweight checksum utility designed to help users verify file integrity. The application provides a simple Windows Forms interface for selecting files and generating checksum values.

## Key Features

- **File Selection**: Choose files to hash from the Windows file picker
- **Checksum Generation**: Produce checksum values for integrity verification
- **Simple UI**: Minimal Windows Forms workflow focused on speed and clarity

## Technical Stack

- **Framework**: .NET 10 with Windows Forms
- **Language**: C#
- **UI Framework**: Windows Forms (WinForms)
- **File I/O**: System.IO

## Project Structure

```
ChecksumGen/
├── Program.cs                 # Application entry point
├── MainForm.cs                # Main application window logic
├── MainForm.Designer.cs       # UI component design
├── MainForm.resx              # UI resources
├── Resources.resx             # Application resources
└── ChecksumGen.csproj         # Project file
```

## System Requirements

- Windows operating system
- .NET 10 Runtime

## Getting Started

To build and run the application:

1. Ensure .NET 10 SDK is installed
2. Navigate to the project directory
3. Run `dotnet build` to compile
4. Run `dotnet run` to execute the application

## Usage

1. Launch Chicksum Generator.
2. Select a file using the file picker in the UI.
3. Generate the checksum and copy the result as needed.

## Development Notes

- Nullable reference types are enabled
- Implicit using statements are enabled
- Windows Forms is configured for this Windows-only desktop application

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Last Updated**: 2026
