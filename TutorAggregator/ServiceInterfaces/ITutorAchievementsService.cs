using ResultOfTask;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface ITutorAchievementsService
    {
        Task<Result<bool>> Create(string tutorLogin, TutorAchievementsDto achievementsDto);
        Task<Result<List<TutorAchievementsDto>>> GetAchievements(string tutorLogin);
        Task<Result<bool>> DeleteAchievement(string tutorLogin, int id);
        Task<Result<bool>> ChangeAchievement(string tutorLogin, TutorAchievementsDto achievementsDto);
    }
}
