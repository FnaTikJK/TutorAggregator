using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorWorksController : ControllerBase
    {
        private readonly ITutorWorksService _tutorWorksService;

        public TutorWorksController(DataContext database, ITutorWorksService tutorWorksService)
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

        [HttpGet]
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
