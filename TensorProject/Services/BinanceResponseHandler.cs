using System.Net.Http;
using System.Threading.Tasks;
using TensorProject.Services.IServices;

namespace TensorProject.Services
{
    public class BinanceResponseHandler : IBinanceResponseHandler
    {
        public async Task<string> HandleResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
