using PriceCalulator.Domain;
using Microsoft.Extensions.Logging;
using PriceCalculator.Services;
using PriceCalulator.Repositories;
using System;
using System.Linq;

namespace PriceCalulator.Services
{
    /// <summary>
    /// Class <c>SpecialOfferCalculator</c> calculates the special offers to be applied to a basket
    /// </summary>
    public class SpecialOfferCalculator : ISpecialOfferCalculator
    {
        private readonly ISpecialOfferRepository _specialOfferRepository;
        private readonly ILogger<BasketProvider> _logger;

        public SpecialOfferCalculator(ISpecialOfferRepository specialOfferRepository, ILogger<BasketProvider> logger)
        {
            _specialOfferRepository = specialOfferRepository;
            _logger = logger;
        }

        /// <summary>
        /// Calculate the offers by comparing products in the basket to available offers
        /// </summary>
        /// <param name="basket">The basket to applt the offers to.</param>
        /// <param name="transactionId">The unique identifier for the transaction</param>
        /// <returns></returns>
        public Basket CalculateOffers(Basket basket, Guid transactionId)
        {
            var offers = _specialOfferRepository.GetAll();

            foreach(Product product in basket.BasketItems.Keys)
            {
                foreach(SpecialOffer offer in offers.FindAll(o => o.PrimaryProduct == product.ProductName))
                {
                    Calculate(product, offer, basket, transactionId);
                }
            }

            return basket;
        }

        /// <summary>
        /// Perform the calulation on the basket
        /// </summary>      
        private void Calculate(Product product, SpecialOffer offer, Basket basket, Guid transactionId)
        {
            switch (offer.OfferType)
            {
                case "PercentageDiscount":
                    CalculatePercentageDiscount(product, offer, basket, transactionId);
                    break;
                case "BuyProductGetDiscountOnSecondProduct":
                    CalculateBuyProductGetDiscountOnSecondProductOffer(product, offer, basket, transactionId);
                    break;
                default:
                    throw new ArgumentException($"Unexpected special offer type: {offer.OfferType}");
            }
        }

        /// <summary>
        /// Calculate Percentage discount offers - A multibuy offer is one in which one or more of a single product 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="offer"></param>
        /// <param name="basket"></param>
        /// <param name="transactionId"></param>
        private void CalculatePercentageDiscount(Product product, SpecialOffer offer, Basket basket, Guid transactionId)
        {
            _logger.LogInformation($"{transactionId} - Calculating Percentage discount offer for {product.ProductName}");

            if(DateTime.Now < offer.ExpiryDate)
            {
                var itemCount = basket.BasketItems[product];
                var totalDiscount = itemCount * product.UnitPrice * ((float)offer.PercentageDiscount / 100);

                basket.AddOffer((int)totalDiscount, $"{product.ProductName} {offer.PercentageDiscount}% off: -{totalDiscount}p");
            }
        }

        private void CalculateBuyProductGetDiscountOnSecondProductOffer(Product product, SpecialOffer offer, Basket basket, Guid transactionId)
        {
            _logger.LogInformation($"{transactionId} - Calculating BuyProductGetDiscountOnSecondProductOffer for {product.ProductName}");

            int primaryItemCount;
            int SecondaryItemCount;

            primaryItemCount = basket.BasketItems[product];

            var secondaryProduct = basket.BasketItems.Keys.Where(k => k.ProductName == offer.SecondaryProduct).SingleOrDefault();

            if(secondaryProduct != null)
            {
                SecondaryItemCount = basket.BasketItems[secondaryProduct];

                // keep calculating offers until we run out of products that match
                while (primaryItemCount >= offer.PrimaryProductCount && SecondaryItemCount > 0)
                {
                    var totalDiscount = secondaryProduct.UnitPrice * ((float)offer.PercentageDiscount / 100);

                    basket.AddOffer((int)totalDiscount
                        , $"Buy {offer.PrimaryProductCount} {product.ProductName}, get {offer.PercentageDiscount}% off {secondaryProduct.ProductName}: -{totalDiscount}p");

                    primaryItemCount -= offer.PrimaryProductCount;
                    SecondaryItemCount--;
                }
            }
        }
    }
}