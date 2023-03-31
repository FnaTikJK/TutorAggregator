using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logic.Models;
using Logic.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LessonsController : ControllerBase
    {
        private ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost]
        public async Task<ActionResult> CreateLesson(LessonDTO lessonDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.TryCreateLesson(tutorLogin, lessonDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<LessonDTO>>> GetLessons([FromQuery]string userLogin)
        {
            var response = await _lessonService.GetLessons(userLogin);

            return response.IsSuccess ? 
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("{lessonId}")]
        public async Task<ActionResult> DeleteLesson(int lessonId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.Delete(tutorLogin, lessonId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut]
        public async Task<ActionResult> ChangeLesson(LessonDTO lessonDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.ChangeLesson(tutorLogin, lessonDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("Subscribe")]
        public async Task<ActionResult> SubscribeToLesson([FromQuery] int lessonId, [FromQuery] int chosenTemplateId)
        {
            var studentLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.SubscribeStudent(studentLogin, lessonId, chosenTemplateId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize]
        [HttpPost("Unsubscribe")]
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
