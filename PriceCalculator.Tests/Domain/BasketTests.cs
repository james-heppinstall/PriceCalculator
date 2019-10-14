using PriceCalulator.Domain;
using Xunit;

namespace PriceCalculator.Tests.Domain
{
    public class BasketTests
    {
        [Fact]
        public void Basket_CalculatesSubTotal()
        {
            // Arrange, 
            var basket = new Basket();
            basket.AddProduct(new Product { ProductName = "Apples", UnitPrice = 100 });
            basket.AddProduct(new Product { ProductName = "Beans", UnitPrice = 65 });
            basket.AddProduct(new Product { ProductName = "Milk", UnitPrice = 130 });

            // Act, Assert
            Assert.Equal(295, basket.Subtotal);
        }

        [Fact]
        public void Basket_CalculatesTotalDiscount()
        {
            // Arrange, 
            var basket = new Basket();
            basket.BasketOffers.Add(new BasketOffer { TotalDiscount = 40 });
            basket.BasketOffers.Add(new BasketOffer { TotalDiscount = 40 });
            basket.BasketOffers.Add(new BasketOffer { TotalDiscount = 10 });

            // Act, Assert
            Assert.Equal(90, basket.TotalOfferDiscount);
        }


    }
}
