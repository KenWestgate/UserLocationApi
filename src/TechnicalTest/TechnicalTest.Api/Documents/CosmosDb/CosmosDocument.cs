using Newtonsoft.Json;

namespace TechnicalTest.Api.Documents.CosmosDb
{
    public class CosmosDocument<T>
    {
        public T Document { get; set; }

        [JsonProperty(PropertyName="partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
