using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Cosmos;

namespace fansoft.cosmosdb.cadcli.core
{
    public class Repository : IRepository
    {

        private const string EndpointUrl = "https://cosmos-demos-fan.documents.azure.com:443/";
        private const string AuthorizationKey = "42dco8NICZrGhjCdqsKoFxXpaQjcVdjWFz97m3JQwZWKxjlNKVsTbJNoV2B4UKQbqROA4i77tM8GClulNXyF5g==";
        private const string DatabaseId = "cadcli";
        private const string ContainerId = "demo3";
        private readonly CosmosClient _cosmosClient;

        public Repository()
        {
            _cosmosClient = new CosmosClient(EndpointUrl, AuthorizationKey);
        }



        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
        }

        private async Task CreateContainerAsync()
        {
            // Create a new container
            var container = await _cosmosClient.GetDatabase(DatabaseId).CreateContainerIfNotExistsAsync(ContainerId, "/local/pais",400);
        }

        public async Task<Cliente> AddItemsToContainerAsync(Cliente cli)
        {
            await CreateDatabaseAsync();
            await CreateContainerAsync();

            // dynamic cliTeste = new
            // {
            //     id = "MyId3",
            //     nome = "Raphael Nalin",
            //     endereco = "Rua ABC, 187",
            //     bairro = "XPTO",
            //     cidade = "São Paulo",
            //     estado = "SP",
            //     pais = "EUA"
            // };

            /*
                dynamic testItem = new { id = "MyTestItemId", partitionKeyPath = "MyTestPkValue", details = "it's working" };
                ItemResponse<dynamic> response = await container.CreateItemAsync(testItem);
            */
            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
            // ItemResponse<Cliente> data = await container.UpsertItemAsync<Cliente>(cliTeste, new PartitionKey("Brasil"));
            cli.Id = System.Guid.NewGuid().ToString("N");
            var response = await container.CreateItemAsync(cli);
            return cli;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
            var sql = "SELECT * FROM c";
            var iterator = container.GetItemQueryIterator<Cliente>(sql);
            List<Cliente> items = new List<Cliente>();
            await foreach (var item in iterator)
            {
                items.Add(item);
            }
            return items;
        }


    }
}