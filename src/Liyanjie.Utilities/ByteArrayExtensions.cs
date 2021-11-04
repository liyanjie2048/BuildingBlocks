using System.IO;
using System.Numerics;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trimHyphen"></param>
        /// <returns></returns>
        public static string ToString(this byte[] input, bool trimHyphen)
        {
            var output = BitConverter.ToString(input);
            return trimHyphen
                ? output.Replace("-", "")
                : output;
        }
    }
}
namespace System.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodeMode"></param>
        /// <returns></returns>
        public static byte[] Encode(this byte[] input, EncodeType encodeMode)
        {
            using HashAlgorithm encoder = encodeMode switch
            {
                EncodeType.MD5 => MD5.Create(),
                EncodeType.SHA1 => SHA1.Create(),
                EncodeType.SHA256 => SHA256.Create(),
                EncodeType.SHA384 => SHA384.Create(),
                EncodeType.SHA512 => SHA512.Create(),
                _ => throw new ArgumentOutOfRangeException(nameof(encodeMode)),
            };
            return encoder?.ComputeHash(input);
        }

        #region Aes

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(this byte[] input, string key, string iv = null)
        {
            using var aes = CreateAes(key, iv);
            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(this byte[] input, string key, string iv = null)
        {
            using var aes = CreateAes(key, iv);
            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(input, 0, input.Length);
        }

        static Aes CreateAes(string key, string iv)
        {
            var aes = Aes.Create();
            aes.Mode = string.IsNullOrWhiteSpace(iv) ? CipherMode.ECB : CipherMode.CBC;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Padding = PaddingMode.PKCS7;
            if (!string.IsNullOrWhiteSpace(iv))
                aes.IV = Encoding.UTF8.GetBytes(iv);
            return aes;
        }

        #endregion

        #region TripleDES

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static byte[] TripleDESEncrypt(this byte[] input, string key, string iv = null)
        {
            using var tripleDES = CreateTripleDES(key, iv);
            using var encryptor = tripleDES.CreateEncryptor();
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static byte[] TripleDESDecrypt(this byte[] input, string key, string iv = null)
        {
            using var tripleDES = CreateTripleDES(key, iv);
            using var decryptor = tripleDES.CreateDecryptor();
            return decryptor.TransformFinalBlock(input, 0, input.Length);
        }

        static TripleDES CreateTripleDES(string key, string iv)
        {
            var tripleDES = TripleDES.Create();
            tripleDES.Mode = string.IsNullOrWhiteSpace(iv) ? CipherMode.ECB : CipherMode.CBC;
            tripleDES.Key = Encoding.UTF8.GetBytes(key);
            tripleDES.Padding = PaddingMode.PKCS7;
            if (!string.IsNullOrWhiteSpace(iv))
                tripleDES.IV = Encoding.UTF8.GetBytes(iv);
            return tripleDES;
        }

        #endregion

        #region RSA

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKey_xml">公钥</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] input,
            Stream publicKey_xml)
            => _RSAEncrypt(input, publicKey_xml);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey_xml">私钥</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] input,
            Stream privateKey_xml)
            => _RSADecrypt(input, privateKey_xml);

#if NETSTANDARD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKey_xml">密钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] input, Stream publicKey_xml, RSAEncryptionPadding encryptionPadding)
            => _RSAEncrypt(input, publicKey_xml, encryptionPadding);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey_xml">密钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] input, Stream privateKey_xml, RSAEncryptionPadding encryptionPadding)
            => _RSADecrypt(input, privateKey_xml, encryptionPadding);
