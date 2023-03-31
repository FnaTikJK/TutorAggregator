using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Interfaces;
using Logic.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LessonTemplatesController : ControllerBase
    {
        private readonly ILessonTemplatesService templateService;

        public LessonTemplatesController(ILessonTemplatesService templateService)
        {
            this.templateService = templateService;
        }

        [HttpGet]
        public async Task<ActionResult<LessonTemplateDTO[]>> GetTemplates(string tutorLogin)
        {
            var response = await templateService.GetTemplates(tutorLogin);
            
            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonTemplateDTO>> GetTemplate(int id)
        {
            var response = await templateService.GetTemplate(id);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost]
        public async Task<ActionResult> CreateTemplate(LessonTemplateDTO templateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await templateService.TryCreateTemplate(tutorLogin, templateDto);
            
            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut]
        public async Task<ActionResult> ChangeTemplate(LessonTemplateDTO templateDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await templateService.TryChangeTemplate(tutorLogin, templateDto);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("{templateId}")]
        public async Task<ActionResult> DeleteTemplate(int templateId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await templateService.TryDeleteTemplate(tutorLogin, templateId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
    }
}
