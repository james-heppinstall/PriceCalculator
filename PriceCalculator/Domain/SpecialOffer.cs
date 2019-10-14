using System;

namespace PriceCalulator.Domain
{
    /// <summary>
    /// Class <c>SpecialOffer</c> represents a special offer
    /// </summary>
    public class SpecialOffer
    {
        public string OfferType { get; set; }
        public string PrimaryProduct { get; set; }
        public int PrimaryProductCount { get; set; }
        public string SecondaryProduct { get; set; }
        public int PercentageDiscount { get;  set; }
        public DateTime ExpiryDate { get; set; }
    }
}