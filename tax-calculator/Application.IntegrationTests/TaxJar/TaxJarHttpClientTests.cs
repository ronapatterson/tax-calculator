using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Infrastructure.Common;
using Application.Dtos.TaxJar;
using Infrastructure.HttpClients.TaxJar;
using Application.Interfaces.TaxCalculators;

namespace Application.IntegrationTests.TaxJar
{
    [TestClass]
    public class TaxJarHttpClientTests
    {
        private IHttpClient _httpClient;

        protected readonly IConfiguration _config;
        protected readonly CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        #region GetTaxForOrder
        [TestMethod]
        public async Task GetTaxForOrderWithTaxableProduct()
        {
            //Arrange
            var orderTaxDto = new OrderTaxParameterDto()
            {
                FromCountry = "US",
                FromZipCode = "07001",
                FromState = "NJ",
                ToCountry = "US",
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto(){Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                }
            };

            var taxJarClient = new TaxJarHttpClient(_config, _httpClient);

            //Act
            var result = await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Tax.AmountToCollect);
        }

        [TestMethod]
        public async Task GetTaxForOrderWithIncorrectCountry_ReturnsZeroTaxAmountToCollect()
        {
            //Arrange
            var orderTaxDto = new OrderTaxParameterDto()
            {
                FromCountry = "EU",
                FromZipCode = "07001",
                FromState = "NJ",
                ToCountry = "EU",
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto(){Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                }
            };

            var taxJarClient = new TaxJarHttpClient(_config, _httpClient);

            //Act
            var results = await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Tax.AmountToCollect);
        }

        [TestMethod]
        [ExpectedException(typeof(TaxJarException))]
        public async Task GetTaxForOrderWithInvalidParameter_ThrowsBadRequest()
        {
            //Arrange
            var orderTaxDto = new OrderTaxParameterDto()
            {
                FromCountry = "US",
                FromZipCode = "07001",
                FromState = "N", //invalid state
                ToCountry = "US",
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto(){Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                }
            };

            var taxJarClient = new TaxJarHttpClient(_config, _httpClient);

            //Act
            await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);
        }

        [TestMethod]
        [ExpectedException(typeof(TaxJarException))]
        public async Task GetTaxForOrderWithoutRequiredToCountry_ThrowsNotAcceptable()
        {
            //Arrange
            var orderTaxDto = new OrderTaxParameterDto()
            {
                FromCountry = "US",
                FromZipCode = "07001",
                FromState = "NJ",
                ToCountry = "", //missing country
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto(){Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                }
            };

            var taxJarClient = new TaxJarHttpClient(_config, _httpClient);

            //Act
            await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);
        }
        #endregion
    }
}