using System.Collections.Generic;
using System.Threading.Tasks;
using UserLocation.Api.Documents;
using UserLocation.Api.Documents.Get;
using UserLocation.Api.Documents.Set;

namespace UserLocation.Api.Services
{
    public interface IUserLocationService
    {
        Task<OperationResult<UserCurrentLocation>> SetCurrentLocationForUserAsync(UserCurrentLocationUpdate model);
        Task<OperationResult<UserCurrentLocation>> GetCurrentLocationForUserAsync(string userIdentifier);
        Task<OperationResult<List<UserCurrentLocation>>> GetLocationHistoryForUserAsync(string userIdentifier);
        Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForAllUsersAsync();
        Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForUsersInAreaAsync(AreaBoundary areaBoundary);
        Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForUsersNearLocationAsync(double latitude, double longitude, int radiusInNm);
    }
}
