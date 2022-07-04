using System.Net.Http;

namespace Liyanjie.Http;

/// <summary>
/// 
/// </summary>
public class Request_Base : Request_QueryAdded
{
    internal Request_Base(HttpMethod method, string url)
    {
        Method = method;
        Url = url;
    }
}
