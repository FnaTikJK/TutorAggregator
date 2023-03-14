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
    public class LessonTemplateController : ControllerBase
    {
        private ILessonTemplatesService _templateService;

        public LessonTemplateController(ILessonTemplatesService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<ActionResult<LessonTemplateDTO[]>> GetTemplates(string tutorLogin)
        {
            var response = await _templateService.GetTemplates(tutorLogin);
            
            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonTemplateDTO>> GetTemplate(int id)
        {
            var response = await _templateService.GetTemplate(id);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost("Create")]
        public async Task<ActionResult> CreateTemplate(LessonTemplateDTO templateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _templateService.TryCreateTemplate(tutorLogin, templateDto);
            
            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut("Change")]
        public async Task<ActionResult> ChangeTemplate(LessonTemplateDTO templateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _templateService.TryChangeTemplate(tutorLogin, templateDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteTemplate(int templateId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await _templateService.TryDeleteTemplate(tutorLogin, templateId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
    }
}
