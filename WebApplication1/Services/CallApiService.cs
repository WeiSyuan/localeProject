using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class CallApiService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CallApiService(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public async Task<(string body, string header)> CallSelfApi()
        {
            var url = "https://localhost:5001/api/locale/Get/Request/Header/Locale";

            var client = _clientFactory.CreateClient("customer");

            ////測試用
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            _ = response.Headers.TryGetValues("locale", out var header);

            return (body, header.FirstOrDefault());
        }
    }
}
