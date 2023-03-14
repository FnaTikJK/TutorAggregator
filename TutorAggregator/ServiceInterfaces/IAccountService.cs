
using ResultOfTask;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;

namespace TutorAggregator.ServiceInterfaces
{
    public interface IAccountService
    {
        Task<Result<string>> Register<TEntity>(AccountRegDTO modelDTO)
            where TEntity : class, IAccountEntity, new ();
        Task<Result<string>> Authenticate(AccountAuthDTO modelDTO);
        Task<Result<string>> ChangePassword(AccountChangePasswordDTO modelDTO);
    }
}
