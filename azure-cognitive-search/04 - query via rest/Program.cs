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
            //var data = RunQuery(indexName, "*&%24count=true&%24orderby=dateCreated%20desc").Result;
            var data = RunQuery(indexName, "*&%24top=10&%24orderby=dateCreated%20desc").Result;

            Console.WriteLine(data);

            Console.WriteLine("FIM");

        }

        public static async Task<string> RunQuery(string indexName, string query = "*")
        {
            var indices = await httpClient.GetAsync($"{indexName}/docs?api-version=2019-05-06&search={query}");
            string responseBodyAsText = await indices.Content.ReadAsStringAsync();

            if (indices.StatusCode != HttpStatusCode.OK) return null;

            return responseBodyAsText;
        }

    }
}
