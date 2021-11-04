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
    public class ChineseCharHelper
    {
        static readonly Lazy<Dictionary<char, (string, int, string[])>> lazyData = new(() =>
        {
            var dataFile = DataFile.GetAbsolutePath();
            if (!File.Exists(dataFile))
                throw new FileNotFoundException("数据文件未找到", dataFile);
            var chineseChars = new Dictionary<char, (string, int, string[])>();
            var data = File.ReadAllLines(dataFile, Encoding.UTF8)
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Where(_ => !_.StartsWith("#"))
                .Select(_ => _.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            foreach (var array in data)
            {
                var @char = array[1][0];
                if (chineseChars.ContainsKey(@char))
                    continue;

                chineseChars.Add(@char, (array[0], int.Parse(array[2]), array[3].Split(',')));
            }
            return chineseChars;
        });

        public static string DataFile { get; set; } = @"Resources\\ChineseChars.txt";

        public static IReadOnlyDictionary<char, (string Unicode, int StrokeCount, string[] Pinyins)> ChineseChars => lazyData.Value;

        public static bool TryGetChineseCharInfo(char chineseChar, out (string Unicode, int StrokeCount, string[] Pinyins) info)
        {
            if ((chineseChar > 47 && chineseChar < 58) || (chineseChar > 64 && chineseChar < 91) || (chineseChar > 96 && chineseChar < 123))
                goto Default;

            if (ChineseChars.ContainsKey(chineseChar))
            {
                info = ChineseChars[chineseChar];
                return true;
            }
            else
                goto Default;

            Default:
            info = default;
            return false;
        }
    }
}
