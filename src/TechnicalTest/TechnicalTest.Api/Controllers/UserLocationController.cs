using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.Set;
using TechnicalTest.Api.Services;

namespace TechnicalTest.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserLocationController : ControllerBase
    {
        private readonly ILogger<UserLocationController> _logger;
        private readonly IUserLocationService _userLocationService;

        public UserLocationController(ILogger<UserLocationController> logger, IUserLocationService userLocationService)
        {
            _logger = logger;
            _userLocationService = userLocationService;
        }

        [HttpPost]
        [Authorize]
        [Route("SetCurrentLocationForUser")]
        public async Task<IActionResult> SetCurrentLocationForUser([FromBody]UserCurrentLocationUpdate userCurrentLocation)
        {
            // TODO: validation, return BadRequest
            _logger.LogInformation($"{nameof(SetCurrentLocationForUser)}: Id: {userCurrentLocation.Id}, Lat: {userCurrentLocation.CurrentLocation.Latitude}, Long: {userCurrentLocation.CurrentLocation.Longitude}");
            var result = await _userLocationService.SetCurrentLocationForUserAsync(userCurrentLocation);
            if (result.Success)
                return CreatedAtAction(nameof(SetCurrentLocationForUser), result.Model);

            return StatusCode(500);
        }

        [HttpGet]
        [Route("GetCurrentLocationForUser/{userIdentifier}")]
        public async Task<IActionResult> GetCurrentLocationForUser([FromRoute]string userIdentifier)
        {
            _logger.LogInformation($"{nameof(GetCurrentLocationForUser)}: {userIdentifier}");
            var result = await _userLocationService.GetCurrentLocationForUserAsync(userIdentifier);
            if (result.Success)
                return Ok(result.Model);

            return NotFound(userIdentifier);
        }

        [HttpGet]
        [Route("GetLocationHistoryForUser/{userIdentifier}")]
        public async Task<IActionResult> GetLocationHistoryForUser([FromRoute]string userIdentifier)
        {
            _logger.LogInformation($"{nameof(GetLocationHistoryForUser)}: {userIdentifier}");
            var result = await _userLocationService.GetLocationHistoryForUserAsync(userIdentifier);
            if (result.Success)
                return Ok(result.Model);

            return NotFound(userIdentifier);
        }

        [HttpGet]
        [Route("GetCurrentLocationForAllUsers")]
        public async Task<IActionResult> GetCurrentLocationForAllUsers()
        {
            _logger.LogInformation($"{nameof(GetCurrentLocationForAllUsers)}");
            var result = await _userLocationService.GetCurrentLocationForAllUsersAsync();
            if (result.Success)
                return Ok(result.Model);

            return StatusCode(500);
        }

        [HttpPost]
        [Route("GetCurrentLocationForUsersInArea")]
        public async Task<IActionResult> GetCurrentLocationForUsersInArea([FromBody]AreaBoundary areaBoundary)
        {
            _logger.LogInformation($"{nameof(GetCurrentLocationForUsersInArea)}");
            return StatusCode(500);
        }

        [HttpGet]
        [Route("GetCurrentLocationForUsersNearLocation/{latitude}/{longitude}/{radiusInNm}")]
        public async Task<IActionResult> GetCurrentLocationForUsersNearLocation([FromRoute]double latitude, [FromRoute]double longitude, [FromRoute]int radiusInNm)
        {
            _logger.LogInformation($"{nameof(GetCurrentLocationForUsersNearLocation)}");
            return StatusCode(500);
        }
    }
}