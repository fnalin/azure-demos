using Microsoft.AspNetCore.Mvc;

namespace fansoft.cosmosdb.cadcli.api.Controllers
{
    public class TesteController
    {
        
        [HttpGet("api/ping")]
        public string Pong() => "Ping";
    }
}