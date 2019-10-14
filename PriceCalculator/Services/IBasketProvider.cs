using PriceCalulator.Domain;
using System;

namespace PriceCalulator.Services
{
    public interface IBasketProvider
    {
        Basket BuildBasket(string[] items, System.Collections.Generic.List<Product> products, Guid transactionId);
    }
}