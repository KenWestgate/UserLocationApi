using System.Threading.Tasks;
using TechnicalTest.Api.Documents;
using TechnicalTest.Api.Documents.Get;
using TechnicalTest.Api.Documents.Set;

namespace TechnicalTest.Api.Services
{
    public interface IUserLocationService
    {
        Task<OperationResult<UserCurrentLocation>> SetCurrentLocationForUserAsync(UserCurrentLocationUpdate model);
        Task<OperationResult<UserCurrentLocation>> GetCurrentLocationForUserAsync(string userIdentifier);
    }
}
