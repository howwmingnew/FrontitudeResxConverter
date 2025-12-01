<p align="right">
🌐 語言：
<a href="README.md">English</a> |
<a href="README.zh-TW.md">繁體中文</a> |
<a href="README.ko.md">한국어</a>
</p>

# FrontitudeResxConverter

FrontitudeResxConverter 是一個將 **Frontitude 匯出的多語系 JSON**
轉換成 **ResX Manager（Visual Studio 擴充套件）可匯入的 Excel `.xlsx`** 的小工具。

此工具主要用於 .NET / WPF 專案的多國語系流程整合，
協助開發者快速將 Frontitude 的文案資料導入 ResX 管理流程中。

---

## 📌 功能特色

- 將 Frontitude JSON 轉換為 ResX Manager Excel 格式
- 自動建立翻譯欄位、調整欄寬
- 自動處理語系名稱：
  - `ko` / `ko-KR` / `ko_KR` → 全部視為韓文 `.ko`
- 若未提供輸出檔名 → 自動使用「yyyyMMdd.xlsx」
- 若完全沒有輸入參數 → 互動模式請使用者輸入 JSON 路徑
- 支援在 Windows 檔案總管 **拖曳 JSON 檔到 exe 執行**

---

## 📂 JSON 格式（Frontitude 匯出）

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

## 📊 Excel 產出格式（符合 ResX Manager）

產生工作表名稱：`ResXResourceManager`

主要欄位：

- `Project`
- `File`
- `Key`
- `Comment`
- 主語系 (en_US)
- `.ar`、`.fr`、`.ja`、`.ko`、`.pl`、`.ru`
- `.zh-CN`、`.zh-TW`
- ……等常見語系欄位

---

# ▶️ 使用方式

## **方式一：指定輸入 + 指定輸出**

```
FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
```

範例：

```
FrontitudeToResxXlsx.exe TestData/Frontitude_export.json Output/output.xlsx
```

---

## **方式二：只有輸入路徑 → 自動產生輸出檔名**

```
FrontitudeToResxXlsx.exe TestData/Frontitude_export.json
```

---

## **方式三：完全沒有輸入參數 → 等待使用者輸入**

程式會顯示：

```
No arguments provided.
Please enter JSON input file path:
```

---

## **方式四：拖曳執行（最方便）**

1. 在檔案總管找到 JSON 檔  
2. 拖曳到 `FrontitudeToResxXlsx.exe` 上  
3. 程式自動使用該 JSON 執行  

---

# 🏗️ 開發說明

- .NET 8  
- ClosedXML 用於產生 xlsx  
- 可在 VS / VS Code 執行  

---

# 🚀 GitHub Actions（自動化 Build / Release）

- 推送 tag → 自動建置、產生 exe、建立 Release
