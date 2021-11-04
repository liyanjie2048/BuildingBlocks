using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Liyanjie.Utilities.Cn
{
    /// <summary>
    /// 
    /// </summary>
    public class PhoneNumberHelper
    {
        static readonly Lazy<Dictionary<string, PhoneNumber>> lazyData = new(() =>
        {
            var dataFile = DataFile.GetAbsolutePath();
            if (!File.Exists(dataFile))
                throw new FileNotFoundException("数据文件未找到", dataFile);

            var phoneNumbers = new Dictionary<string, PhoneNumber>();
            var zeroBytes = Encoding.UTF8.GetBytes("\0");

            var data = File.ReadAllBytes(dataFile);
            DataVersion = Encoding.Default.GetString(data.Take(4).ToArray());
            var boundary = data[4] | data[5] << 8 | data[6] << 16 | data[7] << 24;
            for (int i = boundary; i < data.Length; i += 9)
            {
                var number7 = (data[i] | data[i + 1] << 8 | data[i + 2] << 16 | data[i + 3] << 24).ToString();
                var infoIndex = data[i + 4] | data[i + 5] << 8 | data[i + 6] << 16 | data[i + 7] << 24;
                var carrier = data[i + 8];
                for (int j = infoIndex; j < boundary; j++)
                {
                    if (data[j] == zeroBytes[0])
                    {
                        var info = Encoding.UTF8.GetString(data, infoIndex, j - infoIndex).Split('|');
                        if (!phoneNumbers.ContainsKey(number7))
                            phoneNumbers.Add(number7, new PhoneNumber
                            {
                                Number7 = number7,
                                Province = info[0],
                                City = info[1],
                                ZipCode = info[2],
                                AreaZone = info[3],
                                Carrier = (Carrier)Convert.ToInt32(carrier),
                            });
                        break;
                    }
                }
            }
            return phoneNumbers;
        });

        /// <summary>
        /// 
        /// </summary>
        public static string DataFile { get; set; } = @"Resources\phone.dat";

        /// <summary>
        /// 
        /// </summary>
        public static string DataVersion { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static IReadOnlyDictionary<string, PhoneNumber> PhoneNumbers => lazyData.Value;

        /// <summary>
        /// 返回查找的号码的信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool TryFind(string phoneNumber, out PhoneNumber number)
        {
            number = default;

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            if (!Regex.IsMatch(phoneNumber, @"^1[3-9]\d{5,9}"))
                return false;

            var number7 = phoneNumber.Substring(0, 7);
            if (lazyData.Value.ContainsKey(number7))
            {
                number = lazyData.Value[number7];
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 运营商
        /// </summary>
        public enum Carrier
        {
            [Description("未知")]
            UNKNOWN = 0,
            [Description("中国移动")]
            CMCC,
            [Description("中国联通")]
            CUCC,
            [Description("中国电信")]
            CTCC,
            [Description("电信虚拟运营商")]
            CTCC_V,
            [Description("联通虚拟运营商")]
            CUCC_V,
            [Description("移动虚拟运营商")]
            CMCC_V
        };

        /// <summary>
        /// 手机号码信息结构体
        /// </summary>
        public struct PhoneNumber
        {
            public string Number7 { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string AreaZone { get; set; }
            public Carrier Carrier { get; set; }

            public override string ToString()
            {
                return $"Number7:{Number7}\t{Province}|{City}|{ZipCode}|{AreaZone}|{Carrier}";
            }
        }
    }
}
