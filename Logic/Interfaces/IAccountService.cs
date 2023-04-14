using DAL.Entities;
using Logic.Models.Account;
using Logic.Helpers;

namespace Logic.Interfaces
{
    public interface IAccountService
    {
        Task<Result<string>> RegisterAsync(AccountRegDTO accountRegDto);
        Task<Result<string>> Authenticate(AccountAuthDTO accountAuthDto);
        Task<Result<string>> ChangePassword(AccountChangePasswordDTO accountChangePasswordDto);
    }
}
