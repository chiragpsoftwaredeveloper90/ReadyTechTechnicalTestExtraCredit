namespace CoffeeMachinMgmtSystem.Services
{
    public interface IWeatherService
    {
        public Task<double> GetTempratureAsync();
    }
}
