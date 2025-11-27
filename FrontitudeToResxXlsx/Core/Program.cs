using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ClosedXML.Excel;

// 簡單的小工具：
// Frontitude Json -> ResX Manager 可匯入的 xlsx
// 使用方式：
//   FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
// 例如：
//   FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx

namespace FrontitudeToResxXlsx
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: FrontitudeToResxXlsx <inputJsonPath> <outputXlsxPath>");
                return 1;
            }

            var inputJsonPath = args[0];
            var outputXlsxPath = args[1];

            if (!File.Exists(inputJsonPath))
            {
                Console.WriteLine($"Json file not found: {inputJsonPath}");
                return 1;
            }

            try
            {
                var jsonText = File.ReadAllText(inputJsonPath);

                // 結構：Dictionary<語系, Dictionary<key, value>>
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonText, options);
                if (data == null)
                {
                    Console.WriteLine("Json 內容解析失敗。");
                    return 1;
                }

                if (!data.ContainsKey("en_US"))
                {
                    Console.WriteLine("Json 中找不到 en_US 節點，無法決定主 key 列表。");
                    return 1;
                }

                var enDict = data["en_US"];
                var keys = new List<string>(enDict.Keys);

                // 建立新的 xlsx
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("ResXResourceManager");

                // --- Header Row (根據你提供的 sample 照抄) ---
                int row = 1;
                ws.Cell(row, 1).Value = "Project";
                ws.Cell(row, 2).Value = "File";
                ws.Cell(row, 3).Value = "Key";
                ws.Cell(row, 4).Value = "Comment";
                ws.Cell(row, 5).Value = string.Empty;      // 主語系 (en_US)

                ws.Cell(row, 6).Value = "Comment.ar";
                ws.Cell(row, 7).Value = ".ar";
                ws.Cell(row, 8).Value = "Comment.fr";
                ws.Cell(row, 9).Value = ".fr";
                ws.Cell(row, 10).Value = "Comment.ja";
                ws.Cell(row, 11).Value = ".ja";
                ws.Cell(row, 12).Value = "Comment.kk";
                ws.Cell(row, 13).Value = ".kk";
                ws.Cell(row, 14).Value = "Comment.ko";
                ws.Cell(row, 15).Value = ".ko";
                ws.Cell(row, 16).Value = "Comment.pl";
                ws.Cell(row, 17).Value = ".pl";
                ws.Cell(row, 18).Value = "Comment.pt";
                ws.Cell(row, 19).Value = ".pt";
                ws.Cell(row, 20).Value = "Comment.ro";
                ws.Cell(row, 21).Value = ".ro";
                ws.Cell(row, 22).Value = "Comment.ru";
                ws.Cell(row, 23).Value = ".ru";
                ws.Cell(row, 24).Value = "Comment.th";
                ws.Cell(row, 25).Value = ".th";
                ws.Cell(row, 26).Value = "Comment.tr";
                ws.Cell(row, 27).Value = ".tr";
                ws.Cell(row, 28).Value = "Comment.vi";
                ws.Cell(row, 29).Value = ".vi";
                ws.Cell(row, 30).Value = "Comment.zh-CN";
                ws.Cell(row, 31).Value = ".zh-CN";
                ws.Cell(row, 32).Value = "Comment.zh-TW";
                ws.Cell(row, 33).Value = ".zh-TW";

                // 建立 Json 語系 -> 欄位的對應表
                var langToCol = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    ["en_US"] = 5,
                    ["ar"] = 7,
                    ["fr"] = 9,
                    ["ja"] = 11,
                    ["kk"] = 13,
                    ["ko"] = 15,
                    ["pl"] = 17,
                    ["pt"] = 19,
                    ["ro"] = 21,
                    ["ru"] = 23,
                    ["th"] = 25,
                    ["tr"] = 27,
                    ["vi"] = 29,
                    ["zh_CN"] = 31,
                    ["zh_TW"] = 33,
                };

                // --- 寫入每一列資源 ---
                row = 2;
                foreach (var key in keys)
                {
                    ws.Cell(row, 1).Value = "UI";                   // 依照 sample，Project 固定填 UI
                    ws.Cell(row, 2).Value = "Properties\\Resources"; // 依照 sample
                    ws.Cell(row, 3).Value = key;
                    ws.Cell(row, 4).Value = string.Empty;             // Comment 先留空

                    foreach (var (lang, dict) in data)
                    {
                        if (!langToCol.TryGetValue(lang, out var col))
                        {
                            // 有不在 mapping 中的語系就略過
                            continue;
                        }

                        if (dict.TryGetValue(key, out var value))
                        {
                            ws.Cell(row, col).Value = value;
                        }
                    }

                    row++;
                }

                // 簡單調整欄寬好看一點
                ws.Columns().AdjustToContents();

                // 輸出檔案
                wb.SaveAs(outputXlsxPath);
                Console.WriteLine($"Done. Output: {outputXlsxPath}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤：" + ex.Message);
                return 1;
            }
        }
    }
}
