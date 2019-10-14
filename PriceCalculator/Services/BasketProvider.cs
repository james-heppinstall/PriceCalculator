using System;
using System.Collections.Generic;
using System.Linq;
using PriceCalulator.Domain;
using Microsoft.Extensions.Logging;

namespace PriceCalulator.Services
{
    /// <summary>
    /// Class <c>BasketProvider</c> is responsible for creating a basket
    /// </summary>
    public class BasketProvider : IBasketProvider
    {
        private readonly ILogger<BasketProvider> _logger;

        public BasketProvider(ILogger<BasketProvider> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Build a basket from a list of items
        /// </summary>
        /// <param name="items">The items to add to the basket</param>
        /// <param name="products">A list of products available to add to a basket</param>
        /// <param name="transactionId">The unique identified for a single transaction</param>
        /// <returns></returns>
        public Basket BuildBasket(string[] items, List<Product> products, Guid transactionId)
        {
            var basket = new Basket();

            foreach (string item in items)
            {
                var product = products.FirstOrDefault(p => p.ProductName.ToUpperInvariant() == item.ToUpperInvariant());

                if (product != null)
                {
                    basket.AddProduct(product);
                }
                else
                {
                    throw new ArgumentException($"Unexpected item in the basket: { item }");
                }
            }

            _logger.LogInformation($"{transactionId} - Basket build competed.");

            return basket;
        }
    }
}