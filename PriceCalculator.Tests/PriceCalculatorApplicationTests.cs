using System;
using System.Collections.Generic;
using System.Linq;
using PriceCalulator.Domain;
using PriceCalulator.Repositories;
using PriceCalulator.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PriceCalculator.Services;

namespace PriceCalculator.Tests
{
    public class PriceCalculatorApplicationTests
    {
        protected readonly PriceCalculatorApplication PriceCalculator;
        protected readonly Mock<IShoppingBasketOutputService> MockShoppingBasketOutputService;
        protected readonly Mock<IProductRepository> MockProductRepository;
        protected readonly Mock<IBasketProvider> MockBasketProvider;
        protected readonly Mock<ISpecialOfferCalculator> MockSpecialOfferCalculator;
        protected readonly Mock<ILogger<PriceCalculatorApplication>> MockLogger;

        public PriceCalculatorApplicationTests()
        {
            MockShoppingBasketOutputService = new Mock<IShoppingBasketOutputService>();
            MockProductRepository = new Mock<IProductRepository>();
            MockBasketProvider = new Mock<IBasketProvider>();
            MockSpecialOfferCalculator = new Mock<ISpecialOfferCalculator>();
            MockLogger = new Mock<ILogger<PriceCalculatorApplication>>();

            PriceCalculator = new PriceCalculatorApplication(
                MockShoppingBasketOutputService.Object,
                MockProductRepository.Object,
                MockBasketProvider.Object,
                MockSpecialOfferCalculator.Object,
                MockLogger.Object);

            MockProductRepositoryWithProducts();
        }

        [Fact]
        public void Calculate_CallsOutputErrorOnShoppingBasketOutputService_WhenBasketIsEmpty()
        {
            // Arrange, Act
            PriceCalculator.Calculate(getEmptyBasket());

            // Assert
            MockShoppingBasketOutputService.Verify(mock => mock.OutputError(It.IsAny<Guid>(), "Basket contains no items."), Times.Once);
        }

        [Fact]
        public void Calculate_CallsGetAllOnProductRepository_WhenBasketIsNotEmpty()
        {
            // Arrange, Act
            PriceCalculator.Calculate(getNonEmptyBasket());

            // Assert
            MockProductRepository.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public void Calculate_CallsBuildBasketOnBasketProvider_WhenBasketIsNotEmpty()
        {
            // Arrange
            var itemCapture = new List<string[]>();
            var productCapture = new List<List<Product>>();
            MockBasketProvider.Setup(mock => mock.BuildBasket(Capture.In(itemCapture), Capture.In(productCapture), It.IsAny<Guid>()));

            // Act
            PriceCalculator.Calculate(getNonEmptyBasket());

            // Assert
            MockBasketProvider.Verify(mock => mock.BuildBasket(It.IsAny<string[]>(), It.IsAny<List<Product>>(), It.IsAny<Guid>()), Times.Once);

            Assert.Equal("Apples", itemCapture.First()[0]);
            Assert.Equal("Milk", itemCapture.First()[1]);
            Assert.Equal("Bread", itemCapture.First()[2]);

            Assert.Contains(productCapture.First(), p => p.ProductName == "Apples");
            Assert.Contains(productCapture.First(), p => p.ProductName == "Milk");
            Assert.Contains(productCapture.First(), p => p.ProductName == "Bread");
        }

        [Fact]
        public void Calculate_CallsOutputErrorOnShoppingBasketOutputService_WhenBasketProviderThrowsException()
        {
            // Arrange
            MockBasketProvider.Setup(mock => mock.BuildBasket(It.IsAny<string[]>(), It.IsAny<List<Product>>(), It.IsAny<Guid>()))
                .Throws(new ArgumentException("Unexpected item in the basket: Grapes"));

            //Act
            PriceCalculator.Calculate(getBasketWithUnknownItem());

            // Assert
            MockShoppingBasketOutputService.Verify(mock => mock.OutputError(It.IsAny<Guid>(), "Unexpected item in the basket: Grapes"), Times.Once);
        }

        [Fact]
        public void Calculate_CallsCalculateOffersOnSpecialOfferCalculator_WhenBasketIsNotEmpty()
        {
            // Arrange
            var basket = new Basket();
            basket.AddProduct(new Product { ProductName = "Beans", UnitPrice = 65 });
            
            MockBasketProvider.Setup(mock => mock.BuildBasket(It.IsAny<string[]>(), It.IsAny<List<Product>>(), It.IsAny<Guid>())).Returns(basket);

            // Act
            PriceCalculator.Calculate(getNonEmptyBasket());

            // Assert
            MockSpecialOfferCalculator.Verify(mock => mock.CalculateOffers(basket, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Calculate_CallsOutputBasketOnShoppingBasketOutputService_WhenBasketIsNotEmpty()
        {
            // Arrange, Act
            PriceCalculator.Calculate(getNonEmptyBasket());

            // Assert
            MockShoppingBasketOutputService.Verify(mock => mock.OutputBasket(It.IsAny<Basket>()), Times.Once);
        }

        private string[] getNonEmptyBasket()
        {
            return new[] { "Apples", "Milk", "Bread" };
        }

        private string[] getEmptyBasket()
        {
            return new string[0];
        }

        private string[] getBasketWithUnknownItem()
        {
            return new[] { "Apples", "Milk", "Bread", "Grapes" };
        }
        private void MockProductRepositoryWithProducts()
        {
            var products = new List<Product> {
                new Product{ProductName = "Apples"},
                new Product{ProductName = "Milk"},
                new Product{ProductName = "Bread"}
            };
            MockProductRepository.Setup(r => r.GetAll()).Returns(products);
        }
    }
}
