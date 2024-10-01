using CoffeeMachinMgmtSystem.Controllers;
using CoffeeMachinMgmtSystem.Models;
using CoffeeMachinMgmtSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoffeeMachinMgmtSystem.Test
{
    /// <summary>
    /// Coffee Machine Controller Test
    /// </summary>
    public class CoffeeMachineControllerTest
    {
        private readonly Mock<ICoffeeMachineService> _mockService; 
        private readonly Mock<ILogger<CoffeeMachineController>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly CoffeeMachineController _controller;

        /// <summary>
        /// Coffee Machine Controller Test
        /// </summary>
        public CoffeeMachineControllerTest()
        {
            _mockLogger = new Mock<ILogger<CoffeeMachineController>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockService = new Mock<ICoffeeMachineService>();
            _controller = new CoffeeMachineController(_mockService.Object, _mockConfiguration.Object, _mockLogger.Object);
        }

        /// <summary>
        /// BrewCoffee Returns Ok When Coffee Is Ready
        /// </summary>
        [Fact]
        public async Task BrewCoffee_ReturnsOk_WhenCoffeeIsReady()
        {
            var expectedMessage = "Your piping hot coffee is ready";
            _mockService.Setup(s => s.IsTeapotDay(null)).Returns(false);
            _mockService.Setup(s => s.IsOutOfCoffee()).Returns(false);
            _mockService.Setup(s => s.BrewCoffeeAsync()).Returns(Task.FromResult(expectedMessage));
            _mockConfiguration.Setup(c => c["coffeeReady"]).Returns(expectedMessage);
            var result =await _controller.BrewCoffee();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommonResponse>(okResult.Value);
            Assert.Equal(expectedMessage, response.Message);
        }

        /// <summary>
        /// Brew Coffee Returns Teapot Status When It Is TeapotDay
        /// </summary>
        [Fact]
        public async Task BrewCoffee_ReturnsTeapotStatus_WhenItIsTeapotDay()
        {
            _mockService.Setup(s => s.IsTeapotDay(null)).Returns(true);
            _mockConfiguration.Setup(c => c["teapotMessage"]).Returns("I'm a teapot");
            var result = await _controller.BrewCoffee();
            var teapotResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(418, teapotResult.StatusCode);
        }

        /// <summary>
        /// Brew Coffee Returns Service Unavailable When Out Of Coffee
        /// </summary>
        [Fact]
        public async Task BrewCoffee_ReturnsServiceUnavailable_WhenOutOfCoffee()
        {
            // Arrange
            _mockService.Setup(s => s.IsTeapotDay(null)).Returns(false);
            _mockService.Setup(s => s.IsOutOfCoffee()).Returns(true);
            _mockConfiguration.Setup(c => c["outOfCoffee"]).Returns("Out of coffee!");
            var result = await _controller.BrewCoffee();
            var serviceUnavailableResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, serviceUnavailableResult.StatusCode);
        }

        /// <summary>
        /// Brew Coffee Returns BadRequest OnException
        /// </summary>
        [Fact]
        public async Task BrewCoffee_ReturnsBadRequest_OnException()
        {
            _mockService.Setup(s => s.IsTeapotDay(null)).Throws(new Exception("Test Exception"));
            _mockConfiguration.Setup(c => c["CoffeeMachineError"]).Returns("An error occurred");
            var result = await _controller.BrewCoffee();
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred", badRequestResult.Value);
        }

       
    }
}
