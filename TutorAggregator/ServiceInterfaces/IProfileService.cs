using ResultOfTask;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface IProfileService
    {
        Task<Result<ProfileDTO>> GetProfileInfo(string login, string? senderLogin);
        Task<Result<ProfileDTO>> ChangeProfile(string token, ProfileDTO modelDTO);
    }
}
