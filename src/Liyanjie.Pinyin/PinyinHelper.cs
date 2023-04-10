namespace Liyanjie.Pinyin;

/// <summary>
/// 
/// </summary>
public class PinyinHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chineseChar"></param>
    /// <returns></returns>
    public static string[] GetChineseCharPinyins(char chineseChar)
    {
        if ((chineseChar > 47 && chineseChar < 58) || (chineseChar > 64 && chineseChar < 91) || (chineseChar > 96 && chineseChar < 123))
            return new[] { chineseChar.ToString() };

        return ChineseCharManager.ChineseChars.ContainsKey(chineseChar)
            ? ChineseCharManager.ChineseChars[chineseChar].Pinyins
            : new[] { "*" };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chineseWord"></param>
    /// <returns></returns>
    public static string[]? GetChineseWordPinyin(string chineseWord)
    {
        if (string.IsNullOrWhiteSpace(chineseWord))
            return default;

        return ChineseWordManager.ChineseWords.ContainsKey(chineseWord)
            ? ChineseWordManager.ChineseWords[chineseWord]
            : chineseWord.Select(_ => GetChineseCharPinyins(_)[0]).ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chineseWords"></param>
    /// <returns></returns>
    public static string[] GetPinyin(IEnumerable<string> chineseWords)
    {
        return chineseWords
            .SelectMany(_ => GetChineseWordPinyin(_))
            .ToArray();
    }

    static string GetAbsolutePath(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChineseCharManager
    {
        static readonly Lazy<(string Version, Dictionary<char, (string Code, string[] Pinyins)> Data)> lazyData = new(() =>
        {
            var dataFile = GetAbsolutePath(DataFile!);
            if (!File.Exists(dataFile))
                throw new FileNotFoundException($"Data file not exists:{dataFile}", dataFile);
            var chineseChars = new Dictionary<char, (string, string[])>();
            var data = File.ReadAllLines(dataFile, Encoding.UTF8)
                .Where(_ => !string.IsNullOrWhiteSpace(_));
            var version = data.First().Split(':')[1].Trim();
            var charsData = data.Where(_ => !_.StartsWith("#"))
                .Select(_ => _.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            foreach (var array in charsData)
            {
                var @char = array[3][0];
                if (chineseChars.ContainsKey(@char))
                    continue;

                chineseChars.Add(@char, (array[0].TrimEnd(':'), array[1].Split(',')));
            }
            return (version, chineseChars);
        });

        /// <summary>
        /// 
        /// </summary>
        public static string DataFile { get; set; } = @"Resources\pinyin.txt";

        /// <summary>
        /// 
        /// </summary>
        public static string? DataVersion => lazyData.Value.Version;

        /// <summary>
        /// 
        /// </summary>
        public static IReadOnlyDictionary<char, (string Code, string[] Pinyins)> ChineseChars => lazyData.Value.Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseChar"></param>
        /// <param name="code"></param>
        /// <param name="pinyins"></param>
        public static void AddChineseChar(char chineseChar, string code, string[] pinyins)
        {
            if (lazyData.Value.Data.ContainsKey(chineseChar))
                lazyData.Value.Data[chineseChar] = (lazyData.Value.Data[chineseChar].Code, lazyData.Value.Data[chineseChar].Pinyins.Concat(pinyins).ToArray());
            else
                lazyData.Value.Data[chineseChar] = (code, pinyins);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChineseWordManager
    {
        static readonly Lazy<(string Version, Dictionary<string, string[]> Data)> lazyData = new(() =>
        {
            var dataFile = GetAbsolutePath(DataFile!);
            if (!File.Exists(dataFile))
                throw new FileNotFoundException($"Data file not exists:{dataFile}", dataFile);
            var chineseWords = new Dictionary<string, string[]>();
            var data = File.ReadAllLines(dataFile, Encoding.UTF8)
                .Where(_ => !string.IsNullOrWhiteSpace(_));
            var version = data.First().Split(':')[1].Trim();
            var wordsData = data.Where(_ => !_.StartsWith("#"))
                .Select(_ => _.Split(':', StringSplitOptions.RemoveEmptyEntries));
            foreach (var array in wordsData)
            {
                var word = array[0];
                if (chineseWords.ContainsKey(word))
                    continue;
                chineseWords.Add(word, array[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            return (version, chineseWords);
        });

        /// <summary>
        /// 
        /// </summary>
        public static string DataFile { get; set; } = @"Resources\large_pinyin.txt";

        /// <summary>
        /// 
        /// </summary>
        public static string? DataVersion => lazyData.Value.Version;

        /// <summary>
        /// 
        /// </summary>
        public static IReadOnlyDictionary<string, string[]> ChineseWords => lazyData.Value.Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseWord"></param>
        /// <param name="pinyins"></param>
        public static void AddChineseWord(string chineseWord, params string[] pinyins)
        {
            if (lazyData.Value.Data.ContainsKey(chineseWord))
                lazyData.Value.Data[chineseWord] = lazyData.Value.Data[chineseWord].Concat(pinyins).ToArray();
            else
                lazyData.Value.Data[chineseWord] = pinyins;
        }
    }
}

