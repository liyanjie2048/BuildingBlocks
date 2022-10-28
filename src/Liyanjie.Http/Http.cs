namespace Liyanjie.Http;

/// <summary>
/// 
/// </summary>
public class Http
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static Request_Base Do(HttpMethod method, string url)
    {
        return new Request_Base(method, url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static Request_Base Get(string url)
    {
        return new Request_Base(HttpMethod.Get, url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static Request_Base Post(string url)
    {
        return new Request_Base(HttpMethod.Post, url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static Request_Base Put(string url)
    {
        return new Request_Base(HttpMethod.Put, url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static Request_Base Delete(string url)
    {
        return new Request_Base(HttpMethod.Delete, url);
    }
}
