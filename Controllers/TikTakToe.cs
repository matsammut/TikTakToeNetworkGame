using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TikTakToeSERVER.Controllers
{
    public class NewPosition
    {
        [JsonProperty("Position")]
        public int Position { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
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

        [HttpPost]
        public IActionResult Post([FromBody] NewPosition bodyInner)
        {
            char value = bodyInner.Value.ToCharArray()[0];

            if (bodyInner.Position < 0 || bodyInner.Position > Summaries.Length){
                return BadRequest("Position out of bounds");
            }

            if (value != 'O' || value != 'X') { 
                string error = "Invalid input value :" + value;
                return BadRequest(error);
            }
            
            if (Summaries[bodyInner.Position] != ' '){
                return BadRequest("Occupied position Error");
            }

            Summaries[bodyInner.Position] = bodyInner.Value.ToCharArray()[0];

            
            return Ok(Summaries);
        }
    }
}
