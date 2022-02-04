using RestSharp;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;
using Application.Interfaces.TaxCalculators;

[assembly:InternalsVisibleTo("WebApi")]
namespace Infrastructure.HttpClients.TaxJar
{
    internal class TaxJarHttpClient : ITaxJarHttpClient
    {
        private readonly string _taxJarUrl;
        private readonly string _taxJarApiKey;

        private readonly Common.IHttpClient _httpClient;
        public TaxJarHttpClient(IConfiguration config, Common.IHttpClient httpClient)
        {
            config = new ConfigurationBuilder()
                .AddUserSecrets("cdd5709b-74de-4524-bccd-09e7dfeeca09")
                .Build();

            _httpClient = httpClient;
            _taxJarApiKey = config["ApiKeys:TaxJar"];
            _taxJarUrl = config["TaxCalculatorApis:TaxJar"];            
        }

        public async Task<RateEntity> GetLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{_taxJarUrl}/rates/{dto.Zip}", Method.Get)
                .AddQueryParameter("country", dto.Country)
                .AddQueryParameter("city", dto.City);

            if (!string.IsNullOrWhiteSpace(dto.Street))
                request.AddQueryParameter("street", dto.Street);

            var response = await Get(request, cancellationToken);

            checkForErrors(response);

            return JsonConvert.DeserializeObject<RateEntity>(response.Content);
        }

        public async Task<TaxEntity> GetOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{_taxJarUrl}/taxes", Method.Post)
                .AddJsonBody(new
                {
                    from_country = dto.FromCountry,
                    from_zip = dto.FromZipCode,
                    from_state = dto.FromState,
                    to_country = dto.ToCountry,
                    to_zip = dto.ToZip,
                    to_state = dto.ToState,
                    amount = dto.Amount,
                    shipping = dto.Shipping,
                    line_items = new List<LineItemDto>()
                    {
                        new LineItemDto() { Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                    }
                });

            var response = await Get(request, cancellationToken);

            checkForErrors(response);

            return JsonConvert.DeserializeObject<TaxEntity>(response.Content);
        }

        private async Task<RestResponse> Get(RestRequest request, CancellationToken cancellationToken)
        {
            request.AddHeader("Authorization", $"Token token={_taxJarApiKey}");
            request.AddHeader("Content-Type", "application/json");

            return await _httpClient.Fetch(_taxJarUrl, request, cancellationToken);            
        }

        private void checkForErrors(RestResponse response)
        {
            if(!response.IsSuccessful)
            {
                if (string.IsNullOrWhiteSpace(response.ErrorMessage))
                    throw new TaxJarException(response.Content);
                else
                    throw new TaxJarException(response.ErrorMessage, response.ErrorException);
            }
        }
    }
}
