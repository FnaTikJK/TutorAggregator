using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("GetInfo")]
        public async Task<ActionResult> GetInfo(string login)
        {
            var response = await _profileService.GetProfileInfo(login, null!);
            
            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles="Student")]
        [HttpGet("GetInfoAuth")]
        public async Task<ActionResult> GetFullInfo(string login)
        {
            var senderLogin = HttpContext.User.Claims.First().Value;
            var response = await _profileService.GetProfileInfo(login, senderLogin);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [HttpPut("ChangeInfo")]
        [Authorize]
        public async Task<ActionResult> ChangeInfo([FromBody]ProfileDTO profileDTO)
        {
            var login = HttpContext.User.Claims.First().Value;
            var response = await _profileService.ChangeProfile(login, profileDTO);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
    }
}
