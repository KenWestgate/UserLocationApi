using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents;
using TechnicalTest.Api.Documents.Get;
using TechnicalTest.Api.Documents.Set;

namespace TechnicalTest.Api.Services
{
    public class UserLocationService : IUserLocationService
    {
        private readonly ILogger<UserLocationService> _logger;
        private readonly IUserLocationCacheService _userLocationCacheService;

        public UserLocationService(ILogger<UserLocationService> logger, IUserLocationCacheService userLocationCacheService)
        {
            _logger = logger;
            _userLocationCacheService = userLocationCacheService;
        }

        public async Task<OperationResult<UserCurrentLocation>> SetCurrentLocationForUserAsync(UserCurrentLocationUpdate model)
        {
            try
            {
                var userCurrentLocation = new UserCurrentLocation()
                {
                    Id = model.Id,
                    CurrentLocation = model.CurrentLocation,
                    TimeAtLocation = DateTime.UtcNow
                };
                await _userLocationCacheService.SetCurrentLocationForUserAsync(userCurrentLocation);
                await _userLocationCacheService.SetLocationHistoryForUser(userCurrentLocation);
                await _userLocationCacheService.SetCurrentLocationForAllUsers(userCurrentLocation);

                return new OperationResult<UserCurrentLocation>()
                {
                    Success = true,
                    Model = userCurrentLocation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SetCurrentLocationForUserAsync)} - {model.Id}");
            }

            return new OperationResult<UserCurrentLocation>()
            {
                Success = false,
                Model = default
            };
        }

        public async Task<OperationResult<UserCurrentLocation>> GetCurrentLocationForUserAsync(string userIdentifier)
        {
            var userCurrentLocation = new UserCurrentLocation();
            var success = false;
            try
            {
                userCurrentLocation = await _userLocationCacheService.GetCurrentLocationForUserAsync(userIdentifier);
                success = ((userCurrentLocation != null) && (!string.IsNullOrEmpty(userCurrentLocation.Id)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetCurrentLocationForUserAsync)} - {userIdentifier}");
            }

            return new OperationResult<UserCurrentLocation>()
            {
                Success = success,
                Model = userCurrentLocation
            };
        }

        public async Task<OperationResult<List<UserCurrentLocation>>> GetLocationHistoryForUserAsync(string userIdentifier)
        {
            var locationHistoryForUser = new List<UserCurrentLocation>();
            var success = false;
            try
            {
                locationHistoryForUser = await _userLocationCacheService.GetLocationHistoryForUserAsync(userIdentifier);
                success = ((locationHistoryForUser != null) && (locationHistoryForUser.Count > 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetLocationHistoryForUserAsync));
            }

            return new OperationResult<List<UserCurrentLocation>>()
            {
                Success = success,
                Model = locationHistoryForUser
            };
        }

        public async Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForAllUsersAsync()
        {
            var allUsersCurrentLocation = new List<UserCurrentLocation>();
            var success = false;
            try
            {
                allUsersCurrentLocation = await _userLocationCacheService.GetCurrentLocationForAllUsersAsync();
                success = ((allUsersCurrentLocation != null) && (allUsersCurrentLocation.Count > 0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCurrentLocationForAllUsersAsync));
            }

            return new OperationResult<List<UserCurrentLocation>>()
            {
                Success = success,
                Model = allUsersCurrentLocation
            };
        }

        public async Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForUsersInAreaAsync(AreaBoundary areaBoundary)
        {
            try
            {
                var allUsersCurrentLocation = await _userLocationCacheService.GetCurrentLocationForAllUsersAsync();

                var leftBoundary = 0D;
                var rightBoundary = 0D;

                // TODO: This logic needs checking - check for libraries/better data types for geolocation
                if (Math.Abs(areaBoundary.WesternBoundary) <= 90 && Math.Abs(areaBoundary.EasternBoundary) <= 90)
                {
                    if ((areaBoundary.WesternBoundary < areaBoundary.EasternBoundary))
                    {
                        leftBoundary = areaBoundary.WesternBoundary;
                        rightBoundary = areaBoundary.EasternBoundary;
                    }
                    else
                    {
                        leftBoundary = areaBoundary.EasternBoundary;
                        rightBoundary = areaBoundary.WesternBoundary;
                    }
                }
                else
                {
                    if ((areaBoundary.WesternBoundary > areaBoundary.EasternBoundary))
                    {
                        leftBoundary = areaBoundary.WesternBoundary;
                        rightBoundary = areaBoundary.EasternBoundary;
                    }
                    else
                    {
                        leftBoundary = areaBoundary.EasternBoundary;
                        rightBoundary = areaBoundary.WesternBoundary;
                    }
                }

                var usersInArea = allUsersCurrentLocation
                    .Where(u => u.CurrentLocation.Latitude < areaBoundary.NorthernBoundary)
                    .Where(u => u.CurrentLocation.Latitude > areaBoundary.SouthernBoundary)
                    .Where(u => (u.CurrentLocation.Longitude > leftBoundary) || (u.CurrentLocation.Longitude < rightBoundary))
                    .ToList();

                return new OperationResult<List<UserCurrentLocation>>()
                {
                    Success = true,
                    Model = usersInArea
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCurrentLocationForAllUsersAsync));
            }

            return new OperationResult<List<UserCurrentLocation>>()
            {
                Success = false,
                Model = new List<UserCurrentLocation>()
            };
        }

        public async Task<OperationResult<List<UserCurrentLocation>>> GetCurrentLocationForUsersNearLocationAsync(double latitude, double longitude, int radiusInNm)
        {
            try
            {
                var allUsersCurrentLocation = await _userLocationCacheService.GetCurrentLocationForAllUsersAsync();

                var usersInArea = allUsersCurrentLocation
                    .Where(u =>
                    {
                        var a = u.CurrentLocation.Latitude - latitude;
                        var b = u.CurrentLocation.Longitude - longitude;
                        return Math.Sqrt(Math.Pow(a, 2.0) + Math.Pow(b, 2.0)) < radiusInNm;
                    })
                    .ToList();

                return new OperationResult<List<UserCurrentLocation>>()
                {
                    Success = true,
                    Model = usersInArea
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCurrentLocationForAllUsersAsync));
            }

            return new OperationResult<List<UserCurrentLocation>>()
            {
                Success = false,
                Model = new List<UserCurrentLocation>()
            };
        }
    }
}
