using CoffeeMachinMgmtSystem.Repository;
using CoffeeMachinMgmtSystem.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)  // Sets the base path where the file is located
            .AddJsonFile("Resources/Message.en.json", optional: false, reloadOnChange: true) // Loads the JSON file
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
builder.Services.AddSingleton<IConfiguration>(config);
builder.Services.AddSingleton<ICoffeeMachineService, CoffeeMachineService>();
builder.Services.AddSingleton<ICoffeeMachineRepository, CoffeeMachineRepository>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
