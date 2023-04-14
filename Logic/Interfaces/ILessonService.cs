using Logic.Helpers;
using Logic.Models.Lesson;

namespace Logic.Interfaces
{
    public interface ILessonService
    {
        Task<Result<bool>> AddAsync(string tutorId, LessonCreateOrUpdateDTO lessonCreateOrUpdateDto);
        Task<Result<List<LessonOutDTO>>> GetAllByLoginAsync(string userLogin);
        Task<Result<bool>> DeleteAsync(string tutorLogin, int lessonId);
        Task<Result<bool>> SubscribeStudent(string studentLogin, int lessonId, int chosenTemplateId);
        Task<Result<bool>> UnsubscribeStudent(string userLogin, int lessonId);
        Task<Result<bool>> ChangeLesson(string tutorLogin, LessonCreateOrUpdateDTO lessonCreateOrUpdateDto);
        Task<Result<LessonOutDTO>> GetByIdAsync(int id);
    }
}
