<p align="right">
🌐 Language:
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

FrontitudeResxConverter is a utility that converts **Frontitude-exported multilingual JSON**
into an **Excel `.xlsx` file compatible with ResX Manager** (a Visual Studio extension used for managing `.resx` localization files).

This tool is designed for .NET / WPF applications that need to integrate Frontitude workflows with traditional `.resx`-based localization.

---

## Features

- Converts Frontitude JSON to a ResX Manager–importable Excel file
- Auto-generates translation columns and adjusts formatting
- Handles Korean variants automatically:
  - `ko`, `ko-KR`, `ko_KR` → all mapped to `.ko`
- Auto-generates output filename (yyyyMMdd.xlsx) if none is provided
- Interactive mode when no arguments are passed
- Supports **drag-and-drop execution** (drop JSON onto exe)

---

## Frontitude JSON Format

```json
{
  "en_US": {
    "about_0": "All rights reserved.",
    "about_1": "Terms of use"
  },
  "ko-KR": {
    "about_0": "..."
  },
  "zh_TW": {
    "about_0": "..."
  }
}
```

---

## Excel Output Format (ResX Manager)

Worksheet name: `ResXResourceManager`

Includes columns:

- `Project`, `File`, `Key`, `Comment`
- Main language (en_US)
- `.ar`, `.fr`, `.ja`, `.ko`, `.pl`, `.ru`
- `.zh-CN`, `.zh-TW`
- And more depending on mappings

---

# Usage

### Method 1 — Specify input + output

```
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

### Method 2 — Auto-generate output filename

```
FrontitudeToResxXlsx.exe input.json
```

### Method 3 — Interactive mode (no arguments)

```
FrontitudeToResxXlsx.exe
```

### Method 4 — Drag & Drop (Windows Explorer)

Drag your JSON file onto the exe.

---

## Development

- .NET 8  
- Uses ClosedXML  
- Works in Visual Studio & VS Code  

---

## GitHub Actions (Build & Release)

- Tag push → auto build + auto release
