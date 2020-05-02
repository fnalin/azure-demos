using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;

namespace CadCliSearch
{
    class Program
    {
        static void Main(string[] args)
        {

            // Baseado em https://docs.microsoft.com/en-us/azure/search/search-get-started-dotnet

            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                .Build();

            var serviceClient = createSearchServiceClient(configuration);

            var searchServiceIndexName = configuration["SearchServiceIndexName"];
            createIndexIfNotExists(serviceClient, searchServiceIndexName);
            var indexClient = serviceClient.Indexes.GetClient(searchServiceIndexName);

            Console.Write("Informe o n. de clientes a ser gerado: ");
            var n = int.Parse(Console.ReadLine());
            if (n > 0)
            {
                Console.WriteLine($"Gerando {n} clientes");
                var clientes = createClientes(n);
                UploadDoc(clientes, indexClient);
            }
            Console.WriteLine("Fim");
        }




        private static SearchServiceClient createSearchServiceClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];

            return new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
        }

        private static void createIndexIfNotExists(SearchServiceClient serviceClient, string indexName)
        {

            if (!serviceClient.Indexes.Exists(indexName))
            {
                var definition = new Microsoft.Azure.Search.Models.Index()
                {
                    Name = indexName,
                    Fields = FieldBuilder.BuildForType<Cliente>()
                };

                Console.WriteLine("Índice criado com sucesso");
                serviceClient.Indexes.Create(definition);
            }
        }

        private static List<Cliente> createClientes(int qtde)
        {

            var names = new string[] { "Priscila", "Raphael", "Fabiano", "Isabel", "Nair", "Adriano", "Daniel", "João", "Maria", "José", "Joana", "Raimunda" };
            var countries = new string[] { "USA", "Brazil", "Mexico", "Germany", "Australia", "Italy", "Japan", "Russia", "Chile", "Argentina", "Colombia" };
            var clientes = new List<Cliente>();
            for (int i = 0; i < qtde; i++)
            {
                var name = names[(new Random().Next(0, names.Length - 1))] + " - " + i;
                var country = countries[(new Random().Next(0, countries.Length - 1))];
                clientes.Add(new Cliente { Name = name, Country = country });
                Thread.Sleep(100);
            }

            // clientes.ToList().ForEach(c=> Console.WriteLine($"Id: {c.Id} - Name: {c.Name} - Country: {c.Country} - DateCreated: {c.DateCreated}"));

            return clientes;
        }

        private static void UploadDoc(Cliente data, ISearchIndexClient indexClient)
        {
            //Criando o documento a ser indexado
            var actions = new IndexAction<Cliente>[] { IndexAction.Upload(data) };
            var batch = IndexBatch.New(actions);

            //Atualizado o indice com o novo documento
            indexClient.Documents.Index(batch);
        }

        private static void UploadDoc(List<Cliente> data, ISearchIndexClient indexClient)
        {
            //Criando o documento a ser indexado
            var actions = new List<IndexAction<Cliente>>();
            data.ForEach(d => actions.Add(IndexAction.Upload(d)));
            var batch = IndexBatch.New(actions);

            //Atualizando o indice com o novo documento
            indexClient.Documents.Index(batch);
        }
    }
}
