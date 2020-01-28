using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QueueTriggerComCosmosDbCustomizado
{
    public static class OnMessageProcessor
    {
        [FunctionName("OnMessageProcessor")]
        public static void Run(
            [QueueTrigger("cliente-event-queue", Connection = "AzureWebJobsStorage")]Cliente myQueueItem, 
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"Processando cliente {myQueueItem.Nome}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            //var cstr = config.GetConnectionString("CosmosDbConn");
            //var setting1 = config["IsEncrypted"];

            var cosmos = new CosmosDbContext(config.GetConnectionString("CosmosDbConn"));
            var data = cosmos.AddItemsToContainerAsync(myQueueItem).Result;
            log.LogInformation($"Processado cliente {data.Nome}");
        }
    }
}
