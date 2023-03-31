using Logic.Models;
using ResultOfTask;

namespace Logic.Interfaces
{
    public interface ITutorWorksService
    {
        Task<Result<bool>> Create(string tutorLogin, TutorWorkDto workDto);
        Task<Result<List<TutorWorkDto>>> GetWorks(string tutorLogin);
        Task<Result<bool>> DeleteWork(string tutorLogin, int id);
        Task<Result<bool>> ChangeWork(string tutorLogin, TutorWorkDto workDto);
    }
}
