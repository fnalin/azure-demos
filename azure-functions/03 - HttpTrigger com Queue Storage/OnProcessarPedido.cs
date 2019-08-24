using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HttpTriggerQueue
{
    public static class OnProcessarPedido
    {
        [FunctionName("OnProcessarPedido")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("pedidos")] IAsyncCollector<Pedido> pedidoQueue,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Pedido>(requestBody);
            data.Id = Guid.NewGuid();

            await pedidoQueue.AddAsync(data);

            var value = $"Pedido {data.Id} recebido e será processado em breve";
            return
                (ActionResult)new OkObjectResult(value);

        }
    }
}
