using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Liyanjie.Utilities.Cn
{
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
        public static string[] GetChineseWordPinyin(string chineseWord)
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

        /// <summary>
        /// 
        /// </summary>
        public class ChineseCharManager
        {
            static readonly Lazy<Dictionary<char, (string Code, string[] Pinyins)>> lazyData = new(() =>
            {
                var dataFile = DataFile.GetAbsolutePath();
                if (!File.Exists(dataFile))
                    throw new FileNotFoundException("数据文件未找到", dataFile);
                var chineseChars = new Dictionary<char, (string, string[])>();
                var data = File.ReadAllLines(dataFile, Encoding.UTF8)
                    .Where(_ => !string.IsNullOrWhiteSpace(_));
                DataVersion = data.First().Split(':')[1].Trim();
                var charsData = data.Where(_ => !_.StartsWith("#"))
                    .Select(_ => _.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var array in charsData)
                {
                    var @char = array[3][0];
                    if (chineseChars.ContainsKey(@char))
                        continue;

                    chineseChars.Add(@char, (array[0].TrimEnd(':'), array[1].Split(',')));
                }
                return chineseChars;
            });

            /// <summary>
            /// 
            /// </summary>
            public static string DataFile { get; set; } = @"Resources\pinyin.txt";

            /// <summary>
            /// 
            /// </summary>
            public static string DataVersion { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public static IReadOnlyDictionary<char, (string Code, string[] Pinyins)> ChineseChars => lazyData.Value;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chineseChar"></param>
            /// <param name="code"></param>
            /// <param name="pinyins"></param>
            public static void AddChineseChar(char chineseChar, string code, string[] pinyins)
            {
                if (lazyData.Value.ContainsKey(chineseChar))
                    lazyData.Value[chineseChar] = (lazyData.Value[chineseChar].Code, lazyData.Value[chineseChar].Pinyins.Concat(pinyins).ToArray());
                else
                    lazyData.Value[chineseChar] = (code, pinyins);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ChineseWordManager
        {
            static readonly Lazy<Dictionary<string, string[]>> lazyData = new(() =>
            {
                var dataFile = DataFile.GetAbsolutePath();
                if (!File.Exists(dataFile))
                    throw new FileNotFoundException("数据文件未找到", dataFile);
                var chineseWords = new Dictionary<string, string[]>();
                var data = File.ReadAllLines(dataFile, Encoding.UTF8)
                    .Where(_ => !string.IsNullOrWhiteSpace(_));
                DataVersion = data.First().Split(':')[1].Trim();
                var wordsData = data.Where(_ => !_.StartsWith("#"))
                    .Select(_ => _.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var array in wordsData)
                {
                    var word = array[0];
                    if (chineseWords.ContainsKey(word))
                        continue;
                    chineseWords.Add(word, array[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
                return chineseWords;
            });

            /// <summary>
            /// 
            /// </summary>
            public static string DataVersion { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public static string DataFile { get; set; } = @"Resources\large_pinyin.txt";

            /// <summary>
            /// 
            /// </summary>
            public static IReadOnlyDictionary<string, string[]> ChineseWords => lazyData.Value;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chineseWord"></param>
            /// <param name="pinyins"></param>
            public static void AddChineseWord(string chineseWord, params string[] pinyins)
            {
                if (lazyData.Value.ContainsKey(chineseWord))
                    lazyData.Value[chineseWord] = lazyData.Value[chineseWord].Concat(pinyins).ToArray();
                else
                    lazyData.Value[chineseWord] = pinyins;
            }
        }
    }
}
