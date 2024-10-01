using CoffeeMachinMgmtSystem.Models;
using CoffeeMachinMgmtSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachinMgmtSystem.Controllers
{
    /// <summary>
    /// Coffee Machine Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly ICoffeeMachineService _coffeeMachineService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoffeeMachineController> _logger;

        /// <summary>
        /// Coffee Machine Controller
        /// </summary>
        /// <param name="coffeeMachineService"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public CoffeeMachineController(ICoffeeMachineService coffeeMachineService, IConfiguration configuration, ILogger<CoffeeMachineController> logger)
        {
            _coffeeMachineService = coffeeMachineService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Brew Coffee
        /// </summary>
        /// <returns>Common Response</returns>
        [HttpGet("brew-coffee")]
        [ProducesResponseType(typeof(CommonResponse), 200)]
        public async Task<IActionResult> BrewCoffee()
        {
            try
            {
                if (_coffeeMachineService.IsTeapotDay())
                {
                    return StatusCode(418, _configuration["teapotMessage"]);
                }
                else if (_coffeeMachineService.IsOutOfCoffee())
                {
                    return StatusCode(503, _configuration["outOfCoffee"]);
                }

                var message = await _coffeeMachineService.BrewCoffeeAsync();
                var response = new CommonResponse
                {
                    Message = message,
                    Prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CoffeeMachineController BrewCoffee");
            }

            return BadRequest(_configuration["CoffeeMachineError"]);
        }
    }
}
