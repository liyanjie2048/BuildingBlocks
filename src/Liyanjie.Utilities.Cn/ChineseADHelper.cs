using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Liyanjie.Utilities.Cn
{
    public class ChineseADHelper
    {
        static readonly Lazy<Dictionary<string, Dictionary<string, string>>> lazyData = new(() =>
        {
            var dataFile = DataFile.GetAbsolutePath();
            if (!File.Exists(dataFile))
                throw new FileNotFoundException("数据文件未找到", dataFile);
            var json = File.ReadAllText(dataFile);
            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
        });

        public static string DataFile { get; set; } = @"Resources\\ChineseADs.json";

        public static IReadOnlyDictionary<string, Dictionary<string, string>> ChineseChars => lazyData.Value;

        public static string DataVersion => lazyData.Value.TryGetValue("version", out var value)
            ? $"{value.Single().Key}.{value.Single().Value}"
            : default;

        public static bool TryGetChildren(string code, out Dictionary<string, string> children)
        {
            if (Regex.IsMatch(code, @"^\d{4}00$"))
                return lazyData.Value.TryGetValue(code, out children);
            else
            {
                children = default;
                return false;
            }
        }
        public static string[] Display(string code)
        {
            if (!Regex.IsMatch(code, @"^\d{6,9}$"))
                return default;

            var output = new List<string>();

            var provinceCode = $"{code.Substring(0, 2)}0000";
            var cityCode = $"{code.Substring(0, 4)}00";

            if (TryGetChildren("86", out var provinces) && provinces.TryGetValue(provinceCode, out var province))
            {
                output.Add(province);

                if (TryGetChildren(provinceCode, out var cities) && cities.TryGetValue(cityCode, out var city))
                {
                    output.Add(city);

                    if (TryGetChildren(cityCode, out var counties) && counties.TryGetValue(code, out var county))
                    {
                        output.Add(county);
                    }
                }
            }

            return output.ToArray();
        }
    }
}
