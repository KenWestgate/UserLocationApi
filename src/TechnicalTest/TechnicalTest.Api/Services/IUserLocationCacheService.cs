using System.Collections.Generic;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.Get;

namespace TechnicalTest.Api.Services
{
    public interface IUserLocationCacheService
    {
        Task SetCurrentLocationForUserAsync(UserCurrentLocation userCurrentLocation);
        Task<UserCurrentLocation> GetCurrentLocationForUserAsync(string userIdentifier);
        Task<List<UserCurrentLocation>> GetLocationHistoryForUserAsync(string userIdentifier);
        Task<List<UserCurrentLocation>> GetCurrentLocationForAllUsersAsync();

        Task SetLocationHistoryForUser(UserCurrentLocation userCurrentLocation);
        Task SetLocationHistoryForUser(List<UserCurrentLocation> userLocationHistory);
        Task SetCurrentLocationForAllUsers(UserCurrentLocation userCurrentLocation);
        Task SetCurrentLocationForAllUsers(List<UserCurrentLocation> allUsersCurrentLocation);
    }
}
