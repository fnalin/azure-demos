using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QueueTrigger
{
    public static class OnProcessarPedido
    {
        [FunctionName("OnProcessarPedido")]
        public static void Run(
            [QueueTrigger("pedidos", Connection = "")]Pedido pedidoItem, 
            ILogger log)
        {
            pedidoItem.Aprovado = true;
            var serialize = JsonConvert.SerializeObject(pedidoItem);
            log.LogInformation($"Trigger queue disparada: {pedidoItem.Id}, dados: {serialize}");
        }
    }
}
