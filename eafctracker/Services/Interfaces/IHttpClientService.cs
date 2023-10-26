namespace Eafctracker.Services.Interfaces
{
    public interface IHttpClientService
    {
        public HttpClient GetHttpClient();
        public int HandlerCount();
    }
}