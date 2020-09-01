using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.CosmosDb;
using TechnicalTest.Api.Documents.Get;

namespace TechnicalTest.Api.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        private const string USER_CURRENT_LOCATION_PARTITON_KEY = "USER-CURRENT-LOCATION";

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task SetCurrentLocationForUserAsync(UserCurrentLocation userCurrentLocation)
        {
            var cosmosDocument = new CosmosDocument<UserCurrentLocation>()
            {
                PartitionKey = USER_CURRENT_LOCATION_PARTITON_KEY,
                Document = userCurrentLocation,
                Id = userCurrentLocation.Id
            };
            await this._container.UpsertItemAsync<CosmosDocument<UserCurrentLocation>>(cosmosDocument, new PartitionKey(USER_CURRENT_LOCATION_PARTITON_KEY));
        }

        public async Task<List<UserCurrentLocation>> GetCurrentLocationForAllUsers()
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserCurrentLocation> GetCurrentLocationForUserAsync(string userIdentifier)
        {
            var response = await _container.ReadItemAsync<CosmosDocument<UserCurrentLocation>>(
                partitionKey: new PartitionKey(USER_CURRENT_LOCATION_PARTITON_KEY),
                id: userIdentifier);

            // TODO: log info and costs for performance tuning
            //_logger.LogInformation($"Diagnostics for ReadItemAsync: {response.Diagnostics.ToString()}");
            //_logger.LogInformation("Item read by Id {0}", response.Resource);
            //_logger.LogInformation("Request Units Charge for reading Item by Id {0}", response.RequestCharge);

            var cosmosDocument = (CosmosDocument<UserCurrentLocation>)response;
            return cosmosDocument.Document;
        }

        public async Task<List<UserCurrentLocation>> GetLocationHistoryForUserAsync(string userIdentifier)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateLocationHistoryForUserAsync(UserCurrentLocation userCurrentLocation)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateCurrentLocationForAllUsers(UserCurrentLocation userCurrentLocation)
        {
            throw new System.NotImplementedException();
        }
    }
}