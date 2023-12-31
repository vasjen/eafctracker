﻿using System.Net;

namespace Eafctracker.Services.Interfaces;

public interface IWebService
{
    IAsyncEnumerable<WebProxy> GetProxyList();
    IAsyncEnumerable<HttpClientHandler> CreateHandlers(IAsyncEnumerable<WebProxy> proxies);
    Task<List<HttpClient>> CreateHttpClients(IAsyncEnumerable<HttpClientHandler> handlers);
    IAsyncEnumerable<string> GetIpAddresses(IAsyncEnumerable<HttpClient> clients);
}