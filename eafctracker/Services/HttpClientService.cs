using System.Net;

namespace Eafctracker.Services {
    public class HttpClientService : IHttpClientService {
         private Random rnd = new(); 
         public IEnumerable<HttpClient> Clients {get; private set;}
         public HttpClientService()
         {  
          
           
            List<HttpClient> AllClients = new();
           
            this.Clients=AllClients;
         }
         public HttpClient GetHttpClient() =>
             Clients.ElementAt(rnd.Next(Clients.Count()));

        public int HandlerCount()
        {
            return Clients.Count();
        }
    }
    
}