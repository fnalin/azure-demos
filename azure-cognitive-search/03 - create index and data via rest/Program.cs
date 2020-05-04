using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace AzureSearchRestAPI
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            // Baseado em https://docs.microsoft.com/en-us/azure/search/search-get-started-postman

            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                .Build();

            var key = configuration["SearchServiceKey"];
            httpClient.BaseAddress = new Uri(configuration["SearchServiceURL"]);
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("api-key", key);

            var indexName = "cadcli-index";

            var indexCreated = CreateIndex(indexName).Result;
            if (indexCreated)
                Console.WriteLine($"Índice criado com sucesso");

            Console.Write("Informe o n. de clientes a ser gerado: ");
            var n = int.Parse(Console.ReadLine());
            if (n > 0)
            {
                Console.WriteLine($"Gerando {n} clientes");
                var clientes = createClientes(n);
                var upload = UploadDoc(clientes, indexName).Result;

                if (upload) Console.WriteLine($"Clientes enviados");
            }

            Console.WriteLine("FIM");
        }

        private static string createClientes(int qtde)
        {

            var names = new string[] { "Priscila", "Raphael", "Fabiano", "Isabel", "Nair", "Adriano", "Daniel", "João", "Maria", "José", "Joana", "Raimunda" };
            var countries = new string[] { "USA", "Brazil", "Mexico", "Germany", "Australia", "Italy", "Japan", "Russia", "Chile", "Argentina", "Colombia" };
            var clientes = new List<Dictionary<string, object>>();
            for (int i = 0; i < qtde; i++)
            {
                var name = names[(new Random().Next(0, names.Length - 1))] + " - " + i;
                var country = countries[(new Random().Next(0, countries.Length - 1))];

                var data = new Dictionary<string, object>{
                    {"@search.action",  "upload"},
                    {"id",  Guid.NewGuid().ToString("N")},
                    {"name", name},
                    {"country", country},
                    {"dateCreated", DateTime.UtcNow},
                };

                clientes.Add(data);


                Thread.Sleep(100);
            }

            // clientes.ToList().ForEach(c=> Console.WriteLine($"Id: {c.Id} - Name: {c.Name} - Country: {c.Country} - DateCreated: {c.DateCreated}"));


            var @value = new Dictionary<string, object>{
                {"value", clientes}
            };

            return JsonSerializer.Serialize(@value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // ,WriteIndented = true    
            }
                                    );
        }

        public static async Task<bool> UploadDoc(string clientes, string indexName)
        {
            var content = new StringContent(clientes, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{indexName}/docs/index?api-version=2019-05-06", content);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<bool> CreateIndex(string name)
        {

            // verificar se o índice existe:
            var indices = await httpClient.GetAsync($"{name}?api-version=2019-05-06");
            string responseBodyAsText = await indices.Content.ReadAsStringAsync();

            if (indices.StatusCode != HttpStatusCode.NotFound) return false;



            var data = new Dictionary<string, object>
            {
                { "name", name},
                { "fields",
                    new Dictionary<string, object> []{
                        new Dictionary<string, object>{
                            { "name", "id"},
                            {"type","Edm.String"},
                            {"facetable",false},
                            {"filterable", true},
                            {"key", true},
                            {"retrievable", true},
                            {"searchable", false},
                            {"sortable", false},
                            {"analyzer", null},
                            {"indexAnalyzer", null},
                            {"searchAnalyzer", null}
                        },
                         new Dictionary<string, object>{
                            {"name", "name"},
                            {"type","Edm.String"},
                            {"facetable",false},
                            {"filterable", true},
                            {"key", false},
                            {"retrievable", true},
                            {"searchable", false},
                            {"sortable", true},
                            {"analyzer", null},
                            {"indexAnalyzer", null},
                            {"searchAnalyzer", null}
                        },

                        new Dictionary<string, object>{
                            {"name", "dateCreated"},
                            {"type","Edm.DateTimeOffset"},
                            {"facetable",false},
                            {"filterable", true},
                            {"retrievable", true},
                            {"searchable", false},
                            {"sortable", true},
                            {"analyzer", null},
                            {"indexAnalyzer", null},
                            {"searchAnalyzer", null}
                        }
                    }
                }
            };

            var jsonSerializer = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // ,WriteIndented = true    
            }
                                    );
            var content = new StringContent(jsonSerializer, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"{name}?api-version=2019-05-06", content);

            return response.StatusCode == HttpStatusCode.Created;
        }
    }
}
