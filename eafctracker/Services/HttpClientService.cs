using Eafctracker.Services.Interfaces;

namespace Eafctracker.Services;

public class HttpClientService(IWebService webService ) : IHttpClientService
{
    
    private readonly List<HttpClient> _httpClients = await webService.CreateHttpClients(webService.CreateHandlers(webService.GetProxyList()));
    private int _currentIndex = 0;

    public HttpClient GetClientAsync()
    {
        HttpClient client = _httpClients[_currentIndex];

        _currentIndex = (_currentIndex + 1) % _httpClients.Count;

        return client;
    }
}