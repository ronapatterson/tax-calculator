using RestSharp;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("WebApi")]
namespace Infrastructure.Common
{
    internal interface IHttpClient
    {
        Task<RestResponse> Fetch(string baseUrl, RestRequest request, CancellationToken cancellationToken);
    }
}
