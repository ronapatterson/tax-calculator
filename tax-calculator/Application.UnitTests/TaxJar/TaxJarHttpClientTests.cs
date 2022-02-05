using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Infrastructure.Common;
using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;
using Infrastructure.HttpClients.TaxJar;
using Application.Interfaces.TaxCalculators;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;

namespace Application.UnitTests.TaxJar
{
    [TestClass]
    public class TaxJarHttpClientTests
    {
        private string _baseUrl;       

        private Mock<IHttpClient> _httpClient;
        private Mock<IConfiguration> _mockConfig;
        private Mock<ITaxJarHttpClient> _mockTaxJarHttpClient;

        protected readonly CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _baseUrl = "https://api.taxjar.com/v2";
            _httpClient = new Mock<IHttpClient>();
            _mockConfig = new Mock<IConfiguration>();
            _mockTaxJarHttpClient = new Mock<ITaxJarHttpClient>();
        }

        #region GetTaxForOrder
        [TestMethod]
        public async Task GetTaxForOrderWithTaxableProduct_ReturnsOrderTax()
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
                        
            var tax = new Tax()
            {
                Rate = 0.07725,
                Shipping = 1.2,
                TaxSource = "destination",
                TaxableAmount = 17.2,
                AmountToCollect = 1.10,
                OrderTotalAmount = 17.2
            };

            var taxEntity = new TaxEntity()
            {
                Tax = tax
            };

            var jsonResult = JsonConvert.SerializeObject(taxEntity);

            var response = new RestResponse()
            {
                Content = jsonResult,
                IsSuccessful = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(taxEntity);            
            _httpClient.Setup(s => s.Fetch(_baseUrl, It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var taxJarClient = new TaxJarHttpClient(_mockConfig.Object, _httpClient.Object);

            //Act
            var result = await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(taxEntity.Tax.Rate, result.Tax.Rate);
            Assert.AreEqual(taxEntity.Tax.Shipping, result.Tax.Shipping);
            Assert.AreEqual(taxEntity.Tax.TaxSource, result.Tax.TaxSource);
            Assert.AreEqual(taxEntity.Tax.TaxableAmount, result.Tax.TaxableAmount);
            Assert.AreEqual(taxEntity.Tax.AmountToCollect, result.Tax.AmountToCollect);
            Assert.AreEqual(taxEntity.Tax.OrderTotalAmount, result.Tax.OrderTotalAmount);
        }

        [TestMethod]
        public async Task GetTaxForOrderWithIncorrectCountry_ReturnsZeroTaxAmountToCollect()
        {
            //Arrange
            var orderTaxDto = new OrderTaxParameterDto()
            {
                FromCountry = "EU", //unsupported country
                FromZipCode = "07001",
                FromState = "NJ",
                ToCountry = "EU", //unsupported country
                ToZip = "07446",
                ToState = "NJ",
                Amount = 16.50,
                Shipping = 1.5,
                LineItems = new List<LineItemDto>
                {
                    new LineItemDto(){Quantity = 1, UnitPrice = 15.0, ProductTaxCode = "31000"}
                }
            };

            var tax = new Tax()
            {
                Rate = 0,
                Shipping = 1.5,
                TaxSource = null,
                TaxableAmount = 0,
                AmountToCollect = 0,
                OrderTotalAmount = 16.5
            };

            var taxEntity = new TaxEntity()
            {
                Tax = tax
            };

            var jsonResult = JsonConvert.SerializeObject(taxEntity);

            var response = new RestResponse()
            {
                Content = jsonResult,
                IsSuccessful = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(taxEntity);
            _httpClient.Setup(s => s.Fetch(_baseUrl, It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var taxJarClient = new TaxJarHttpClient(_mockConfig.Object, _httpClient.Object);

            //Act
            var result = await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Tax.AmountToCollect);
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
                FromState = "N", //invalid state for zip
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

            string jsonResult = @"{
                'status': 400,
                'error': 'Bad Request',
                'detail': 'to_zip 07446 is not used within to_state N'
            }";

            var response = new RestResponse()
            {
                Content = jsonResult,
                IsSuccessful = false,
                ResponseStatus = ResponseStatus.Error,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            _httpClient.Setup(s => s.Fetch(_baseUrl, It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var taxJarClient = new TaxJarHttpClient(_mockConfig.Object, _httpClient.Object);

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

            string jsonResult = @"{
                'status': 406,
                'error': 'Not Acceptable',
                'detail': 'to_country must be a two-letter ISO code.'
            }";

            var response = new RestResponse()
            {
                Content = jsonResult,
                IsSuccessful = false,
                ResponseStatus = ResponseStatus.Error,
                StatusCode = System.Net.HttpStatusCode.NotAcceptable
            };

            _httpClient.Setup(s => s.Fetch(_baseUrl, It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var taxJarClient = new TaxJarHttpClient(_mockConfig.Object, _httpClient.Object);

            //Act
            await taxJarClient.GetOrderTax(orderTaxDto, _cancellationToken);
        }
        #endregion
    }
}