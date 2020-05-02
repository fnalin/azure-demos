using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;

namespace CadCliSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            // Baeado em https://docs.microsoft.com/en-us/azure/search/tutorial-csharp-create-first-app

            var configuration =
               new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
               .Build();

            var serviceClient = createSearchServiceClient(configuration);
            var searchServiceIndexName = configuration["SearchServiceIndexName"];
            var indexClient = serviceClient.Indexes.GetClient(searchServiceIndexName);

            RunQueries(indexClient);



            Console.WriteLine("FIM");
        }

        private static SearchServiceClient createSearchServiceClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];

            return new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
        }

        private static void RunQueries(ISearchIndexClient indexClient)
        {
            SearchParameters parameters;
            DocumentSearchResult<Cliente> results;

            // Query 1 
            Console.WriteLine("\nQuery 1: Search * - & dateCreated desc\n");
            parameters = new SearchParameters() { OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true };
            results = indexClient.Documents.Search<Cliente>(null, parameters);
            WriteDocuments(results);

            // Query 2
            Console.WriteLine("\nQuery 1: Search $filter=country eq 'Brazil' - & dateCreated desc\n");
            parameters = new SearchParameters() { Filter = "country eq 'Brazil'", OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true };
            results = indexClient.Documents.Search<Cliente>(null, parameters);
            WriteDocuments(results);

            // Query 3
            Console.WriteLine("\nQuery 1: Search $filter=dateCreated ge 2010-01-01T00:00:00-00:00 - & dateCreated desc\n");
            //últimos 5 min
            var dataAtual = DateTime.UtcNow.AddMinutes(-5).ToString("o"); //o = formato utc
            parameters = new SearchParameters() { Filter = $"dateCreated ge {dataAtual}", OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true };
            //parameters = new SearchParameters(){Filter = $"dateCreated ge 2020-05-01T02:00:00-00:00",OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true};
            results = indexClient.Documents.Search<Cliente>(null, parameters);
            WriteDocuments(results);

            //Paging 200 - 200
            Console.WriteLine("\nPaginando de 100 em 100\n");
            parameters = new SearchParameters() { OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true };
            results = indexClient.Documents.Search<Cliente>(null, parameters);
            var total = results.Count;
            var pages = total / 200;
            for (int itemActual = 0; itemActual < total; itemActual += 200)
            {
                Console.WriteLine("\nItem: " + (itemActual + 1));
                parameters = new SearchParameters() { OrderBy = new[] { "dateCreated desc" }, IncludeTotalResultCount = true, Skip = itemActual, Top = 200 };
                results = indexClient.Documents.Search<Cliente>(null, parameters);
                WriteDocuments(results, itemActual + 1);

                Console.Write("Continuar para a próxima página? (S|N) ");
                if (Console.ReadLine().ToLower() == "n") return;
            }

            // para mais exemplos de filters: https://docs.microsoft.com/en-us/azure/search/search-query-odata-filter

        }


        private static void WriteDocuments(DocumentSearchResult<Cliente> results, int start = 1)
        {
            Console.WriteLine($"Total: {results.Count}");
            Console.WriteLine($"{"Item",-10} {"Id",-32} {"Name",-15} {"Country",-15} DateCreated");
            results.Results.ToList().ForEach(s =>
                    {
                        Console.WriteLine($"{start.ToString("0000000000")} {s.Document.Id} {s.Document.Name,-15} {s.Document.Country,-15} {s.Document.DateCreated}");
                        start++;
                    }
                );

        }
    }
}
