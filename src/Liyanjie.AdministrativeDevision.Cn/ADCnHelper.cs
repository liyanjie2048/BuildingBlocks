namespace Liyanjie.AdministrativeDevision.Cn;

public class ADCnHelper
{
    static string GetAbsolutePath(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
    }

    static readonly Lazy<Dictionary<string, Dictionary<string, string>>> lazyData = new(() =>
        {
            var dataFile = GetAbsolutePath(DataFile!);
            if (!File.Exists(dataFile))
                throw new FileNotFoundException($"Data file not exists:{dataFile}", dataFile);
            var json = File.ReadAllText(dataFile);
            if (string.IsNullOrWhiteSpace(json))
                return new();
            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json!)!;
        });

    public static string DataFile { get; set; } = @"Resources\\123-all.json";

    /// <summary>
    /// 
    /// </summary>
    public static string? DataVersion => lazyData.Value.TryGetValue("info", out var info)
        ? info.TryGetValue("version", out var version) ? version : default
        : default;

    public static IReadOnlyDictionary<string, Dictionary<string, string>> Data => lazyData.Value;

    public static bool TryGetChildren(string code, out Dictionary<string, string>? children)
    {
        if (Regex.IsMatch(code, @"^0|(\d{2})|(\d{4})$"))
            return lazyData.Value.TryGetValue(code, out children);
        else
        {
            children = default;
            return false;
        }
    }
    public static string[]? Display(string code)
    {
        if (!Regex.IsMatch(code, @"^(\d{2})|(\d{4})|(\d{6})$"))
            return default;

        var output = new List<string>();
        if (code.Length >= 2)
        {
            var provinceCode = $"{code[..2]}";
            if (TryGetChildren("0", out var provinces) && provinces?.TryGetValue(provinceCode, out var province) == true)
                output.Add(province);
        }
        if (code.Length >= 4)
        {
            var provinceCode = $"{code[..2]}";
            var cityCode = $"{code[..4]}";

            if (TryGetChildren(provinceCode, out var cities) && cities?.TryGetValue(cityCode, out var city) == true)
                output.Add(city);
        }
        if (code.Length >= 6)
        {
            var cityCode = $"{code[..4]}";
            var countyCode = $"{code[..6]}";
            if (TryGetChildren(cityCode, out var counties) && counties?.TryGetValue(countyCode, out var county) == true)
                output.Add(county);
        }

        return output.ToArray();
    }
}

