using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QueueTriggerComCosmosDbCustomizado
{
    public static class OnMessageReceiver
    {
        [FunctionName("OnMessageReceiver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [Queue("cliente-event-queue")]IAsyncCollector<Cliente> clienteQueue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Cliente data = JsonConvert.DeserializeObject<Cliente>(requestBody);
            data.Id = System.Guid.NewGuid();
            if (data!=null){
                await clienteQueue.AddAsync(data);
                return (ActionResult)new OkObjectResult($"Cliente {data.Nome} adicionado");
            }

            return new BadRequestObjectResult("Dados inválidos");
        }
    }
}
