using PriceCalulator.Domain;

namespace PriceCalculator.Services
{
    public interface ISpecialOfferCalculator
    {
        Basket CalculateOffers(Basket basket, System.Guid transactionId);
    }
}