using System.Collections.Generic;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.Get;

namespace TechnicalTest.Api.Services
{
    public interface ICosmosDbService
    {
        Task SetCurrentLocationForUserAsync(UserCurrentLocation userCurrentLocation);
        Task<UserCurrentLocation> GetCurrentLocationForUserAsync(string userIdentifier);
        Task<List<UserCurrentLocation>> GetLocationHistoryForUserAsync(string userIdentifier);
        Task<List<UserCurrentLocation>> GetCurrentLocationForAllUsers();
        Task UpdateLocationHistoryForUserAsync(UserCurrentLocation userCurrentLocation);
        Task UpdateCurrentLocationForAllUsers(UserCurrentLocation userCurrentLocation);

    }
}