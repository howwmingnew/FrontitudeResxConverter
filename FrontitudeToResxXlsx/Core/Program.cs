using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ClosedXML.Excel;

// Simple utility:
// Convert Frontitude JSON -> ResX Manager compatible .xlsx
// Usage:
//   FrontitudeToResxXlsx.exe <inputJsonPath> <outputXlsxPath>
// Example:
//   FrontitudeToResxXlsx.exe Frontitude_export.json output.xlsx

namespace FrontitudeToResxXlsx
{
    internal class Program
    {
        static int Main(string[] args)
        {
            // If no arguments are provided → wait for user input
            if (args.Length < 1)
            {
                Console.WriteLine("No arguments provided.");
                Console.Write("Please enter JSON input file path: ");
                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("No input provided.");
                    return 1;
                }

                args = new string[] { userInput };
            }

            var inputJsonPath = args[0];
            string outputXlsxPath;

            // Use second argument if provided
            if (args.Length >= 2)
            {
                outputXlsxPath = args[1];
            }
            else
            {
                // Auto-generate filename based on today's date
                var today = DateTime.Now.ToString("yyyyMMdd");
                outputXlsxPath = $"{today}.xlsx";
                Console.WriteLine($"Output path not provided. Auto-generated: {outputXlsxPath}");
            }

            if (!File.Exists(inputJsonPath))
            {
                Console.WriteLine($"JSON file not found: {inputJsonPath}");
                return 1;
            }

            try
            {
                var jsonText = File.ReadAllText(inputJsonPath);

                // Data structure: Dictionary<Language, Dictionary<Key, Value>>
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonText, options);
                if (data == null)
                {
                    Console.WriteLine("Failed to parse JSON content.");
                    return 1;
                }

                if (!data.ContainsKey("en_US"))
                {
                    Console.WriteLine("JSON does not contain 'en_US' node. Cannot determine base key list.");
                    return 1;
                }

                var enDict = data["en_US"];
                var keys = new List<string>(enDict.Keys);

                // Create new xlsx
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("ResXResourceManager");

                // --- Header Row (copied according to sample file) ---
                int row = 1;
                ws.Cell(row, 1).Value = "Project";
                ws.Cell(row, 2).Value = "File";
                ws.Cell(row, 3).Value = "Key";
                ws.Cell(row, 4).Value = "Comment";
                ws.Cell(row, 5).Value = string.Empty; // Main language (en_US)

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

                // JSON language → column mapping
                var langToCol = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    ["en_US"] = 5,
                    ["ar"] = 7,
                    ["fr"] = 9,
                    ["ja"] = 11,
                    ["kk"] = 13,

                    // Korean may appear as "ko", "ko-KR", or "ko_KR"
                    // Treat all as the same language column (.ko)
                    ["ko"] = 15,
                    ["ko-KR"] = 15,
                    ["ko_KR"] = 15,

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

                // --- Write each resource row ---
                row = 2;
                foreach (var key in keys)
                {
                    ws.Cell(row, 1).Value = "UI"; // Based on sample
                    ws.Cell(row, 2).Value = "Properties\\Resources"; // Based on sample
                    ws.Cell(row, 3).Value = key;
                    ws.Cell(row, 4).Value = string.Empty; // Comment left empty

                    foreach (var (lang, dict) in data)
                    {
                        if (!langToCol.TryGetValue(lang, out var col))
                        {
                            // Ignore languages not in mapping
                            continue;
                        }

                        if (dict.TryGetValue(key, out var value))
                        {
                            ws.Cell(row, col).Value = value;
                        }
                    }

                    row++;
                }

                // Auto-adjust column width
                ws.Columns().AdjustToContents();

                // Save output file
                wb.SaveAs(outputXlsxPath);

                // Auto-open the generated file if save succeeded
                try
                {
                    var fullPath = Path.GetFullPath(outputXlsxPath);
                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine("Opening output file...");
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = fullPath,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception openEx)
                {
                    Console.WriteLine($"File saved but could not be opened automatically: {openEx.Message}");
                }
                Console.WriteLine($"Done. Output: {outputXlsxPath}");

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
