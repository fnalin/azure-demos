using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace fansoft.cosmosdb.cadcli.core
{
    public class Cliente
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("local")]
        public Local Local { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

    public class Local
    {
        [JsonPropertyName("endereco")]
        public string Endereco { get; set; }
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }
        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
        [JsonPropertyName("pais")]
        public string Pais { get; set; }
    }

}
