using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultOfTask;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorAchievementsController : ControllerBase
    {
        private ITutorAchievementsService _tutorAchievementsService;

        public TutorAchievementsController(ITutorAchievementsService tutorAchievementsService)
        {
            _tutorAchievementsService = tutorAchievementsService;
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost]
        public async Task<ActionResult> CreateAchievement(TutorAchievementsDto achievementsDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _tutorAchievementsService.Create(tutorLogin, achievementsDto);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [HttpGet]
        public async Task<ActionResult> GetAchievements(string tutorLogin)
        {
            Result<List<TutorAchievementsDto>> response = await _tutorAchievementsService.GetAchievements(tutorLogin);

            return response.IsSuccess
                ? Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles="Tutor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAchievement(int id)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _tutorAchievementsService.DeleteAchievement(tutorLogin, id);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut]
        public async Task<ActionResult> ChangeAchievement(TutorAchievementsDto achievementsDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            Result<bool> response = await _tutorAchievementsService.ChangeAchievement(tutorLogin, achievementsDto);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }
    }
}
