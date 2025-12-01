<p align="right">
🌐 Language:
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

A small utility that converts multilingual JSON exported from **Frontitude** into an Excel `.xlsx` file that can be imported by **ResX Manager** (Visual Studio extension).

## Overview

This tool is designed for WPF/.NET applications that:

- Use **Frontitude** to manage UI copy and translations.
- Use **ResX Manager** to manage `.resx` resource files in Visual Studio.
- Need to **bridge** Frontitude's JSON export format and ResX Manager's Excel import format.

The program:

- Reads a Frontitude JSON export (language → key → value).
- Generates an Excel file with the layout expected by ResX Manager:
  - **One row per resource key** (e.g., `about_0`).
  - **One column per language** (e.g., `en_US`, `ar`, `fr`, `zh-CN`, `zh-TW`, …).
  - Includes `Project`, `File`, `Key`, `Comment`, etc., matching an existing sample file.

## Requirements

- .NET SDK (recommended: .NET 6 or .NET 8).
- Windows environment (the GitHub Actions workflow uses `windows-latest`).
- NuGet package: [`ClosedXML`](https://www.nuget.org/packages/ClosedXML) for writing `.xlsx` files.

## Frontitude JSON format

The expected JSON structure is:

```jsonc
{
  "en_US": {
    "about_0": "All right reserved.",
    "about_1": "Terms of use"
  },
  "ar": {
    "about_0": "...",
    "about_1": "..."
  },
  "fr": {
    "about_0": "..."
  },
  "zh_CN": {
    "about_0": "..."
  },
  "zh_TW": {
    "about_0": "..."
  }
}
```

- Top-level keys are **language codes** (e.g., `en_US`, `ar`, `fr`, `zh_CN`, `zh_TW`).
- Second-level keys are **resource keys** (e.g., `about_0`), and the values are the translated strings.

## Output Excel format (ResX Manager)

The program creates a worksheet named `ResXResourceManager`, compatible with ResX Manager's Excel import.

Typical columns:

- `Project`
- `File`
- `Key`
- `Comment`
- Main language column (e.g., `en_US`)
- Per-language columns: `.ar`, `.fr`, `.ja`, `.kk`, `.ko`, `.pl`, `.pt`, `.ro`, `.ru`, `.th`, `.tr`, `.vi`, `.zh-CN`, `.zh-TW`, …

The exact headers and order can be adjusted to match your existing ResX Manager sample file.

## Usage (command line)

After building the project, run:

```bash
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

Example:

```bash
FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx
```

- `inputJsonPath` – path to the JSON file exported from Frontitude.
- `outputXlsxPath` – path for the generated `.xlsx` file.

Once executed, the tool produces an Excel file that can be imported into ResX Manager.

## Project structure

A simplified structure:

```text
FrontitudeResxConverter/
├─ FrontitudeToResxXlsx/          # Console project
│  ├─ Core/
│  ├─ Properties/
│  ├─ TestData/
│  ├─ FrontitudeToResxXlsx.csproj
│  └─ FrontitudeToResxXlsx.sln
├─ .github/
│  └─ workflows/
│     └─ release.yml              # GitHub Actions: Build + Release
├─ README.md                      # English
├─ README.zh-TW.md                # Traditional Chinese
└─ README.ko.md                   # Korean
```

## Development

1. Open the solution in Visual Studio or VS Code.
2. Install the `ClosedXML` NuGet package.
3. Build the solution.
4. Run the console app with the JSON input and desired Excel output path.

## GitHub Actions: automatic Build & Release

This repository includes a GitHub Actions workflow (`.github/workflows/release.yml`) that:

- Triggers on **push tags matching `v*`** (e.g., `v1.0.0`).
- Runs on `windows-latest`.
- Restores NuGet packages.
- Builds and publishes the console app for `win-x64` as a single `.exe`.
- Creates a `.zip` package.
- Uses `softprops/action-gh-release` to create/update a GitHub Release and upload:
  - The standalone `.exe`
  - The `.zip` package

### Release flow

1. Make changes and push to `main`.
2. Create a tag:

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. GitHub Actions runs automatically.
4. Download the artifacts from the **Releases** page.