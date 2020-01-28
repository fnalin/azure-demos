using System;
using System.Text.Json.Serialization;

namespace QueueTriggerComCosmosDbCustomizado
{
    public class Cliente
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        [JsonPropertyName("idade")]
        public int Idade { get; set; }

        public System.DateTime DataCadastro { get;} = DateTime.UtcNow;
    }
}