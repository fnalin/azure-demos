using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QueueTrigger
{
    public static class OnReceberPedido
    {
        [FunctionName("OnReceberPedido")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("pedidos")] IAsyncCollector<Pedido> pedidoQueue,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);
            var data = JsonConvert.DeserializeObject<Pedido>(requestBody);
            data.Id = Guid.NewGuid();
            data.Aprovado = false;
            
            await pedidoQueue.AddAsync(data);

            return (ActionResult)new OkObjectResult(data);
                
        }
    }
}
