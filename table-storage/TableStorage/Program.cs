using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace TableStorage
{
    class Program
    {
        private static CloudTable _table;

        static async Task Main(string[] args)
        {
            var account = CloudStorageAccount.Parse("STRING DE CONEXÃO DO STORAGE");
            var client = account.CreateCloudTableClient();

            _table = client.GetTableReference("people");

            await _table.CreateIfNotExistsAsync();

            var customer1 = new CustomerEntity("Haynes", "Jodie");
            customer1.Email = "jodie@contoso.com";
            customer1.PhoneNumber = "425-555-0101";

            AddCustomer(customer1);

            GetData();

            Console.WriteLine("FIM");
            Console.ReadLine();



        }

        private static void GetData()
        {
            var condition = TableQuery.GenerateFilterCondition(
                            "PartitionKey", QueryComparisons.Equal, "Haynes");
            var query = new TableQuery<CustomerEntity>().Where(condition);
            foreach (CustomerEntity entity in _table.ExecuteQuery(query))
            {
                Console.WriteLine($"{entity.RowKey} {entity.PartitionKey} [Email: {entity.Email} | Phone: {entity.PhoneNumber}]");
            }

        }

        private static void AddCustomer(CustomerEntity customer1)
        {
            var insertOperation = TableOperation.Insert(customer1);
            _table.Execute(insertOperation);

        }
    }
}
