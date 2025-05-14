using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TikTakToeSERVER.Controllers
{
    [ApiController]
    [Route("tiktaktoe")]
    public class TikTakToe : ControllerBase
    {
        public static char[] Summaries = new[]
        {' ',' ',' ',' ',' ',' ',' ',' ',' '};

        private readonly ILogger<TikTakToe> _logger;

        public TikTakToe(ILogger<TikTakToe> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public char[] Post()
        {
            var rng = new Random();
            /*return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {

                Summaries
            })
            .ToArray();*/
            return Summaries;
        }
    }
}
