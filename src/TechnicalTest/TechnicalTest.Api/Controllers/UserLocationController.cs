using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TechnicalTest.Api.Documents.Set;

namespace TechnicalTest.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserLocationController : ControllerBase
    {
        private readonly ILogger<UserLocationController> _logger;

        public UserLocationController(ILogger<UserLocationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
		[Authorize]
        [Route("SetCurrentLocationForUser")]
        public async Task<IActionResult> SetCurrentLocationForUser([FromBody]UserCurrentLocationUpdate userCurrentLocation)
        {
            _logger.LogInformation($"{nameof(SetCurrentLocationForUser)}: Id: {userCurrentLocation.Id}, Lat: {userCurrentLocation.CurrentLocation.Latitude}, Long: {userCurrentLocation.CurrentLocation.Longitude}");
            return StatusCode(500);
        }

        [HttpGet]
        [Route("GetCurrentLocationForUser/{userIdentifier}")]
		public async Task<IActionResult> GetCurrentLocationForUser([FromRoute]string userIdentifier)
		{
			_logger.LogInformation($"{nameof(GetCurrentLocationForUser)}: {userIdentifier}");
            return StatusCode(500);
        }

        [HttpGet]
        [Route("GetCurrentLocationForAllUsers")]
        public async Task<IActionResult> GetCurrentLocationForAllUsers()
        {
            _logger.LogInformation($"{nameof(GetCurrentLocationForAllUsers)}");
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
            _logger.LogInformation("GetCurrentUsersNearLocation");
            return StatusCode(500);
        }
    }
}