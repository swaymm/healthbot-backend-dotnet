using Microsoft.AspNetCore.Mvc;
using AIhealthchatbot; // ✅ Import the model's namespace

namespace AIhealthchatbot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Chilly", "Cool", "Mild", "Warm", "Hot", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 40),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }
    }
}
