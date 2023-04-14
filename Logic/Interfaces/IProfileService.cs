using Logic.Models;
using Logic.Helpers;

namespace Logic.Interfaces
{
    public interface IProfileService
    {
        Task<Result<ProfileDTO>> GetProfileInfo(string login, string? senderLogin);
        Task<Result<ProfileDTO>> ChangeProfile(string token, ProfileDTO modelDTO);
    }
}
