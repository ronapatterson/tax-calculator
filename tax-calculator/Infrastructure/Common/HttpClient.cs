using RestSharp;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("WebApi"),
    InternalsVisibleTo("Application.IntegrationTests")]
namespace Infrastructure.Common
{
    internal class HttpClient : IHttpClient
    {
        public HttpClient()
        {}

        //Made reusable for other tax calculator httpclient implementations
        public async Task<RestResponse> Fetch(string baseUrl, RestRequest request, CancellationToken cancellationToken)
        {
            var client = new RestClient(baseUrl);

            var response = await client.ExecuteAsync(request, cancellationToken);

            return response;
        }
    }
}
