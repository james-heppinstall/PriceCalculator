using PriceCalulator.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PriceCalulator.Repositories;
using System;
using PriceCalulator.Domain;
using System.Collections.Generic;

namespace PriceCalculator.Tests
{
    public class SpecialOfferCalculatorTest
    {
        protected readonly Mock<ISpecialOfferRepository> MockSpecialOfferRepository;
        protected readonly Mock<ILogger<BasketProvider>> MockLogger;
        protected readonly SpecialOfferCalculator SpecialOfferCalculator;
        protected readonly Guid TransactionId;

        public SpecialOfferCalculatorTest()
        {
            MockSpecialOfferRepository = new Mock<ISpecialOfferRepository>();
            MockLogger = new Mock<ILogger<BasketProvider>>();
            SpecialOfferCalculator = new SpecialOfferCalculator(MockSpecialOfferRepository.Object, MockLogger.Object);
            TransactionId = Guid.NewGuid();
        }

        [Fact]
        public void Calculate_CallsGetAllOnProductRepository()
        {
            /// Arrange
            var basket = new Basket();
            basket.AddProduct(new Product { ProductName = "Apples" });
            MockSpecialOffers();

            // Act
            SpecialOfferCalculator.CalculateOffers(basket, TransactionId);

            // Assert
            MockSpecialOfferRepository.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public void Calculate_ThrowsException_WhenSpecialOfferHasAnInvalidOfferType()
        {
            // Arrange
            MockInvalidSpecialOffers();
            var basket = new Basket();
            basket.AddProduct(new Product { ProductName = "Apples" });

            // Act, Assert
            Exception ex = Assert.Throws<ArgumentException>(() => SpecialOfferCalculator.CalculateOffers(basket, TransactionId));
            Assert.Equal("Unexpected special offer type: InvalidOfferType", ex.Message);
        }

        [Fact]
        public void CalculateOffers_CorrectlyCalculatesPercentageDiscountOffer()
        {
            // Arrange
            MockSpecialOffers();
            var basket = new Basket();
            basket.AddProduct(new Product { ProductName = "Apples", UnitPrice = 100 });
            basket.AddProduct(new Product { ProductName = "Apples", UnitPrice = 100 });

            // Act
            SpecialOfferCalculator.CalculateOffers(basket, TransactionId);

            // Assert
            Assert.Equal(20, basket.TotalOfferDiscount);
        }

        [Fact]
        public void CalculateOffers_CorrectlyCalculatesBuyProductGetDiscountOnSecondProduct_ForSingleOffer()
        {
            // Arrange
            MockSpecialOffers();
            var basket = new Basket();
            var beansProduct = new Product { ProductName = "Beans", UnitPrice = 65 };
            var breadProduct = new Product { ProductName = "Bread", UnitPrice = 80 };
            basket.AddProduct(beansProduct);
            basket.AddProduct(beansProduct);
            basket.AddProduct(breadProduct);

            // Act
            SpecialOfferCalculator.CalculateOffers(basket, TransactionId);

            // Assert
            Assert.Equal(40, basket.TotalOfferDiscount);
        }

        [Fact]
        public void CalculateOffers_CorrectlyCalculatesBuyProductGetDiscountOnSecondProduct_ForPercentageDiscountOffer()
        {
            // Arrange
            MockSpecialOffers();
            var basket = new Basket();
            var beansProduct = new Product { ProductName = "Beans", UnitPrice = 65 };
            var breadProduct = new Product { ProductName = "Bread", UnitPrice = 80 };
            basket.AddProduct(beansProduct);
            basket.AddProduct(beansProduct);
            basket.AddProduct(beansProduct);
            basket.AddProduct(beansProduct);
            basket.AddProduct(breadProduct);
            basket.AddProduct(breadProduct);

            // Act
            SpecialOfferCalculator.CalculateOffers(basket, TransactionId);

            // Assert
            Assert.Equal(80, basket.TotalOfferDiscount);
        }

        private void MockSpecialOffers()
        {
            var offers = new List<SpecialOffer>
            {
                new SpecialOffer
                {
                    OfferType = "PercentageDiscount",
                    PrimaryProduct = "Apples",
                    PercentageDiscount = 10,
                    ExpiryDate = DateTime.Now.AddDays(7)
                },
                new SpecialOffer
                {
                    OfferType = "BuyProductGetDiscountOnSecondProduct",
                    PrimaryProduct = "Beans",
                    PrimaryProductCount = 2,
                    SecondaryProduct = "Bread",
                    PercentageDiscount = 50
                }
            };
            MockSpecialOfferRepository.Setup(mock => mock.GetAll()).Returns(offers);
        }

        private void MockInvalidSpecialOffers()
        {
            var offers = new List<SpecialOffer>
            {
                new SpecialOffer
                {
                    OfferType = "InvalidOfferType",
                    PrimaryProduct = "Apples",
                    PercentageDiscount = 10,
                    ExpiryDate = DateTime.Now.AddDays(7)
                }
            };
            MockSpecialOfferRepository.Setup(mock => mock.GetAll()).Returns(offers);
        }
    }
}
