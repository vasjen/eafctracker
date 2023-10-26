using System.Net;
using Eafctracker.Models;
using Newtonsoft.Json;

namespace Eafctracker.Services;

public class ProxyService : IProxyService
{
    private readonly HttpClient _client;
    private readonly ILogger<ProxyService> _logger;

    public ProxyService(IHttpClientFactory clientFactory, ILogger<ProxyService> logger)
    {
        _client = clientFactory.CreateClient("proxy");
        _logger = logger;
    }

    public async Task<List<IWebProxy>> GetProxiesList()
    {
        string proxiesInJson = await _client.GetStringAsync(_client.BaseAddress);
        Proxies proxiesFromJson = JsonConvert.DeserializeObject<Proxies>(proxiesInJson);
        List<IWebProxy> Proxies = new List<IWebProxy>();
        int index = 0;
        foreach (Proxy item in proxiesFromJson.data.items)
        {
            var proxy = new WebProxy
            {
                Address = new Uri("http://" + item.ip + ":" + item.port_http),
                Credentials = new NetworkCredential(item.login, item.password)
            };
            HttpClientHandler handler = new HttpClientHandler
            {
                Proxy = proxy
            };
            Proxies.Add(proxy);
            var client = new HttpClient(handler: handler, disposeHandler: true);

            Console.WriteLine(
                "{0}: ip: {1}",++index,
            await client.GetStringAsync("https://api.ipify.org/"));
                
        }

        return Proxies;
    }
}

public interface IProxyService
{
    Task<List<IWebProxy>> GetProxiesList();
}