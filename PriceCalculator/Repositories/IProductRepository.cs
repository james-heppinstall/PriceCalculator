using PriceCalulator.Domain;
using System.Collections.Generic;

namespace PriceCalulator.Repositories
{
    /// <summary>
    /// Interface <c>IProductRepository</c> provides access to Product data
    /// </summary>
    public interface IProductRepository
    {
        List<Product> GetAll();
    }
}