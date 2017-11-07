using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SandTigerShark.Functions.TicTacToe
{
    public static class TicTacToe
    {
        [FunctionName("TicTacToe")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"TicTacToe has been triggered");

            string jsonContent = await req.Content.ReadAsStringAsync();
            var command = JsonConvert.DeserializeObject<Play>(jsonContent);

            if(command == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = "No command supplied."
                });
            }

            try
            {
                var game = new Game(command.Board);
                game.Play(command.Player, command.Position);

                return req.CreateResponse(HttpStatusCode.OK, new
                {
                    game.Board,
                    GameOver = game.IsGameOver,
                    Winner = game.Winner
                });
            }
            catch(Exception e)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = e.Message
                });
            }
        }
    }
}
