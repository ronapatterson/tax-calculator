using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Application.Dtos.TaxJar;
using Application.Interfaces.TaxCalculators;
using Infrastructure.HttpClients.TaxJar;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Common;
using System;

namespace Application.IntegrationTests
{
    [TestClass]
    public class TaxServiceTests
    {
        private IHttpClient _httpClient;
        private ITaxJarHttpClient _taxJarHttpClient;       

        protected readonly IConfiguration _config;
        protected readonly CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _taxJarHttpClient = new TaxJarHttpClient(_config, _httpClient);
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

            var service = new TaxService(_taxJarHttpClient);

            //Act
            var result = await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.AmountToCollect > 0);
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

            var service = new TaxService(_taxJarHttpClient);

            //Act
            var result = await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.AmountToCollect);
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

            var service = new TaxService(_taxJarHttpClient);

            //Act
            await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);
        }

        [TestMethod]
        [ExpectedException(typeof (TaxJarException))]
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

            var service = new TaxService(_taxJarHttpClient);

            //Act
            await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);
        }
        #endregion

        #region GetLocationTaxRate
        [TestMethod]
        public async Task GetLocationTaxRateForTaxableCity()
        {
            //Arrange
            var locationTaxRateDto = new LocationTaxRateParameterDto()
            {
                Country = "US",
                City = "Seattle",
                Street = "400 Broad St",
                Zip = "98109"
            };

            var service = new TaxService(_taxJarHttpClient);

            //Act
            var result = await service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken);    

            //Assert
            double stateRate = Convert.ToDouble(result.StateRate);

            Assert.IsNotNull(result);
            Assert.IsTrue(stateRate > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TaxJarException))]
        public async Task GetLocationTaxRateWithoutRequiredZip_ThrowsBadRequest()
        {
            //Arrange
            var locationTaxRateDto = new LocationTaxRateParameterDto()
            {
                Country = "US",
                City = "Seattle",
                Street = "400 Broad St",
                Zip = "" //missing required field
            };

            var service = new TaxService(_taxJarHttpClient);

            //Act
            await service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken);
        }

        [TestMethod]
        [ExpectedException(typeof(TaxJarException))]
        public async Task GetLocationTaxRateWithInvalidParameter_ThrowsBadRewuest()
        {
            //Arrange
            var locationTaxRateDto = new LocationTaxRateParameterDto()
            {
                Country = "EU", //Unsupported country EU
                City = "Seattle",
                Street = "400 Broad St",
                Zip = "98109"
            };

            var service = new TaxService(_taxJarHttpClient);

            //Act
            await service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken);
        }
        #endregion
    }
}
