using CoffeeMachinMgmtSystem.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CoffeeMachinMgmtSystem.Services
{

    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<double> GetTempratureAsync()
        {
            try
            {
                string apiUrl = GetWeatherURL();
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var responsemodel = JsonSerializer.Deserialize<Weather>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (responsemodel != null && responsemodel.Main != null)
                    {
                        return ConvertKelvinToCelsius(responsemodel.Main.Temp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        private string GetWeatherURL()
        {
            string apiUrl = _configuration["WeatherBaseApiUrl"];
            string WeatherBaseApiUrlParameter = _configuration["WeatherBaseApiUrlParameter"];
            string WeatherBaseApiKey = _configuration["WeatherBaseApiKey"];
            string Latitude = _configuration["Latitude"];
            string Longitude = _configuration["Longitude"];
            string url = apiUrl + string.Format(WeatherBaseApiUrlParameter, Latitude, Longitude, WeatherBaseApiKey);
            return url;
        }

        public static double ConvertKelvinToCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }
    }
}
