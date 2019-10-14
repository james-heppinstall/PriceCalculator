using PriceCalulator.Repositories;
using PriceCalulator.Services;
using Microsoft.Extensions.Logging;
using System;
using PriceCalculator.Services;

namespace PriceCalculator
{
    /// <summary>
    /// Class <c>PriceCalculator</c> calculates the price of a basket of shopping.
    /// </summary>
    public class PriceCalculatorApplication
    {
        private readonly IShoppingBasketOutputService _shoppingBasketOutputService;
        private readonly IProductRepository _productRepository;
        private readonly IBasketProvider _basketProvider;
        private readonly ISpecialOfferCalculator _specialOfferCalculator;
        private readonly ILogger<PriceCalculatorApplication> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceCalculatorApplication"/> class.
        /// </summary>
        /// <param name="shoppingBasketOutputService">Service for outputting messages from the PriceCalculator</param>
        /// <param name="productRepository">The product repository</param>
        /// <param name="basketProvider">Provider for creating a shopping basket</param>
        /// <param name="logger"></param>
        public PriceCalculatorApplication(
            IShoppingBasketOutputService shoppingBasketOutputService,
            IProductRepository productRepository,
            IBasketProvider basketProvider,
            ISpecialOfferCalculator specialOfferCalculator,
            ILogger<PriceCalculatorApplication> logger)
        {
            _shoppingBasketOutputService = shoppingBasketOutputService;
            _productRepository = productRepository;
            _basketProvider = basketProvider;
            _specialOfferCalculator = specialOfferCalculator;
            _logger = logger;
        }

        /// <summary>
        /// Calculate the cost of the items specified in the <c>args</c> parameter.
        /// </summary>
        /// <param name="args">The shopping basket items.</param>
        public void Calculate(string[] args)
        {
            var transactionId = Guid.NewGuid();

            _logger.LogInformation($"{transactionId} - Starting price calculation for items: {string.Join(", ", args)}");

            if (args.Length == 0)
            {
                _shoppingBasketOutputService.OutputError(transactionId, "Basket contains no items.");
                return;
            }

            try
            {
                var products = _productRepository.GetAll();
                var basket = _basketProvider.BuildBasket(args, products, transactionId);
                var pricedBasket = _specialOfferCalculator.CalculateOffers(basket, transactionId);
                _shoppingBasketOutputService.OutputBasket(pricedBasket);
            }
            catch (Exception ex)
            {
                _shoppingBasketOutputService.OutputError(transactionId, ex.Message);
                return;
            }
        }
    }
}
