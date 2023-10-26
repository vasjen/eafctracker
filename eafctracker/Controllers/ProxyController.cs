using System.Net;
using Eafctracker.Models;
using Eafctracker.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eafctracker.Controllers;

[ApiController]
[Route("[controller]")]
public class ProxyController (WebService webService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProxiesList()
    {
        IAsyncEnumerable<WebProxy> proxies =  webService.GetProxyList();
        IAsyncEnumerable<HttpClientHandler> handlers = webService.CreateHandlers(proxies);
        IAsyncEnumerable<HttpClient> clients = webService.CreateHttpClients(handlers);
        IAsyncEnumerable<string> ipAddresses = webService.GetIpAddresses(clients);
        
        return Ok(ipAddresses);
    }
}