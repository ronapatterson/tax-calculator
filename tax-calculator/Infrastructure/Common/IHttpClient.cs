using RestSharp;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("WebApi"),
    InternalsVisibleTo("Application.UnitTests"),
    InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Infrastructure.Common
{
    internal interface IHttpClient
    {
        Task<RestResponse> Fetch(string baseUrl, RestRequest request, CancellationToken cancellationToken);
    }
}
