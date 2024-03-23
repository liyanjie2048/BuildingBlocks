namespace Liyanjie.QQWry;

public class QQWryHelper
{
    static string GetAbsolutePath(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
    }

    static readonly Lazy<QQWry> qqwry_Lazy = new(() => ReadData());
    static QQWry ReadData()
    {
        var dataFile = GetAbsolutePath(DatFile!);
        if (!File.Exists(dataFile))
            throw new FileNotFoundException($"Data file not exists:{dataFile}", dataFile);
        return new QQWry(dataFile);
    }

    static QQWryHelper()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static string DatFile { get; set; } = @"Resources\qqwry.dat";
    public static (string? Area, string? ISP) SearchIP(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return default;
        if (!Regex.IsMatch(ip, @"^([1-9]?\d|1\d{2}|2([0-4]\d|5[0-5]))(\.([1-9]?\d|1\d{2}|2([0-4]\d|5[0-5]))){3}$"))
            return default;

        var l = qqwry_Lazy.Value.SearchIPLocation(ip);
        return (l.Area, l.ISP);
    }

    /// <summary>
    /// IP对象类，该对象包含所属国家和地区
    /// </summary>
    class IPLocation
    {
        public string? ISP { get; set; }
        public string? Area { get; set; }
    }

    /// <summary>
    /// 读取QQ纯真IP数据库 
    /// </summary>    
    sealed class QQWry
    {
        //第一种模式 
        const byte REDIRECT_MODE_1 = 0x01;

        //第二种模式 
        const byte REDIRECT_MODE_2 = 0x02;

        //每条记录长度 
        const int IP_RECORD_LENGTH = 7;

        const string unISP = "未知ISP";
        const string unArea = "未知地区";

        //数据库文件 
        readonly FileStream ipFile;

        //索引开始位置 
        readonly long ipBegin;

        //索引结束位置 
        readonly long ipEnd;

        //IP地址对象 
        readonly IPLocation loc;

        //存储文本内容 
        readonly byte[] buf;

        //存储4字节 
        readonly byte[] b4;

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="ipfile">IP数据库文件绝对路径</param> 
        /// <returns></returns> 
        public QQWry(string ipfile)
        {
            buf = new byte[100];
            b4 = new byte[4];
            lock (this)
            {
                ipFile = new FileStream(ipfile, FileMode.Open);
                ipBegin = ReadLong4(0);
                ipEnd = ReadLong4(4);
                loc = new IPLocation();
            }
        }

        /// <summary> 
        /// 搜索IP地址搜索 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <returns></returns> 
        public IPLocation SearchIPLocation(string ip)
        {
            //将字符IP转换为字节 
            var ipSp = ip.Split('.');
            if (ipSp.Length != 4)
            {
                ip = "127.0.0.1";
                ipSp = ip.Split('.');
            }
            var IP = new byte[4];
            for (int i = 0; i < IP.Length; i++)
            {
                IP[i] = (byte)(Int32.Parse(ipSp[i]) & 0xFF);
            }

            IPLocation? local = null;
            var offset = LocateIP(IP);
            if (offset != -1)
            {
                local = GetIPLocation(offset);
            }
            local ??= new IPLocation
            {
                Area = unArea,
                ISP = unISP
            };
            return local;
        }

        /// <summary> 
        /// 取得具体信息 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        IPLocation GetIPLocation(long offset)
        {
            ipFile.Position = offset + 4;

            //读取第一个字节判断是否是标志字节 
            var one = (byte)ipFile.ReadByte();
            if (one == REDIRECT_MODE_1)
            {
                //第一种模式 
                //读取国家偏移 
                var countryOffset = ReadLong3();
                //转至偏移处 
                ipFile.Position = countryOffset;

                //再次检查标志字节 
                var b = (byte)ipFile.ReadByte();
                if (b == REDIRECT_MODE_2)
                {
                    loc.Area = ReadString(ReadLong3());
                    ipFile.Position = countryOffset + 4;
                }
                else
                    loc.Area = ReadString(countryOffset);

                //读取运营商标志
                loc.ISP = ReadArea(ipFile.Position);

            }
            else if (one == REDIRECT_MODE_2)
            {
                //第二种模式 
                loc.Area = ReadString(ReadLong3());
                loc.ISP = ReadArea(offset + 8);
            }
            else
            {
                //普通模式 
                loc.Area = ReadString(--ipFile.Position);
                loc.ISP = ReadString(ipFile.Position);
            }
            ipFile.Close();
            return loc;
        }

        /// <summary> 
        /// 读取地区名称 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        string ReadArea(long offset)
        {
            ipFile.Position = offset;

            var one = (byte)ipFile.ReadByte();
            if (one == REDIRECT_MODE_1 || one == REDIRECT_MODE_2)
            {
                var areaOffset = ReadLong3(offset + 1);
                return (areaOffset == 0) ? unArea : ReadString(areaOffset);
            }
            else
            {
                return ReadString(offset);
            }
        }

