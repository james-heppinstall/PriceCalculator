using PriceCalulator.Domain;
using System.Collections.Generic;

namespace PriceCalulator.Repositories
{
    /// <summary>
    /// Interface <c>ISpecialOfferRepository</c> provides access to SpecialOffer data
    /// </summary>
    public interface ISpecialOfferRepository
    {
        List<SpecialOffer> GetAll();
    }
}