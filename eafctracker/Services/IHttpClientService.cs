namespace Eafctracker.Services
{
    public interface IHttpClientService
    {
        public HttpClient GetHttpClient();
        public int HandlerCount();
    }
}