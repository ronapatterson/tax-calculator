using RestSharp;

namespace Infrastructure.Common
{
    internal interface IHttpClient
    {
        Task<RestResponse> Fetch(string baseUrl, RestRequest request, CancellationToken cancellationToken);
    }
}
