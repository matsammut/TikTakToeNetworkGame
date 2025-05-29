using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicTacToeClient
{
    public class GameResult
    {
        [JsonPropertyName("board")]
        public char[] Board { get; set; }

        [JsonPropertyName("winner")]
        public string Winner { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            string serverUrl = "http://SSSD-ACU-DEV5:64920/tiktaktoe";


            Console.WriteLine("Tic Tac Toe Client");
            Console.WriteLine("------------------\n\n");

            PrintBoardTemplate();

            while (true)
            {
                Console.Write("Enter position (0-8): ");
                string posInput = Console.ReadLine();

                Console.Write("Enter value (X or O): ");
                string valInput = Console.ReadLine();

                if (!int.TryParse(posInput, out int position) || position < 0 || position > 8)
                {
                    Console.WriteLine("Invalid position.");
                    continue;
                }

                string value = valInput.ToUpper();

                if (value != "X" && value != "O")
                {
                    Console.WriteLine("Invalid value.");
                    continue;
                }

                var move = new
                {
                    Position = position,
                    Value = value
                };

                string json = System.Text.Json.JsonSerializer.Serialize(move);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    using var client = new HttpClient();
                    var response = await client.PostAsync(serverUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string resultJson = await response.Content.ReadAsStringAsync();

                        var gameResult = System.Text.Json.JsonSerializer.Deserialize<GameResult>(resultJson);

                        PrintBoard(gameResult.Board);

                        if (!string.IsNullOrEmpty(gameResult.Winner))
                        {
                            if (gameResult.Winner == "D")
                                Console.WriteLine("It's a draw!");
                            else
                                Console.WriteLine($"Player {gameResult.Winner} wins!");

                            break;
                        }
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to connect to server: " + ex.Message);
                }

                Console.WriteLine();
            }
        }

        static void PrintBoard(char[] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                Console.Write($" {board[i]} ");
                if ((i + 1) % 3 == 0)
                    Console.WriteLine();
                else
                    Console.Write("|");
            }
        }

        static void PrintBoardTemplate()
        {
            Console.WriteLine("Board Positions:");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($" {i} ");
                if ((i + 1) % 3 == 0)
                    Console.WriteLine();
                else
                    Console.Write("|");
            }
            Console.WriteLine();
        }

    }
}