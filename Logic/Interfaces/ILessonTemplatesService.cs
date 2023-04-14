using Logic.Helpers;
using Logic.Models.LessoonTemplate;

namespace Logic.Interfaces
{
    public interface ILessonTemplatesService
    {
        public Task<Result<LessonTemplateOutDTO[]>> GetTemplatesAsync(string tutorLogin);
        public Task<Result<LessonTemplateOutDTO>> GetTemplateAsync(int id);
        public Task<Result<(int id, bool isInserted)>> InsertOrUpdateTemplateAsync(string tutorLogin, LessonTemplateAddDTO templateAddDto);
        public Task<Result<bool>> DeleteTemplateAsync(string tutorLogin, int templateId);
    }
}