        /// <summary> 
        /// 读取字符串 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        string ReadString(long offset)
        {
            ipFile.Position = offset;

            var i = 0;
            for (buf[i] = (byte)ipFile.ReadByte(); buf[i] != 0; buf[++i] = (byte)ipFile.ReadByte()) ;
            return (i > 0)
                ? Encoding.GetEncoding("GBK").GetString(buf, 0, i)
                : string.Empty;
        }

        /// <summary> 
        /// 查找IP地址所在的绝对偏移量 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <returns></returns> 
        long LocateIP(byte[] ip)
        {
            var m = 0L;

            //比较第一个IP项 
            ReadIP(ipBegin, b4);

            var r = CompareIP(ip, b4);
            if (r == 0)
                return ipBegin;
            else if (r < 0)
                return -1;
            //开始二分搜索 
            for (long i = ipBegin, j = ipEnd; i < j;)
            {
                m = this.GetMiddleOffset(i, j);
                ReadIP(m, b4);
                r = CompareIP(ip, b4);
                if (r > 0)
                    i = m;
                else if (r < 0)
                {
                    if (m == j)
                    {
                        m = j -= IP_RECORD_LENGTH;
                    }
                    else
                    {
                        j = m;
                    }
                }
                else
                    return ReadLong3(m + 4);
            }
            m = ReadLong3(m + 4);
            ReadIP(m, b4);
            return (CompareIP(ip, b4) <= 0) ? m : -1;
        }

        /// <summary> 
        /// 从当前位置读取四字节,此四字节是IP地址 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <param name="ip"></param> 
        void ReadIP(long offset, byte[] ip)
        {
            ipFile.Position = offset;
            ipFile.Read(ip, 0, ip.Length);

            (ip[0], ip[3]) = (ip[3], ip[0]);
            (ip[1], ip[2]) = (ip[2], ip[1]);
        }

        /// <summary> 
        /// 比较IP地址是否相同 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <param name="beginIP"></param> 
        /// <returns>0:相等,1:ip大于beginIP,-1:小于</returns> 
        int CompareIP(byte[] ip, byte[] beginIP)
        {
            for (int i = 0; i < 4; i++)
            {
                var r = CompareByte(ip[i], beginIP[i]);
                if (r != 0)
                    return r;
            }
            return 0;
        }

        /// <summary> 
        /// 比较两个字节是否相等 
        /// </summary> 
        /// <param name="bsrc"></param> 
        /// <param name="bdst"></param> 
        /// <returns></returns> 
        int CompareByte(byte bsrc, byte bdst)
        {
            if ((bsrc & 0xFF) > (bdst & 0xFF))
                return 1;
            else if ((bsrc ^ bdst) == 0)
                return 0;
            else
                return -1;
        }

        /// <summary> 
        /// 从当前位置读取4字节,转换为长整型 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        long ReadLong4(long offset)
        {
            ipFile.Position = offset;

            var ret = 0L;
            ret |= ipFile.ReadByte() & (uint)0xFF;
            ret |= (ipFile.ReadByte() << 8) & (uint)0xFF00;
            ret |= (ipFile.ReadByte() << 16) & (uint)0xFF0000;
            ret |= (ipFile.ReadByte() << 24) & 0xFF000000;
            return ret;
        }

        /// <summary> 
        /// 根据当前位置,读取3字节 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        long ReadLong3(long offset)
        {
            var ret = 0L;
            ipFile.Position = offset;
            ret |= ipFile.ReadByte() & (uint)0xFF;
            ret |= (ipFile.ReadByte() << 8) & (uint)0xFF00;
            ret |= (ipFile.ReadByte() << 16) & (uint)0xFF0000;
            return ret;
        }

        /// <summary> 
        /// 从当前位置读取3字节 
        /// </summary> 
        /// <returns></returns> 
        long ReadLong3()
        {
            var ret = 0L;
            ret |= ipFile.ReadByte() & (uint)0xFF;
            ret |= (ipFile.ReadByte() << 8) & (uint)0xFF00;
            ret |= (ipFile.ReadByte() << 16) & (uint)0xFF0000;
            return ret;
        }

        /// <summary> 
        /// 取得begin和end中间的偏移 
        /// </summary> 
        /// <param name="begin"></param> 
        /// <param name="end"></param> 
        /// <returns></returns> 
        long GetMiddleOffset(long begin, long end)
        {
            var records = (end - begin) / IP_RECORD_LENGTH;
            records >>= 1;
            if (records == 0)
                records = 1;
            return begin + records * IP_RECORD_LENGTH;
        }
    }
}
