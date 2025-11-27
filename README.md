# FrontitudeResxConverter

將 Frontitude 匯出的多語系 JSON 轉換成 ResX Manager 可匯入的 xlsx 檔的小工具。

## 功能簡介

- 讀取 Frontitude 匯出的 JSON 檔（語系 → key → value 結構）。
- 依照 ResX Manager 的匯入格式，產生對應的 xlsx：
  - 一列一個資源 key（例如 `about_0`）。
  - 一欄一個語系（例如 `en_US`、`ar`、`fr`、`zh-CN`、`zh-TW`…）。
  - `Project`、`File` 等欄位可依照既有專案習慣填寫。

## 前置需求

- .NET SDK（建議 .NET 6 或 8）。
- Windows 環境（目前 GitHub Actions workflow 使用 `windows-latest`）。

## JSON 格式說明（Frontitude 匯出）

本工具預期的 JSON 結構如下：

```json
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

- 第一層 key：語系代碼（例如 `en_US`、`ar`、`fr`、`zh_CN`、`zh_TW`）。
- 第二層 key：資源 key（例如 `about_0`），value 為對應語系的翻譯文字。

## 輸出 xlsx 格式說明（ResX Manager）

輸出時會建立一個工作表 `ResXResourceManager`，欄位布局與 ResX Manager 的 Excel 匯入格式相容，常見欄位：

- `Project`
- `File`
- `Key`
- `Comment`
- 主語系欄位（對應 `en_US`）
- 各語系欄位：`.ar`、`.fr`、`.ja`、`.ko`、`.zh-CN`、`.zh-TW` …

實際欄位名稱與順序可依現有專案的範例檔調整。

## 使用方式（命令列）

在編譯完成後，執行：

```bash
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

範例：

```bash
FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx
```

- `inputJsonPath`：Frontitude 匯出的 JSON 檔路徑。
- `outputXlsxPath`：要輸出的 xlsx 檔案路徑。

執行完成後，將產生可供 ResX Manager 匯入的 Excel 檔。

## 專案結構

簡化後結構示意：

```text
FrontitudeResxConverter/
├─ FrontitudeToResxXlsx/        # Console 專案
│  ├─ Program.cs
│  └─ FrontitudeToResxXlsx.csproj
├─ .github/
│  └─ workflows/
│     └─ release.yml            # GitHub Actions：自動 Build + Release
├─ README.md
└─ Frontitude_export.json       # 測試用 JSON（可視需求放置）
```

## 開發環境建議

1. 使用 Visual Studio 或 VS Code 開啟此專案。
2. 透過 NuGet 安裝 ClosedXML（用來輸出 xlsx）：
   - 套件名稱：`ClosedXML`
3. 編譯後即可於命令列執行。

## GitHub Actions：自動 Build & Release

此專案附帶一個 GitHub Actions workflow（`.github/workflows/release.yml`）：

- 觸發條件：push tag，名稱符合 `v*`（例如 `v1.0.0`）。
- 在 `windows-latest` runner 上：
  - 還原 NuGet 套件。
  - 使用 Release 設定編譯 Console App。
  - 使用 `dotnet publish` 輸出執行檔。
  - 將輸出資料夾打包為 zip。
  - 使用 `softprops/action-gh-release` 建立 GitHub Release 並上傳 zip。

使用流程：

1. 修改程式並 push。
2. 建立 tag：

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. GitHub Actions 會自動執行 workflow，完成後於 GitHub Releases 頁面即可下載對應版本 zip 檔。

## 授權條款

依實際需求填寫（例如 MIT License）。

