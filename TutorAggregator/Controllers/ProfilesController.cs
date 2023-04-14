using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{login}")]
        public async Task<ActionResult> GetInfo(string login)
        {
            var response = await _profileService.GetProfileInfo(login, null!);
            
            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize]
        [HttpGet("{login}/auth")]
        public async Task<ActionResult> GetFullInfo(string login)
        {
            var senderLogin = HttpContext.User.Claims.First().Value;
            var response = await _profileService.GetProfileInfo(login, senderLogin);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }
        
        [Authorize]
        [HttpPut]
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
