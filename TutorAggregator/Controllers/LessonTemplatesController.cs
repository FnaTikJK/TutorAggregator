using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logic.Interfaces;
using Logic.Models.LessoonTemplate;

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
        public async Task<ActionResult<LessonTemplateOutDTO[]>> GetTemplates(string tutorLogin)
        {
            var response = await templateService.GetTemplatesAsync(tutorLogin);
            
            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [HttpGet("{id}", Name = nameof(GetTemplate))]
        public async Task<ActionResult<LessonTemplateOutDTO>> GetTemplate(int id)
        {
            var response = await templateService.GetTemplateAsync(id);

            return response.IsSuccess ?
                Ok(response.Value)
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut]
        public async Task<ActionResult> InsertOrUpdateTemplate(LessonTemplateAddDTO templateOutDto)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await templateService.InsertOrUpdateTemplateAsync(tutorLogin, templateOutDto);
            
            return response.IsSuccess ?
                response.Value.isInserted ? CreatedAtRoute(nameof(GetTemplate), new { id = response.Value.id }, response.Value.id) 
                    : Ok()
                : BadRequest(response.Error);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("{templateId}")]
        public async Task<ActionResult> DeleteTemplate(int templateId)
        {
            var tutorLogin = HttpContext.User.Claims.First().Value;
            var response = await templateService.DeleteTemplateAsync(tutorLogin, templateId);

            return response.IsSuccess ?
                Ok()
                : BadRequest(response.Error);
        }
    }
}
