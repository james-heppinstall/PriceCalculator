using PriceCalulator.Domain;
using System.Collections.Generic;

namespace PriceCalulator.Repositories
{
    /// <summary>
    /// Class <c>SimpleProductRepository</c> is a hard-coded implementation of <c>IProductRepository</c>
    /// </summary>
    public class SimpleProductRepository : IProductRepository
    {
        /// <summary>
        /// Fetches all the products
        /// </summary>
        /// <returns>A list of products</returns>
        public List<Product> GetAll()
        {
            return new List<Product> {
                new Product{ProductName = "Beans", UnitPrice = 65},
                new Product{ProductName = "Bread", UnitPrice = 80},
                new Product{ProductName = "Milk", UnitPrice = 130},
                new Product{ProductName = "Apples", UnitPrice = 100}
            };
        }
    }
}