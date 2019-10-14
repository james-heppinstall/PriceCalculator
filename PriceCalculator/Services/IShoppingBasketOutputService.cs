using PriceCalulator.Domain;

namespace PriceCalculator.Services
{
    public interface IShoppingBasketOutputService
    {
        void OutputBasket(Basket basket);
        void OutputError(System.Guid transactionId, string errorMessage);
    }
}
