using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RazorMrzLibrary
{
    public class OcrResult
    {
        public int Confidence { get; set; }
        public string Text { get; set; }
        public int[] Points { get; set; }

        public OcrResult()
        {
            Points = new int[8];
            Text = "";
        }

        public static List<OcrResult> WrapResult(JsonElement? result)
        {
            List<OcrResult> results = new List<OcrResult>();
            if (result != null)
            {
                JsonElement element = result.Value;
                Console.WriteLine(element.ToString());

                if (element.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        if (item.TryGetProperty("lineResults", out JsonElement lineResults))
                        {
                            foreach (JsonElement line in lineResults.EnumerateArray())
                            {
                                OcrResult mrzResult = new OcrResult();
                                Console.WriteLine(line.ToString());
                                if (line.TryGetProperty("confidence", out JsonElement confidenceValue))
                                {
                                    int value = confidenceValue.GetInt32();
                                    mrzResult.Confidence = value;
                                }

                                if (line.TryGetProperty("location", out JsonElement locationValue))
                                {
                                    if (locationValue.TryGetProperty("points", out JsonElement pointsValue))
                                    {
                                        int index = 0;
                                        if (pointsValue.ValueKind == JsonValueKind.Array)
                                        {
                                            foreach (JsonElement point in pointsValue.EnumerateArray())
                                            {
                                                if (point.TryGetProperty("x", out JsonElement xValue))
                                                {
                                                    int intValue = xValue.GetInt32();
                                                    mrzResult.Points[index++] = intValue;
                                                }

                                                if (point.TryGetProperty("y", out JsonElement yValue))
                                                {
                                                    int intValue = yValue.GetInt32();
                                                    mrzResult.Points[index++] = intValue;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (line.TryGetProperty("text", out JsonElement textValue))
                                {
                                    string? value = textValue.GetString();
                                    if (value != null)
                                    {
                                        mrzResult.Text = value;
                                    }

                                }

                                results.Add(mrzResult);
                            }
                        }
                    }
                }
            }
            return results;
        }
    }
}
