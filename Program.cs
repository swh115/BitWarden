using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BitwardenApiExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonOutput = RunBitwardenListItems();
            if (!string.IsNullOrEmpty(jsonOutput))
            {
                var items = JsonConvert.DeserializeObject<List<VaultItem>>(jsonOutput);
                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        Console.WriteLine($"Item Name: {item.Name}, ID: {item.Id}");
                        foreach (var field in item.Fields)
                        {
                            Console.WriteLine($"Field Name: {field.Name}, Field Value: {field.Value}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No items found.");
                }
            }
            else
            {
                Console.WriteLine("Error retrieving items.");
            }
        }

        private static string RunBitwardenListItems()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "bw",
                    Arguments = "list items --session <qWMgHRstl1FudI2auNTbXKPZ2b7N2mI3f1/F2LMgof+8BOV78QftqMaiHX+NOC7Nhhhz+i9D8wob3ijbDeKTsA==>",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public class VaultItem
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;

            [JsonProperty("name")]
            public string Name { get; set; } = string.Empty;

            [JsonProperty("fields")]
            public List<Field> Fields { get; set; } = new();

            public class Field
            {
                [JsonProperty("name")]
                public string Name { get; set; } = string.Empty;

                [JsonProperty("value")]
                public string Value { get; set; } = string.Empty;
            }
        }
    }
}
