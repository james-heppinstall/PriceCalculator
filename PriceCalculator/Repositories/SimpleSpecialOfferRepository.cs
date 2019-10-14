using PriceCalulator.Domain;
using System;
using System.Collections.Generic;

namespace PriceCalulator.Repositories
{
    /// <summary>
    /// Class <c>SimpleSpecialOfferRepository</c> is a hard-coded implementation of <c>ISpecialOfferRepository</c>
    /// </summary>
    public class SimpleSpecialOfferRepository : ISpecialOfferRepository
    {
        /// <summary>
        /// Fetches all the special offers
        /// </summary>
        /// <returns>A list of special offers</returns>
        public List<SpecialOffer> GetAll()
        {
            return new List<SpecialOffer> {
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
        }
    }
}