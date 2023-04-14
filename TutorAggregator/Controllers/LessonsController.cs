using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Interfaces;
using Logic.Models.Lesson;

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
        public async Task<ActionResult> CreateLesson(LessonCreateOrUpdateDTO lessonCreateOrUpdateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.AddAsync(tutorLogin, lessonCreateOrUpdateDto);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonOutDTO>>> GetLessons([FromQuery] string userLogin)
        {
            var response = await _lessonService.GetAllByLoginAsync(userLogin);

            return response.IsSuccess
                ? Ok(response.Value)
                : BadRequest(response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonOutDTO>> GetLessonByIdAsync(int id)
        {
            var response = await _lessonService.GetByIdAsync(id);

            return response.IsSuccess
                ? Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("{lessonId}")]
        public async Task<ActionResult> DeleteLesson(int lessonId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.DeleteAsync(tutorLogin, lessonId);

            return response.IsSuccess
                ? Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPatch]
        public async Task<ActionResult> ChangeLesson([FromBody] LessonCreateOrUpdateDTO lessonCreateOrUpdateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _lessonService.ChangeLesson(tutorLogin, lessonCreateOrUpdateDto);

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
