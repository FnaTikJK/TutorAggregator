using DAL;
using DAL.Entities;
using ResultOfTask;
using Logic.Services.Helpers;
using Logic.Interfaces;
using Logic.Models;

namespace Logic.Services
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

        public async Task<Result<string>> Register<TEntity>(AccountRegDTO accountRegDto)
             where TEntity : class, IAccountEntity, new()
        {
            var (user, role) = await _database.FindUserAsync(accountRegDto.Login);
            if (user != null)
                return Result.Fail<string>("Аккаунт с таким логином уже существует");

            await _database.Set<TEntity>().AddAsync(new TEntity
            {
                Login = accountRegDto.Login,
                PasswordHash = PasswordHasher.ComputeHash(accountRegDto.Password),
            });
            await _database.SaveChangesAsync();

            return await Authenticate(new AccountAuthDTO
            {
                Login = accountRegDto.Login,
                Password = accountRegDto.Password
            });
        }

        public async Task<Result<string>> Authenticate(AccountAuthDTO accountAuthDto)
        {
            var (user, role) = await _database.FindUserAsync(accountAuthDto.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");

            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash, accountAuthDto.Password);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            return Result.Ok(_jwtService.CreateToken(user.Login, role));
        }

        public async Task<Result<string>> ChangePassword(AccountChangePasswordDTO accountChangePasswordDto)
        {
            var (user, role) = await _database.FindUserAsync(accountChangePasswordDto.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");

            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash, accountChangePasswordDto.OldPassword);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            user.PasswordHash = PasswordHasher.ComputeHash(accountChangePasswordDto.NewPassword);
            await _database.SaveChangesAsync();

            return Result.Ok("");
        }
    }
}
