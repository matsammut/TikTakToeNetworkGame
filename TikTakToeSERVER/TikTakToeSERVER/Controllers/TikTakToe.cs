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

        private static char? currentTurn = null;

        public TikTakToe(ILogger<TikTakToe> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewPosition bodyInner)
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();


            char value = bodyInner.Value.ToCharArray()[0];

            if (bodyInner.Position < 0 || bodyInner.Position > Summaries.Length)
            {
                return BadRequest("Position out of bounds");
            }

            if (value != 'O' && value != 'X')
            {
                string error = "Invalid input value :" + value;
                return BadRequest(error);
            }

            if (Summaries[bodyInner.Position] != ' ')
            {
                return BadRequest("Occupied position Error");
            }

            if (currentTurn == null)
            {
                currentTurn = value;
            }

            if (value != currentTurn)
            {
                return BadRequest($"It is not {value}'s turn. It is {currentTurn}'s turn.");
            }

            currentTurn = currentTurn == 'X' ? 'O' : 'X';

            Summaries[bodyInner.Position] = bodyInner.Value.ToCharArray()[0];

            var winner = CheckWin(Summaries);

            var response = new
            {
                Board = Summaries,
                Winner = winner
            };

            if (winner != null)
            {
                currentTurn = null;

                char[] BoardCopy = Summaries;

                response = new
                {
                    Board = BoardCopy,
                    Winner = winner
                };

                ResetSummaries();
            }
            return Ok(response);

        }

        public void ResetSummaries()
        {
            for (int i = 0; i < Summaries.Length; i++)
                Summaries[i] = ' ';
        }

        private char? CheckWin(char[] board)
        {
            int[][] lines = new[]
            {
                new[] {0,1,2},
                new[] {3,4,5},
                new[] {6,7,8},
                new[] {0,3,6},
                new[] {1,4,7},
                new[] {2,5,8},
                new[] {0,4,8},
                new[] {2,4,6 }
            };
            //get all the line winning possibilities.

            foreach (var line in lines)
            {
                char pos1 = board[line[0]];
                char pos2 = board[line[1]];
                char pos3 = board[line[2]];

                //get the chars in the board on the "winning" line.

                if (pos1 != ' ' && pos1 == pos2 && pos2 == pos3)
                    return pos1;//return the winning char.
            }

            //if the board has no empty spaces then no winners.
            if (board.All(cell => cell != ' '))
                return 'D'; //D returned for draw.

            return null;
        }
    }
}
