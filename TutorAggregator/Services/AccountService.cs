using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Helpers;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _database;
        private readonly IJWTService _jwtService;

        public AccountService(DataContext database, IJWTService jwtService)
        {
            _database = database;
            _jwtService = jwtService;
        }

        public async Task<Result<string>> Register<TEntity>(AccountRegDTO accRegDTO)
             where TEntity : class, IAccountEntity, new()
        {
            var (user, role) = await _database.FindUserAsync(accRegDTO.Login);
            if (user != null)
                return Result.Fail<string>("Аккаунт с таким логином уже существует");

            await _database.Set<TEntity>().AddAsync(new TEntity
            {
                Login = accRegDTO.Login,
                PasswordHash = PasswordHasher.ComputeHash(accRegDTO.Password),
            });
            await _database.SaveChangesAsync();

            return await Authenticate(new AccountAuthDTO
            {
                Login = accRegDTO.Login,
                Password = accRegDTO.Password,
            });
        }

        public async Task<Result<string>> Authenticate(AccountAuthDTO accAuthDTO)
        {
            var (user, role) = await _database.FindUserAsync(accAuthDTO.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");
            
            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash, accAuthDTO.Password);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            return Result.Ok(_jwtService.CreateToken(user.Login, role));
        }

        public async Task<Result<string>> ChangePassword(AccountChangePasswordDTO accChangePasswordDTO)
        {
            var (user, role) = await _database.FindUserAsync(accChangePasswordDTO.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");

            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash,
                accChangePasswordDTO.OldPassword);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            user.PasswordHash = PasswordHasher.ComputeHash(accChangePasswordDTO.NewPassword);
            await _database.SaveChangesAsync();

            return Result.Ok("");
        }
    }
}
