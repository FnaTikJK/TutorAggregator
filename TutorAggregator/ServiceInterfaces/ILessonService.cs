using ResultOfTask;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface ILessonService
    {
        Task<Result<bool>> TryCreateLesson(string tutorId, LessonDTO lessonDto);
        Task<Result<List<LessonDTO>>> GetLessons(string userLogin);
        Task<Result<bool>> Delete(string tutorLogin, int lessonId);
        Task<Result<bool>> SubscribeStudent(string studentLogin, int lessonId, int chosenTemplateId);
        Task<Result<bool>> UnsubscribeStudent(string userLogin, int lessonId);
        Task<Result<bool>> ChangeLesson(string tutorLogin, LessonDTO lessonDto);
    }
}
