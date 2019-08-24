using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace HttpTrigger
{
    public static class OnProcessarPedido
    {
        [FunctionName("OnProcessarPedido")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Pedido>(requestBody);
            data.Id = Guid.NewGuid();
            log.LogInformation($"Pedido {data.Id} recebido");
            return
                (ActionResult)new OkObjectResult($"Dados recebidos");

        }
    }
}
