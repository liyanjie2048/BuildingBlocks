namespace Liyanjie.Http;

/// <summary>
/// 
/// </summary>
public abstract class Request
{
    HttpClient? client;

    internal HttpMethod Method { get; set; } = HttpMethod.Get;
    internal string? Url { get; set; }
    internal List<KeyValuePair<string, string>> Queries { get; set; } = [];
    internal List<KeyValuePair<string, string>> Headers { get; set; } = [];
    internal List<HttpContent> Contents { get; set; } = [];
    internal Request() { }

    internal HttpClient CreateHttpClient(TimeSpan timeout)
    {
        var httpClient = new HttpClient
        {
            Timeout = timeout
        };
        if (Headers != null)
            foreach (var item in Headers)
            {
                if (!httpClient.DefaultRequestHeaders.Contains(item.Key))
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        return httpClient;
    }

    internal Uri? CreateRequestUri()
    {
        if (Url is null)
            return default;

        var requestUrl = Url.IndexOf('?') < 0 ? $"{Url}?" : Url;
        if (Queries != null)
            foreach (var item in Queries)
            {
                requestUrl = $"{requestUrl}&{item.Key}={item.Value}";
            }
        return new Uri(requestUrl);
    }

    internal HttpContent? CreateHttpContent()
    {
        if (Contents != null)
            if (Contents.Count == 1)
                return Contents[0];
            else if (Contents.Count > 0)
            {
                var content = new MultipartContent();
                foreach (var item in Contents)
                {
                    content.Add(item);
                }
                return content;
            }
        return null;
    }

    public Request UseHttpClient(HttpClient client)
    {
        this.client = client;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeoutBySeconds"></param>
    /// <returns></returns>
    public virtual async Task<HttpResponseMessage> SendAsync(int timeoutBySeconds = 60)
    {
        if (client is not null)
        {
            return await SendAsync(this.client);
        }
        else
        {
            using var client = CreateHttpClient(TimeSpan.FromSeconds(timeoutBySeconds));
            return await SendAsync(client);
        }

        async Task<HttpResponseMessage> SendAsync(HttpClient client)
        {
            return await client.SendAsync(new HttpRequestMessage
            {
                Content = CreateHttpContent(),
                Method = Method,
                RequestUri = CreateRequestUri(),
            });
        }
    }
}
