using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.Get;

namespace TechnicalTest.Api.Services
{
    public class UserLocationCacheService : IUserLocationCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<UserLocationService> _logger;

        private const string ALL_USERS_CURRENT_LOCATION_KEY = "all-users-current-location";
        private const string USER_CURRENT_LOCATION_KEY_SUFFIX = "-current-location";
        private const string USER_LOCATION_HISTORY_SUFFIX = "-location-history";

        public UserLocationCacheService(IDistributedCache distributedCache, ILogger<UserLocationService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task SetCurrentLocationForUserAsync(UserCurrentLocation userCurrentLocation)
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

        public async Task<UserCurrentLocation> GetCurrentLocationForUserAsync(string userIdentifier)
        {
            try
            {
                var key = $"{userIdentifier}{USER_CURRENT_LOCATION_KEY_SUFFIX}";
                var userCurrentLocationJson = await _distributedCache.GetStringAsync(key);
                var userCurrentLocation = new UserCurrentLocation();
                return string.IsNullOrWhiteSpace(userCurrentLocationJson)
                    ? default
                    : userCurrentLocation = JsonSerializer.Deserialize<UserCurrentLocation>(userCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetCurrentLocationForUserAsync)} - {userIdentifier}");
            }

            return default;
        }

        public async Task<List<UserCurrentLocation>> GetLocationHistoryForUserAsync(string userIdentifier)
        {
            try
            {
                // TODO: key making function for consistency
                var key = $"{userIdentifier}{USER_LOCATION_HISTORY_SUFFIX}";
                var userLocationHistoryJson = await _distributedCache.GetStringAsync(key);
                return string.IsNullOrWhiteSpace(userLocationHistoryJson)
                    ? new List<UserCurrentLocation>()
                    : JsonSerializer.Deserialize<List<UserCurrentLocation>>(userLocationHistoryJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetLocationHistoryForUserAsync));
            }
            return default;
        }

        public async Task<List<UserCurrentLocation>> GetCurrentLocationForAllUsersAsync()
        {
            try
            {
                var allUsersCurrentLocationJson = await _distributedCache.GetStringAsync(ALL_USERS_CURRENT_LOCATION_KEY);
                return string.IsNullOrWhiteSpace(allUsersCurrentLocationJson)
                    ? new List<UserCurrentLocation>()
                    : JsonSerializer.Deserialize<List<UserCurrentLocation>>(allUsersCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(GetAllUsersCurrentLocation)");
                throw new Exception($"nameof(GetAllUsersCurrentLocation):", ex);
            }
        }

        public async Task SetLocationHistoryForUser(UserCurrentLocation userCurrentLocation)
        {
            try
            {
                var userLocationHistory = new List<UserCurrentLocation>();
                var key = $"{userCurrentLocation.Id}{USER_LOCATION_HISTORY_SUFFIX}";
                var userLocationHistoryJson = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrWhiteSpace(userLocationHistoryJson))
                    userLocationHistory = JsonSerializer.Deserialize<List<UserCurrentLocation>>(userLocationHistoryJson);
                userLocationHistory.Add(userCurrentLocation);
                await SetLocationHistoryForUser(userLocationHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(UpdateLocationHistoryForUser): {userCurrentLocation.Id}");
                throw new Exception($"nameof(UpdateLocationHistoryForUser): {userCurrentLocation.Id}", ex);
            }
        }

        public async Task SetLocationHistoryForUser(List<UserCurrentLocation> userLocationHistory)
        {
            try
            {
                var key = $"{userLocationHistory.FirstOrDefault().Id}{USER_LOCATION_HISTORY_SUFFIX}";
                var userLocationHistoryJson = JsonSerializer.Serialize(userLocationHistory);
                await _distributedCache.SetStringAsync(key, userLocationHistoryJson);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"nameof(SetLocationHistoryForUser)");
                throw new Exception($"nameof(SetLocationHistoryForUser)", ex);
            }
        }

        public async Task SetCurrentLocationForAllUsers(UserCurrentLocation userCurrentLocation)
        {
            try
            {
                var currentLocationForAllUsers = await GetCurrentLocationForAllUsersAsync();
                if (currentLocationForAllUsers != null)
                {
                    var index = currentLocationForAllUsers.FindIndex(u =>
                    {
                        return String.Compare(u.Id, userCurrentLocation.Id, StringComparison.InvariantCultureIgnoreCase) == 0;
                    });
                    if (index > -1)
                        currentLocationForAllUsers.RemoveAt(index);
                }
                currentLocationForAllUsers.Add(userCurrentLocation);
                await SetCurrentLocationForAllUsers(currentLocationForAllUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SetCurrentLocationForAllUsers)}: {userCurrentLocation.Id}");
                throw new Exception($"{nameof(SetCurrentLocationForAllUsers)}: {userCurrentLocation.Id}", ex);
            }
        }

        public async Task SetCurrentLocationForAllUsers(List<UserCurrentLocation> currentLocationForAllUsers)
        {
            try
            {
                var allUsersCurrentLocationJson = JsonSerializer.Serialize(currentLocationForAllUsers);
                await _distributedCache.SetStringAsync(ALL_USERS_CURRENT_LOCATION_KEY, allUsersCurrentLocationJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SetCurrentLocationForAllUsers)}");
                throw new Exception($"{nameof(SetCurrentLocationForAllUsers)}", ex);
            }
        }
    }
}
