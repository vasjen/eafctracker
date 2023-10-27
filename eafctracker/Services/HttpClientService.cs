using Eafctracker.Services.Interfaces;

namespace Eafctracker.Services;

public class HttpClientService(IWebService webService ) : IHttpClientService
{
    
    private readonly List<HttpClient> _httpClients =  webService.CreateHttpClients(webService.CreateHandlers(webService.GetProxyList())).Result;
    private int _currentIndex = 0;

    public HttpClient GetClient()
    {
        HttpClient client = _httpClients[_currentIndex];
        client.DefaultRequestHeaders
            .Add("User-Agent","User Agent	Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko)");

        _currentIndex = (_currentIndex + 1) % _httpClients.Count;

        return client;
    }
}