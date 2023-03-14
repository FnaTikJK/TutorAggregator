using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LessonController : ControllerBase
    {
        private ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost("Create")]
        public async Task<ActionResult> CreateLesson(LessonDTO lessonDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.TryCreateLesson(tutorLogin, lessonDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
        
        [HttpGet("All")]
        public async Task<ActionResult<List<LessonDTO>>> GetLessons([FromQuery]string userLogin)
        {
            var response = await _lessonService.GetLessons(userLogin);

            return response.IsSuccess ? 
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteLesson([FromQuery] int lessonId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.Delete(tutorLogin, lessonId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut("Change")]
        public async Task<ActionResult> ChangeLesson(LessonDTO lessonDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.ChangeLesson(tutorLogin, lessonDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Student")]
        [HttpPut("Subscribe")]
        public async Task<ActionResult> SubscribeToLesson([FromQuery] int lessonId, [FromQuery] int chosenTemplateId)
        {
            var studentLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.SubscribeStudent(studentLogin, lessonId, chosenTemplateId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize]
        [HttpPut("Unsubscribe")]
        public async Task<ActionResult> UnsubscribeToLesson([FromQuery] int lessonId)
        {
            var userLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.UnsubscribeStudent(userLogin, lessonId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
    }
}