#endif

        static byte[] _RSAEncrypt(this byte[] input,
            Stream publicKey_xml
#if NETSTANDARD
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
#endif
            using var rsa = CreateRSAByXmlKey(publicKey_xml);
            var bufferSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (input.Length <= bufferSize)
            {
                return
#if NETSTANDARD
                    rsa.Encrypt(input, encryptionPadding)
#elif NETFRAMEWORK
                    rsa.EncryptValue(input)
#endif
                    ;
            }
            else
            {
                using var originalStream = new MemoryStream(input);
                using var encryptedStream = new MemoryStream();
                var buffer = new byte[bufferSize];
                var readSize = originalStream.Read(buffer, 0, bufferSize);

                while (readSize > 0)
                {
                    var tmpBuffer = new byte[readSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, readSize);
                    var tmpEncrypted =
#if NETSTANDARD
                        rsa.Encrypt(tmpBuffer, encryptionPadding)
#elif NETFRAMEWORK
                        rsa.EncryptValue(tmpBuffer)
#endif
                        ;
                    encryptedStream.Write(tmpEncrypted, 0, tmpEncrypted.Length);

                    readSize = originalStream.Read(buffer, 0, bufferSize);
                }
                return encryptedStream.ToArray();
            }
        }

        static byte[] _RSADecrypt(this byte[] input,
            Stream privateKey_xml
#if NETSTANDARD
            , RSAEncryptionPadding encryptionPadding = null
#endif
            )
        {
#if NETSTANDARD
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
#endif
            using var rsa = CreateRSAByXmlKey(privateKey_xml);
            var maxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

            if (input.Length <= maxBlockSize)
            {
                return
#if NETSTANDARD
                    rsa.Decrypt(input, encryptionPadding)
#elif NETFRAMEWORK
                    rsa.DecryptValue(input)
#endif
                    ;
            }
            else
            {
                using var encryptedStream = new MemoryStream(input);
                using var decryptedStream = new MemoryStream();
                var buffer = new byte[maxBlockSize];
                var blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);

                while (blockSize > 0)
                {
                    var tmpBuffer = new byte[blockSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, blockSize);
                    var tmpDecrypted =
#if NETSTANDARD
                        rsa.Decrypt(tmpBuffer, encryptionPadding)
#elif NETFRAMEWORK
                        rsa.DecryptValue(tmpBuffer)
#endif
                        ;
                    decryptedStream.Write(tmpDecrypted, 0, tmpDecrypted.Length);

                    blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);
                }
                return decryptedStream.ToArray();
            }
        }

        static RSA CreateRSAByXmlKey(Stream key_xml)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(RSAHelper.DeserializeRSAParameters(key_xml));
            return rsa;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey_xml"></param>
        /// <param name="hashAlgorithmName"></param>
        /// <param name="rsaSignaturePadding"></param>
        /// <returns></returns>
        public static byte[] RSASign(this byte[] input,
            Stream privateKey_xml,
            HashAlgorithmName hashAlgorithmName,
            RSASignaturePadding rsaSignaturePadding = default)
        {
            using var rsa = CreateRSAByXmlKey(privateKey_xml);
            return rsa.SignData(input, hashAlgorithmName, rsaSignaturePadding ?? RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey_xml"></param>
        /// <param name="hashAlgorithmName"></param>
        /// <param name="rsaSignaturePadding"></param>
        /// <returns></returns>
        public static bool RSAVerify(this byte[] input,
            byte[] signature,
            Stream publicKey_xml,
            HashAlgorithmName hashAlgorithmName,
            RSASignaturePadding rsaSignaturePadding = default)
        {
            using var rsa = CreateRSAByXmlKey(publicKey_xml);
            return rsa.VerifyData(input, signature, hashAlgorithmName, rsaSignaturePadding ?? RSASignaturePadding.Pkcs1);
        }

#if NETSTANDARD
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKey_str">公钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] input,
            string publicKey_str,
            RSAEncryptionPadding encryptionPadding = default)
        {
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            using var rsa = CreateRSAByStringKey(publicKey_str, null);
            var bufferSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (input.Length <= bufferSize)
            {
                return rsa.Encrypt(input, encryptionPadding);
            }
            else
            {
                using var originalStream = new MemoryStream(input);
                using var encryptedStream = new MemoryStream();
                var buffer = new byte[bufferSize];
                var readSize = originalStream.Read(buffer, 0, bufferSize);

                while (readSize > 0)
                {
                    var tmpBuffer = new byte[readSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, readSize);
                    var tmpEncrypted =
                        rsa.Encrypt(tmpBuffer, encryptionPadding)
                        ;
                    encryptedStream.Write(tmpEncrypted, 0, tmpEncrypted.Length);

                    readSize = originalStream.Read(buffer, 0, bufferSize);
                }
                return encryptedStream.ToArray();
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey_str">私钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] input,
            string privateKey_str,
            RSAEncryptionPadding encryptionPadding = default)
        {
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            using var rsa = CreateRSAByStringKey(null, privateKey_str);
            var maxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

            if (input.Length <= maxBlockSize)
            {
                return rsa.Decrypt(input, encryptionPadding);
            }
            else
            {
                using var encryptedStream = new MemoryStream(input);
                using var decryptedStream = new MemoryStream();
                var buffer = new byte[maxBlockSize];
                var blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);

                while (blockSize > 0)
                {
                    var tmpBuffer = new byte[blockSize];
                    Array.Copy(buffer, 0, tmpBuffer, 0, blockSize);
                    var tmpDecrypted =
                        rsa.Decrypt(tmpBuffer, encryptionPadding)
                        ;
                    decryptedStream.Write(tmpDecrypted, 0, tmpDecrypted.Length);

                    blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);
                }
                return decryptedStream.ToArray();
            }
        }

        static RSA CreateRSAByStringKey(string publicKey_str, string privateKey_str)
        {
            if (publicKey_str.IsNullOrWhiteSpace() && privateKey_str.IsNullOrWhiteSpace())
                throw new ArgumentException("No keys.");

            var rsa = RSA.Create();
            if (publicKey_str.IsNotNullOrEmpty())
                rsa.ImportRSAPublicKey(new ReadOnlySpan<byte>(RSAHelper.DeserializeRSAKey(publicKey_str)), out _);
            if (privateKey_str.IsNotNullOrEmpty())
                rsa.ImportRSAPrivateKey(new ReadOnlySpan<byte>(RSAHelper.DeserializeRSAKey(privateKey_str)), out _);
            return rsa;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKey_str">私钥</param>
        /// <param name="hashAlgorithmName"></param>
        /// <param name="rsaSignaturePadding"></param>
        /// <returns></returns>
        public static byte[] RSASign(this byte[] input,
            string privateKey_str,
            HashAlgorithmName hashAlgorithmName,
            RSASignaturePadding rsaSignaturePadding = default)
        {
            using var rsa = CreateRSAByStringKey(null, privateKey_str);
            return rsa.SignData(input, hashAlgorithmName, rsaSignaturePadding ?? RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey_str">公钥</param>
        /// <param name="hashAlgorithmName"></param>
        /// <param name="rsaSignaturePadding"></param>
        /// <returns></returns>
        public static bool RSAVerify(this byte[] input,
            byte[] signature,
            string publicKey_str,
            HashAlgorithmName hashAlgorithmName,
            RSASignaturePadding rsaSignaturePadding = default)
        {
            using var rsa = CreateRSAByStringKey(publicKey_str, null);
            return rsa.VerifyData(input, signature, hashAlgorithmName, rsaSignaturePadding ?? RSASignaturePadding.Pkcs1);
        }
#endif
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EncodeType
    {
        /// <summary>
        /// 
        /// </summary>
        MD5,

        /// <summary>
        /// 
        /// </summary>
        SHA1,

        /// <summary>
        /// 
        /// </summary>
        SHA256,

        /// <summary>
        /// 
        /// </summary>
        SHA384,

        /// <summary>
        /// 
        /// </summary>
        SHA512,
    }
}
