using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents;
using TechnicalTest.Api.Documents.Get;
using TechnicalTest.Api.Documents.Set;

namespace TechnicalTest.Api.Services
{
    public class UserLocationService : IUserLocationService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<UserLocationService> _logger;

        private const string ALL_USERS_CURRENT_LOCATION_KEY = "all-users-current-location";

        private const string USER_CURRENT_LOCATION_KEY_SUFFIX = "-current-location";
        private const string USER_LOCATION_HISTORY_SUFFIX = "-location-history";

        public UserLocationService(IDistributedCache distributedCache, ILogger<UserLocationService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        #region public methods

        public async Task<OperationResult<UserCurrentLocation>> SetCurrentLocationForUserAsync(UserCurrentLocationUpdate model)
        {
            var success = false;
            var userCurrentLocation = new UserCurrentLocation();
            try
            {
                userCurrentLocation.Id = model.Id;
                userCurrentLocation.CurrentLocation = model.CurrentLocation;
                userCurrentLocation.TimeAtLocation = DateTime.UtcNow;

                await UpdateCurrentLocationForUser(userCurrentLocation);
                await UpdateLocationHistoryForUser(userCurrentLocation);
                await UpdateAllUsersCurrentLocation(userCurrentLocation);

                success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SetCurrentLocationForUserAsync)} - {model.Id}");
            }

            return new OperationResult<UserCurrentLocation>()
            {
                Success = success,
                Model = userCurrentLocation
            };
        }

        public async Task<OperationResult<UserCurrentLocation>> GetCurrentLocationForUserAsync(string userIdentifier)
        {
            try
            {
                var key = $"{userIdentifier}{USER_CURRENT_LOCATION_KEY_SUFFIX}";
                var userCurrentLocationJson = await _distributedCache.GetStringAsync(key);
                var userCurrentLocation = JsonSerializer.Deserialize<UserCurrentLocation>(userCurrentLocationJson);
                return new OperationResult<UserCurrentLocation>()
                {
                    Success = true,
                    Model = userCurrentLocation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetCurrentLocationForUserAsync)} - {userIdentifier}");
            }

            return new OperationResult<UserCurrentLocation>()
            {
                Success = false,
                Model = new UserCurrentLocation()
            };
        }

        #endregion

        #region private methods

        private async Task UpdateCurrentLocationForUser(UserCurrentLocation userCurrentLocation)
        {
            try
            {
                var key = $"{userCurrentLocation.Id}{USER_CURRENT_LOCATION_KEY_SUFFIX}";
                var userCurrentLocationJson = JsonSerializer.Serialize(userCurrentLocation);
                await _distributedCache.SetStringAsync(key, userCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(UpdateCurrentLocationForUser): {userCurrentLocation.Id}");
                throw new Exception($"nameof(UpdateCurrentLocationForUser): {userCurrentLocation.Id}", ex);
            }
        }

        private async Task UpdateLocationHistoryForUser(UserCurrentLocation userCurrentLocation)
        {
            var userLocationHistory = new List<UserCurrentLocation>();
            try
            {
                var key = $"{userCurrentLocation.Id}{USER_LOCATION_HISTORY_SUFFIX}";
                var userLocationHistoryJson = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrWhiteSpace(userLocationHistoryJson))
                    userLocationHistory = JsonSerializer.Deserialize<List<UserCurrentLocation>>(userLocationHistoryJson);
                userLocationHistory.Add(userCurrentLocation);
                userLocationHistoryJson = JsonSerializer.Serialize(userLocationHistory);
                await _distributedCache.SetStringAsync(key, userLocationHistoryJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(UpdateLocationHistoryForUser): {userCurrentLocation.Id}");
                throw new Exception($"nameof(UpdateLocationHistoryForUser): {userCurrentLocation.Id}", ex);
            }
        }

        private async Task UpdateAllUsersCurrentLocation(UserCurrentLocation userCurrentLocation)
        {
            var allUsersCurrentLocation = new List<UserCurrentLocation>();
            try
            {
                allUsersCurrentLocation = await GetAllUsersCurrentLocation();
                allUsersCurrentLocation.Add(userCurrentLocation);
                var allUsersCurrentLocationJson = JsonSerializer.Serialize(allUsersCurrentLocation);
                await _distributedCache.SetStringAsync(ALL_USERS_CURRENT_LOCATION_KEY, allUsersCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(UpdateAllUsersCurrentLocation): {userCurrentLocation.Id}");
                throw new Exception($"nameof(UpdateAllUsersCurrentLocation): {userCurrentLocation.Id}", ex);
            }
        }

        private async Task<List<UserCurrentLocation>> GetAllUsersCurrentLocation()
        {
            var allUsersCurrentLocation = new List<UserCurrentLocation>();
            try
            {
                var allUsersCurrentLocationJson = await _distributedCache.GetStringAsync(ALL_USERS_CURRENT_LOCATION_KEY);
                if (!string.IsNullOrWhiteSpace(allUsersCurrentLocationJson))
                    allUsersCurrentLocation = JsonSerializer.Deserialize<List<UserCurrentLocation>>(allUsersCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(GetAllUsersCurrentLocation)");
                throw new Exception($"nameof(GetAllUsersCurrentLocation):", ex);
            }

            return allUsersCurrentLocation;
        }

        #endregion
    }
}
