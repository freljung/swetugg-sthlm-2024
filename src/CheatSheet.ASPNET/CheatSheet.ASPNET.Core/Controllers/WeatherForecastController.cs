using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheatSheet.ASPNET.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ValidateAntiForgeryToken]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("decentWeather")]
        [Authorize(Roles = "User")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("awesomeWeather")]
        [Authorize(Policy = "AwesomeUser")]
        public IEnumerable<WeatherForecast> GetAwesomeWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(20, 30),
                Summary = "Awesome weather",
            })
            .ToArray();
        }

        [HttpGet("regularWeather")]
        [Authorize]
        public IEnumerable<WeatherForecast> GetRegularWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "Who knows?",
            })
            .ToArray();
        }

        [HttpGet("crappyWeather")]
        [AllowAnonymous]
        public IEnumerable<WeatherForecast> GetAnonymousWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 0),
                Summary = "Who knows?",
            })
            .ToArray();
        }
    }
}