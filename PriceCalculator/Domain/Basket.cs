using System.Collections.Generic;

namespace PriceCalulator.Domain
{
    /// <summary>
    /// Class <c>Basket</c> represents a basket of shopping.
    /// </summary>
    public class Basket
    {
        public readonly Dictionary<Product, int> BasketItems = new Dictionary<Product, int>();
        public readonly List<BasketOffer> BasketOffers = new List<BasketOffer>();

        public void AddProduct(Product product)
        {
            if (BasketItems.ContainsKey(product))
            {
                BasketItems[product]++;
            }
            else
            {
                BasketItems.Add(product, 1);
            }
        }

        /// <summary>
        /// Add a calculated off to the basket
        /// </summary>
        /// <param name="totalDiscount">The discount for this offer</param>
        /// <param name="OfferDescription">A description of the offer</param>
        public void AddOffer(int totalDiscount, string OfferDescription)
        {
            BasketOffers.Add(new BasketOffer { TotalDiscount = totalDiscount, OfferDescription = OfferDescription });
        }

        /// <summary>
        /// Returns the total for the basket BEFORE offers have been applied.
        /// </summary>
        public int Subtotal { get 
            {
                int subTotal = 0;
                foreach (KeyValuePair<Product, int> product in BasketItems)
                {
                    subTotal += product.Key.UnitPrice * product.Value;
                }

                return subTotal;
            }
        }

        /// <summary>
        /// Returns the total discount for all the offers in the basket
        /// </summary>
        public int TotalOfferDiscount
        {
            get
            {
                int offerDiscount = 0;
                foreach (BasketOffer offer in BasketOffers)
                {
                    offerDiscount += offer.TotalDiscount;
                }

                return offerDiscount;
            }
        }
    }

    /// <summary>
    /// Class <c>BasketOffer</c> represents an instance of an offer.
    /// </summary>
    public class BasketOffer
    {
        public int TotalDiscount { get; set; }
        public string OfferDescription { get; set; }
    }
}