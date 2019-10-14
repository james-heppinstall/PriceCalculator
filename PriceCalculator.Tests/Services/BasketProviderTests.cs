using PriceCalulator.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using PriceCalulator.Domain;
using System.Linq;

namespace PriceCalculator.Tests
{
    public class BasketProviderTests
    {
        protected readonly BasketProvider BasketProvider;
        protected readonly Mock<ILogger<BasketProvider>> MockLogger;
        protected readonly Guid TransactionId;
        protected readonly List<Product> Products;

        public BasketProviderTests()
        {
            MockLogger = new Mock<ILogger<BasketProvider>>();
            BasketProvider = new BasketProvider(MockLogger.Object);
            TransactionId = Guid.NewGuid();

            Products = new List<Product> {
                new Product{ProductName = "Apples"},
                new Product{ProductName = "Milk"},
                new Product{ProductName = "Bread"}
            };
        }

        [Fact]
        public void BuildBasket_BuildsBasketCorrectly_WithNoDuplicatedItems()
        {
            // Arrange, 
            var basketItems = new[] { "Apples", "Milk", "Bread" };

            // Act
            var basket = BasketProvider.BuildBasket(basketItems, Products, TransactionId);

            // Assert
            Assert.Equal(3, basket.BasketItems.Count);
            Assert.Equal(1, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Apples").Value);
            Assert.Equal(1, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Milk").Value);
            Assert.Equal(1, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Bread").Value);
        }

        [Fact]
        public void BuildBasket_BuildsBasketCorrectly_WithDuplicatedItems()
        {
            // Arrange, 
            var basketItems = new[] { "Apples", "Milk", "Milk", "Apples", "Bread", "Apples" };

            // Act
            var basket = BasketProvider.BuildBasket(basketItems, Products, TransactionId);

            // Assert
            Assert.Equal(3, basket.BasketItems.Count);
            Assert.Equal(3, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Apples").Value);
            Assert.Equal(2, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Milk").Value);
            Assert.Equal(1, basket.BasketItems.SingleOrDefault(p => p.Key.ProductName == "Bread").Value);
        }

        [Fact]
        public void BuildBasket_ThrowsException_IfItemIsNotAValidProduct()
        {
            // Arrange, 
            var basketItems = new[] { "Apples", "Milk", "Grapes" };

            // Act, Assert
            Exception ex = Assert.Throws<ArgumentException>(() => BasketProvider.BuildBasket(basketItems, Products, TransactionId));
            Assert.Equal("Unexpected item in the basket: Grapes", ex.Message);
        }
    }
}
