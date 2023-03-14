using ResultOfTask;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
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
