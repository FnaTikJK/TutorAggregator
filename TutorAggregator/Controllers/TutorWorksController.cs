using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorWorksController : ControllerBase
    {
        private readonly ITutorWorksService _tutorWorksService;

        public TutorWorksController(ITutorWorksService tutorWorksService)
        {
            _tutorWorksService = tutorWorksService;
        }

        [Authorize(Roles="Tutor")]
        [HttpPost]
        public async Task<ActionResult> CreateWork(TutorWorkDto workDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            Result<bool> response = await _tutorWorksService.Create(tutorLogin, workDto);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [HttpGet("{tutorLogin}")]
        public async Task<ActionResult> GetWorks(string tutorLogin)
        {
            var response = await _tutorWorksService.GetWorks(tutorLogin);

            return response.IsSuccess
                ? Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWork(int id)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _tutorWorksService.DeleteWork(tutorLogin, id);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut]
        public async Task<ActionResult> ChangeWork(TutorWorkDto workDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _tutorWorksService.ChangeWork(tutorLogin, workDto);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }
    }
}
