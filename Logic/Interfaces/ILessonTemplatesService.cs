using ResultOfTask;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface ILessonTemplatesService
    {
        public Task<Result<LessonTemplateDTO[]>> GetTemplates(string tutorLogin);
        public Task<Result<LessonTemplateDTO>> GetTemplate(int id);
        public Task<Result<bool>> TryCreateTemplate(string tutorLogin, LessonTemplateDTO templateDto);
        public Task<Result<bool>> TryChangeTemplate(string tutorLogin, LessonTemplateDTO templateDto);
        public Task<Result<bool>> TryDeleteTemplate(string tutorLogin, int templateId);
    }
}
