using System.Threading.Tasks;
using Azure.Cosmos;

namespace QueueTriggerComCosmosDbCustomizado
{
    public class CosmosDbContext
    {
        private readonly string DatabaseId;
        private readonly string ContainerId;
        private readonly CosmosClient _cosmosClient;

        public CosmosDbContext(string cosmosDbConn)
        {
            _cosmosClient = new CosmosClient(cosmosDbConn);
            var data = System.DateTime.UtcNow;
            DatabaseId = $"cadcli-{data.Year}-{data.Month}";
            ContainerId = $"cadcli-{data.Year}-{data.Month}-{data.Day}";
        }

        public async Task<dynamic> AddItemsToContainerAsync(dynamic data)
        {
            await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            await _cosmosClient.GetDatabase(DatabaseId)
                .CreateContainerIfNotExistsAsync(ContainerId, "/idade", 400);

            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
            
            await container.CreateItemAsync(data);
            return data;
        }

    }
}