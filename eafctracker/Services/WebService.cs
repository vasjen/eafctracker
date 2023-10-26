using System.Net;
using Eafctracker.Models;
using Newtonsoft.Json;

namespace Eafctracker.Services;

public class WebService(IHttpClientFactory clientFactory)
{
    private readonly HttpClient _client = clientFactory.CreateClient("proxy");

    public async IAsyncEnumerable<WebProxy> GetProxyList()
    {
        string proxiesInJson = await _client.GetStringAsync(_client.BaseAddress);
        Proxies? proxiesFromJson = JsonConvert.DeserializeObject<Proxies>(proxiesInJson);

        if (proxiesFromJson == null)
        {
            yield break;
        }

        foreach (Proxy item in proxiesFromJson.data.items)
        {
            yield return new WebProxy
            {
                Address = new Uri("http://" + item.ip + ":" + item.port_http),
                Credentials = new NetworkCredential(item.login, item.password)
            };
        }
    }
    
    public async IAsyncEnumerable<HttpClientHandler> CreateHandlers(IAsyncEnumerable<WebProxy> proxies)
    {
        await foreach (WebProxy proxy in proxies)
        {
            yield return new HttpClientHandler
            {
                Proxy = proxy
            };
        }
    }
    
    public async IAsyncEnumerable<HttpClient> CreateHttpClients(IAsyncEnumerable<HttpClientHandler> handlers)
    {
        await foreach (HttpClientHandler handler in handlers)
        {
            yield return new HttpClient(handler: handler, disposeHandler: true);
        }
        
    }

    public async IAsyncEnumerable<string> GetIpAddresses(IAsyncEnumerable<HttpClient> clients)
    {
        await foreach (HttpClient client in clients)
        {
            yield return await client.GetStringAsync("https://api.ipify.org/");
        }
    }
    
}