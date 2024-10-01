using CoffeeMachinMgmtSystem.Repository;
using Microsoft.Extensions.Configuration;

namespace CoffeeMachinMgmtSystem.Services
{
    /// <summary>
    /// Coffee Machine Service Class
    /// </summary>
    public class CoffeeMachineService : ICoffeeMachineService
    {
        private readonly ICoffeeMachineRepository _coffeeMachineRepository;
        private readonly IConfiguration _configuration;
        private readonly IWeatherService _weatherService; 



        /// <summary>
        /// Coffee Machine Service Constructor
        /// </summary>
        /// <param name="coffeeMachineRepository"></param>
        /// <param name="configuration"></param>
        public CoffeeMachineService(ICoffeeMachineRepository coffeeMachineRepository, IConfiguration configuration, IWeatherService weatherService)
        {
            _coffeeMachineRepository = coffeeMachineRepository;
            _configuration = configuration;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Brew Coffee
        /// </summary>
        /// <returns>string</returns>
        public async Task<string> BrewCoffeeAsync()
        {
            var temperature = Convert.ToDouble(_configuration["Temperature"]);
            _coffeeMachineRepository.IncrementBrewCoffeeCount();
            if (await _weatherService.GetTempratureAsync() > temperature)
            {
                return _configuration["icedCoffeeReady"];
            }
            return _configuration["coffeeReady"];
        }

        

        /// <summary>
        /// Is Out Of Coffee?
        /// </summary>
        /// <returns>bool</returns>
        public bool IsOutOfCoffee()
        {
            if (_coffeeMachineRepository.GetBrewCoffeeCount() % 5 == 0)
            {
                _coffeeMachineRepository.IncrementBrewCoffeeCount();
                return true;
            }

            return false;

        }

        /// <summary>
        /// Is Teapot Day?
        /// </summary>
        /// <returns>bool</returns>
        public bool IsTeapotDay(DateTime? date = null)
        {
            DateTime currentDate = date ?? DateTime.Now;
            return currentDate.Month == 4 && currentDate.Day == 1;
        }
    }
}
