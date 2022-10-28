namespace System.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public static class RSAHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static (Stream RSAPublicKeyXml, Stream RSAPrivateKeyXml) GenerateXmlKeys()
    {
        using var rsa = RSA.Create();
        return (SerializeRSAParameters(rsa.ExportParameters(false)), SerializeRSAParameters(rsa.ExportParameters(true)));
    }

    internal static Stream SerializeRSAParameters(RSAParameters parameters)
    {
        var stream = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(stream);
        new XmlSerializer(typeof(RSAParameters)).Serialize(xmlWriter, parameters);
        return stream;
    }

    internal static RSAParameters DeserializeRSAParameters(Stream xmlDocStream)
    {
        using var xmlReader = XmlReader.Create(xmlDocStream);
        return (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(xmlReader);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static (string RSAPublicKeyString, string RSAPrivateKeyString) GenerateStringKeys()
    {
        using var rsa = RSA.Create();
        return (SerializeRSAKey(rsa.ExportRSAPublicKey()), SerializeRSAKey(rsa.ExportRSAPrivateKey()));
    }

    internal static string SerializeRSAKey(byte[] keyBytes)
    {
        return Convert.ToBase64String(keyBytes);
    }

    internal static byte[] DeserializeRSAKey(string keyString)
    {
        return Convert.FromBase64String(keyString);
    }
}
