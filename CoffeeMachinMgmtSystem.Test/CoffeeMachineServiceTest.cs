using CoffeeMachinMgmtSystem.Repository;
using CoffeeMachinMgmtSystem.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CoffeeMachinMgmtSystem.Test
{
    /// <summary>
    /// Coffee Machine Service Test
    /// </summary>
    public class CoffeeMachineServiceTest
    {
        private readonly Mock<ICoffeeMachineRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly CoffeeMachineService _service;
        private readonly Mock<IWeatherService> _weatherServiceMock;

        /// <summary>
        /// Coffee Machine Service Test
        /// </summary>
        public CoffeeMachineServiceTest()
        {
            _mockRepository = new Mock<ICoffeeMachineRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _service = new CoffeeMachineService(_mockRepository.Object, _mockConfiguration.Object, _weatherServiceMock.Object);
        }

        /// <summary>
        /// BrewCoffee Increments Brew Count With Weather Check
        /// </summary>
        /// <returns>void</returns>
        public async Task BrewCoffee_IncrementsBrewCount_WithWeatherCheck()
        {
            _mockRepository.Setup(r => r.GetBrewCoffeeCount()).Returns(1);
            _weatherServiceMock.Setup(w => w.GetTempratureAsync()).ReturnsAsync(25); // Mock temperature to 25°C (below the iced coffee threshold)
            _mockConfiguration.Setup(c => c["coffeeReady"]).Returns("Your piping hot coffee is ready");
            var result = await _service.BrewCoffeeAsync();
            _mockRepository.Verify(r => r.IncrementBrewCoffeeCount(), Times.Once);
            Assert.Equal("Your piping hot coffee is ready", result);
        }

        /// <summary>
        /// BrewCoffee Increments Brew Count Returns IcedCoffee When Temperature Is High
        /// </summary>
        /// <returns>Void</returns>
        public async Task BrewCoffee_IncrementsBrewCount_ReturnsIcedCoffee_WhenTemperatureIsHigh()
        {
            _mockRepository.Setup(r => r.GetBrewCoffeeCount()).Returns(1);
            _weatherServiceMock.Setup(w => w.GetTempratureAsync()).ReturnsAsync(35); // Mock temperature to 35°C (above the iced coffee threshold)
            _mockConfiguration.Setup(c => c["icedCoffeeReady"]).Returns("Your iced coffee is ready");
            var result = await _service.BrewCoffeeAsync();
            _mockRepository.Verify(r => r.IncrementBrewCoffeeCount(), Times.Once);
            Assert.Equal("Your iced coffee is ready", result);
        }

        /// <summary>
        /// Is OutOf Coffee Returns True On Fifth Brew
        /// </summary>
        [Fact]
        public void IsOutOfCoffee_ReturnsTrue_OnFifthBrew()
        {
            _mockRepository.Setup(r => r.GetBrewCoffeeCount()).Returns(5);
            var result = _service.IsOutOfCoffee();
            Assert.True(result);
            _mockRepository.Verify(r => r.IncrementBrewCoffeeCount(), Times.Once);  
        }


        /// <summary>
        /// Is OutOf Coffee Returns False On Non Fifth Brew
        /// </summary>
        [Fact]
        public void IsOutOfCoffee_ReturnsFalse_OnNonFifthBrew()
        {
            _mockRepository.Setup(r => r.GetBrewCoffeeCount()).Returns(3);
            var result = _service.IsOutOfCoffee();
            Assert.False(result);
            _mockRepository.Verify(r => r.IncrementBrewCoffeeCount(), Times.Never);  
        }

        /// <summary>
        /// Is Teapot Day Returns True On April First
        /// </summary>
        [Fact]
        public void IsTeapotDay_ReturnsTrue_OnAprilFirst()
        {
            var testDate = new DateTime(2024, 4, 1);
            var result = _service.IsTeapotDay(testDate);
            Assert.True(result);
        }

        /// <summary>
        /// Is TeapotDay Returns False On Other Days
        /// </summary>
        [Fact]
        public void IsTeapotDay_ReturnsFalse_OnOtherDays()
        {
            var testDate = new DateTime(2024, 4, 2);
            var result = _service.IsTeapotDay(testDate);
            Assert.False(result);
        }
    }
}
