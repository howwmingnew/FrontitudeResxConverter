<p align="right">
🌐 語言：
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

一個將 **Frontitude** 匯出的多語系 JSON，轉換為 **ResX Manager** 可匯入的 Excel `.xlsx` 檔的小工具。

## 功能簡介

本工具適用於使用 WPF / .NET 的專案，情境大致如下：

- 使用 **Frontitude** 管理 UI 文案與多國語系內容。
- 在 Visual Studio 中使用 **ResX Manager** 管理 `.resx` 資源檔。
- 需要在兩者之間建立橋樑：  
  把 Frontitude 的 JSON 匯出 → 轉成 ResX Manager 可讀的 Excel 匯入格式。

工具會：

- 讀取 Frontitude 匯出的 JSON（語系 → key → value 的結構）。
- 產生符合 ResX Manager 匯入格式的 Excel：
  - 一列代表一個資源 key（例如 `about_0`）。
  - 一欄代表一種語言（例如 `en_US`、`ar`、`fr`、`zh-CN`、`zh-TW`…）。
  - 包含 `Project`、`File`、`Key`、`Comment` 等欄位，欄位順序可依既有範例調整。

## 前置需求

- .NET SDK（建議 .NET 6 或 .NET 8）。
- Windows 環境（目前 GitHub Actions workflow 使用 `windows-latest`）。
- NuGet 套件：[`ClosedXML`](https://www.nuget.org/packages/ClosedXML) 用來輸出 `.xlsx`。

## JSON 格式說明（Frontitude 匯出）

預期的 JSON 結構如下：

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

- 第一層 key：語系代碼（例如 `en_US`、`ar`、`fr`、`zh_CN`、`zh_TW`）。
- 第二層 key：資源 key（例如 `about_0`），value 為對應語系的翻譯文字。

## 輸出 Excel 格式（ResX Manager）

程式會建立名為 `ResXResourceManager` 的工作表，欄位設計與 ResX Manager 的 Excel 匯入相容。

常見欄位：

- `Project`
- `File`
- `Key`
- `Comment`
- 主語系欄位（例如 `en_US`）
- 各語系欄位：`.ar`、`.fr`、`.ja`、`.kk`、`.ko`、`.pl`、`.pt`、`.ro`、`.ru`、`.th`、`.tr`、`.vi`、`.zh-CN`、`.zh-TW`……

實際欄位名稱與順序可依你現有的 ResX Manager 範例檔調整。

## 使用方式（命令列）

編譯完成後，在命令列執行：

```bash
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

範例：

```bash
FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx
```

- `inputJsonPath`：Frontitude 匯出的 JSON 檔路徑。
- `outputXlsxPath`：要輸出的 Excel 檔案路徑。

執行成功後，會產生一個可由 ResX Manager 匯入的 Excel 檔。

## 專案結構

簡化後的結構示意：

```text
FrontitudeResxConverter/
├─ FrontitudeToResxXlsx/          # Console 專案
│  ├─ Core/
│  ├─ Properties/
│  ├─ TestData/
│  ├─ FrontitudeToResxXlsx.csproj
│  └─ FrontitudeToResxXlsx.sln
├─ .github/
│  └─ workflows/
│     └─ release.yml              # GitHub Actions：自動 Build + Release
├─ README.md                      # 英文
├─ README.zh-TW.md                # 繁體中文
└─ README.ko.md                   # 韓文
```

## 開發環境建議

1. 使用 Visual Studio 或 VS Code 開啟此專案。
2. 透過 NuGet 安裝 `ClosedXML` 套件。
3. 編譯後即可於命令列執行，指定 JSON 與輸出 xlsx 路徑。

## GitHub Actions：自動 Build & Release

此專案包含一個 GitHub Actions workflow（`.github/workflows/release.yml`）：

- 觸發條件：push **tag 名稱符合 `v*`**（例如 `v1.0.0`）。
- 在 `windows-latest` 上執行：
  - 還原 NuGet 套件。
  - 使用 Release 設定建置 Console App。
  - 針對 `win-x64` 做 `dotnet publish`，輸出單一 `.exe`。
  - 產生 `.zip` 壓縮檔。
  - 使用 `softprops/action-gh-release` 建立或更新 GitHub Release，並上傳：
    - 單一 exe
    - zip 包

### 使用流程

1. 修改程式並 push 到 `main`。
2. 建立 tag：

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. GitHub Actions 會自動執行
4. 完成後可在 **Releases** 頁面下載對應版本的檔案。