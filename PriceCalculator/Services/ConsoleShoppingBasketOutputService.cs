using Microsoft.Extensions.Logging;
using PriceCalulator.Domain;
using System;

namespace PriceCalculator.Services
{
    /// <summary>
    /// Class <c>ConsoleShoppingBasketOutputService</c> is responsible for outputting basket information to the console.
    /// </summary>
    public class ConsoleShoppingBasketOutputService : IShoppingBasketOutputService
    {
        private readonly ILogger<PriceCalculatorApplication> _logger;

        public ConsoleShoppingBasketOutputService(ILogger<PriceCalculatorApplication> logger)
        {
            _logger = logger;
        }

        public void OutputBasket(Basket basket)
        {
            Console.WriteLine($"Subtotal: {String.Format("£{0:0.00}", (decimal)basket.Subtotal / 100)}");

            if (basket.BasketOffers.Count == 0)
            {
                Console.WriteLine("(No offers available)");
            }
            else
            {
                foreach (BasketOffer offer in basket.BasketOffers)
                {
                    Console.WriteLine(offer.OfferDescription);
                }
            }

            Console.WriteLine($"Total: {String.Format("£{0:0.00}", (decimal)(basket.Subtotal - basket.TotalOfferDiscount) / 100)}");
        }

        public void OutputError(Guid transactionId, string errorMessage)
        {
            _logger.LogInformation($"{transactionId} - {errorMessage}");
            Console.WriteLine(errorMessage);
        }
    }
}
