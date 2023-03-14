using ResultOfTask;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface ITutorWorksService
    {
        Task<Result<bool>> Create(string tutorLogin, TutorWorkDto workDto);
        Task<Result<List<TutorWorkDto>>> GetWorks(string tutorLogin);
        Task<Result<bool>> DeleteWork(string tutorLogin, int id);
        Task<Result<bool>> ChangeWork(string tutorLogin, TutorWorkDto workDto);
    }
}
