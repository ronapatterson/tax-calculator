using RestSharp;

namespace Infrastructure.Common
{
    internal class HttpClient : IHttpClient
    {
        public HttpClient()
        {}

        public async Task<RestResponse> Fetch(string baseUrl, RestRequest request, CancellationToken cancellationToken)
        {
            var client = new RestClient(baseUrl);

            var response = await client.ExecuteAsync(request, cancellationToken);

            return response;
        }
    }
}
