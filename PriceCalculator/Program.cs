using PriceCalulator.Repositories;
using PriceCalulator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PriceCalculator.Services;

namespace PriceCalculator
{
    class Program
    {
        /// <summary>
        /// Entry point to the console application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<PriceCalculatorApplication>().Calculate(args);
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
            });

            services.AddTransient<IShoppingBasketOutputService, ConsoleShoppingBasketOutputService>();
            services.AddTransient<IProductRepository, SimpleProductRepository>();
            services.AddTransient<IBasketProvider, BasketProvider>();
            services.AddTransient<ISpecialOfferCalculator, SpecialOfferCalculator>();
            services.AddTransient<ISpecialOfferRepository, SimpleSpecialOfferRepository>();
            services.AddTransient<PriceCalculatorApplication>();
            return services;
        }
    }
}
