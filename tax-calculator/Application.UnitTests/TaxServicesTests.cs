using Moq;
using RestSharp;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Common;
using Application.Interfaces.TaxCalculators;
using Application.Dtos.TaxJar;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.TaxJar;
using Infrastructure.HttpClients.TaxJar;

namespace Application.UnitTests
{
    [TestClass]
    public class TaxServicesTests
    {
        private Mock<ITaxService> _mockTaxService;
        private Mock<ITaxJarHttpClient> _mockTaxJarHttpClient;

        protected readonly CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _mockTaxService = new Mock<ITaxService>();
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

            var outputDto = new OrderTaxOutputDto()
            {
                Rate = 0.07725,
                Shipping = 1.2,
                TaxSource = "destination",
                TaxableAmount = 17.2,
                AmountToCollect = 1.10,
                OrderTotalAmount = 17.2
            };

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(taxEntity);
            _mockTaxService.Setup(s => s.GetTaxJarOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(outputDto);

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Act
            var result = await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(outputDto.TaxSource, result.TaxSource);
            Assert.AreEqual(outputDto.TaxableAmount, result.TaxableAmount);
            Assert.AreEqual(outputDto.AmountToCollect, result.AmountToCollect);
            Assert.AreEqual(outputDto.OrderTotalAmount, result.OrderTotalAmount);
            Assert.AreEqual(outputDto.Shipping, result.Shipping);
            Assert.AreEqual(outputDto.Rate, result.Rate);
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

            var outputDto = new OrderTaxOutputDto()
            {
                Rate = 0,
                Shipping = 1.5,
                TaxSource = null,
                TaxableAmount = 0,
                AmountToCollect = 0,
                OrderTotalAmount = 16.5
            };

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(taxEntity);
            _mockTaxService.Setup(s => s.GetTaxJarOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(outputDto);

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Act
            var result = await service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(outputDto.TaxSource, result.TaxSource);
            Assert.AreEqual(outputDto.TaxableAmount, result.TaxableAmount);
            Assert.AreEqual(outputDto.AmountToCollect, result.AmountToCollect);
            Assert.AreEqual(outputDto.OrderTotalAmount, result.OrderTotalAmount);
            Assert.AreEqual(outputDto.Shipping, result.Shipping);
            Assert.AreEqual(outputDto.Rate, result.Rate);
        }
        
        [TestMethod]
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

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(new TaxEntity());

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Assert
            Assert.ThrowsExceptionAsync<TaxJarException>(() => service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken));
        }

        [TestMethod]
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

            _mockTaxJarHttpClient.Setup(s => s.GetOrderTax(orderTaxDto, It.IsAny<CancellationToken>())).ReturnsAsync(new TaxEntity());

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Assert
            Assert.ThrowsExceptionAsync<TaxJarException>(() => service.GetTaxJarOrderTax(orderTaxDto, _cancellationToken));
        }
        #endregion

        #region GetLocationTaxRate
        [TestMethod]
        public async Task GetLocationTaxRateForTaxableCity_ReturnsLocationTaxRate()
        {
            //Arrange
            var locationTaxRateDto = new LocationTaxRateParameterDto()
            {
                Country = "US",
                City = "Waterbury",
                Street = "400 Broad St",
                Zip = "03456"
            };

            var rate = new Rate
            {
                City = "WATERBURY",
                CityRate = "0.0",
                State = "CT",
                CombinedRate = "0.1223",
                Country = "US",
                CountryRate = "0.0",
                County = "NEW LONDON",
                CountyRate = "0.00",
                FreightTaxable = false,
                StateRate = "0.0325",
                Zip = "03456",
                CombinedDistrictRate = "0.01"
            };

            var rateEntity = new RateEntity()
            {
                Rate = rate
            };

            var outputDto = new LocationTaxRateOutputDto()
            {
                City = "WATERBURY",
                CityRate = "0.0",
                State = "CT",
                CombinedRate = "0.1223",
                Country = "US",
                CountryRate = "0.0",
                County = "NEW LONDON",
                CountyRate = "0.00",
                FreightTaxable = false,
                StateRate = "0.0325",
                Zip = "03456",
                CombinedDistrictRate = "0.01"
            };

            _mockTaxJarHttpClient.Setup(s => s.GetLocationTaxRate(locationTaxRateDto, It.IsAny<CancellationToken>())).ReturnsAsync(rateEntity);
            _mockTaxService.Setup(s => s.GetTaxJarLocationTaxRate(locationTaxRateDto, It.IsAny<CancellationToken>())).ReturnsAsync(outputDto);

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Act
            var result = await service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(outputDto.CityRate, result.CityRate);
            Assert.AreEqual(outputDto.City, result.City);
            Assert.AreEqual(outputDto.State, result.State);
            Assert.AreEqual(outputDto.CombinedRate, result.CombinedRate);
            Assert.AreEqual(outputDto.Country, result.Country);
            Assert.AreEqual(outputDto.CountryRate, result.CountryRate);
            Assert.AreEqual(outputDto.County, result.County);
            Assert.AreEqual(outputDto.CountyRate, result.CountyRate);
            Assert.AreEqual(outputDto.FreightTaxable, result.FreightTaxable);
            Assert.AreEqual(outputDto.StateRate, result.StateRate);
            Assert.AreEqual(outputDto.Zip, result.Zip);
            Assert.AreEqual(outputDto.CombinedDistrictRate, result.CombinedDistrictRate);
        }

        [TestMethod]
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

            _mockTaxJarHttpClient.Setup(s => s.GetLocationTaxRate(locationTaxRateDto, It.IsAny<CancellationToken>())).ReturnsAsync(new RateEntity());

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Assert
            Assert.ThrowsExceptionAsync<TaxJarException>(() => service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken));
        }

        [TestMethod]
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

            _mockTaxJarHttpClient.Setup(s => s.GetLocationTaxRate(locationTaxRateDto, It.IsAny<CancellationToken>())).ReturnsAsync(new RateEntity());

            var service = new TaxService(_mockTaxJarHttpClient.Object);

            //Assert
            Assert.ThrowsExceptionAsync<TaxJarException>(() => service.GetTaxJarLocationTaxRate(locationTaxRateDto, _cancellationToken));
        }
        #endregion
    }
}
