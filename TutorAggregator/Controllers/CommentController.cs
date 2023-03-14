using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;


namespace TutorAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CommentController: ControllerBase
    {
        private ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {   
            _commentService = commentService;
        }

        [HttpGet("StudentComments")]
        public async Task<ActionResult<CommentDTO[]>> GetStudentComments(string studentLogin)
        {
            var comments = await _commentService.GetStudentComments(studentLogin);
            if (comments == null)
                return BadRequest("Неверный логин");

            return Ok(comments);
        }

        [HttpGet("TutorComments")]
        public async Task<ActionResult<CommentDTO[]>> GetTutorComments(string tutorLogin)
        {
            var comments = await _commentService.GetTutorComments(tutorLogin);
            if (comments == null)
                return BadRequest("Неверный логин");

            return Ok(comments);
        }
        [HttpGet("TutorRating")]
        public async Task<ActionResult<double?>> GetTutorRating(string tutorLogin)
        {
            var rating = await _commentService.GetTutorRating(tutorLogin);
            if (rating == null)
                return BadRequest("Неверный логин");

            return Ok(rating);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("Create")]
        public async Task<ActionResult> CreateComment(CommentDTO commentDto)
        {
            var studentLogin = HttpContext.User.Claims.First().Value;
            var res = await _commentService.TryCreateComment(studentLogin, commentDto);
            if (!res)
                return BadRequest("Нет такого преподавателя или комментарий уже создан!");

            return Ok("Success!");
        }

        [Authorize(Roles = "Student")]
        [HttpPut("Change")]
        public async Task<ActionResult> ChangeComment(CommentDTO commentDto)
        {
            var studentLogin = HttpContext.User.Claims.First().Value;
            var res = await _commentService.TryChangeComment(studentLogin, commentDto);
            if (!res)
                return BadRequest("Не удалось изменить комментарий");

            return Ok("Success!");
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteTemplate(string tutorLogin, int commentId)
        {
            var studentLogin = HttpContext.User.Claims.First().Value;
            var res = await _commentService.TryDeleteComment(tutorLogin, studentLogin, commentId);
            if (!res)
                return BadRequest("Не удалось удалить комментарий");

            return Ok("Success!");
        }

    }
}
