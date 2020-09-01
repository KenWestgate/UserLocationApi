using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechnicalTest.Api.Services;

namespace TechnicalTest.Api.Extensions
{
    public static class CosmosDBServiceBuilder
    {
        public static IServiceCollection AddCosmosDbService(this IServiceCollection services, Action<CosmosDatabaseConfiguration> configuration)
        {
            var c = new CosmosDatabaseConfiguration();
            configuration?.Invoke(c);

            var client = new CosmosClient(c.AccountEndPoint, c.Key);
            var cosmosDbService = new CosmosDbService(client, c.DatabaseName, c.ContainerName);
            var database = client.CreateDatabaseIfNotExistsAsync(c.DatabaseName).GetAwaiter().GetResult();
            database.Database.CreateContainerIfNotExistsAsync(c.ContainerName, "/partitionKey").GetAwaiter().GetResult();

            services.AddSingleton(typeof(ICosmosDbService), cosmosDbService);
            return services;
        }
    }

    public class CosmosDatabaseConfiguration
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string AccountEndPoint { get; set; }
        public string Key { get; set; }
    }
}
