using DAL.Entities;
using Logic.Models;
using ResultOfTask;

namespace Logic.Interfaces
{
    public interface IAccountService
    {
        Task<Result<string>> Register<TEntity>(AccountRegDTO accountRegDto)
            where TEntity : class, IAccountEntity, new ();
        Task<Result<string>> Authenticate(AccountAuthDTO accountAuthDto);
        Task<Result<string>> ChangePassword(AccountChangePasswordDTO accountChangePasswordDto);
    }
}
