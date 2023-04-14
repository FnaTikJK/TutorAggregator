using AutoMapper;
using DAL.Entities;
using Logic.Helpers;
using Logic.Services.Helpers;
using Logic.Interfaces;
using DAL.Entities.Enums;
using DAL.Interfaces;
using Logic.Models.Account;

namespace Logic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IJWTService jwtService;
        private readonly IUserRepository userRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;

        public AccountService(IMapper mapper, IJWTService jwtService, IStudentRepository studentRepository,
            ITutorRepository tutorRepository, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.jwtService = jwtService;
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<string>> RegisterAsync(AccountRegDTO accountRegDto)
        {
            var user = await userRepository.GetUserAsync(accountRegDto.Login);
            if (user != null)
                return Result.Fail<string>("Аккаунт с таким логином уже существует");
            
            switch (accountRegDto.Role)
            {
                case Role.Student:
                    await studentRepository.AddAsync(mapper.Map<Student>(accountRegDto));
                    await studentRepository.SaveChangesAsync();
                    break;
                case Role.Tutor:
                    await tutorRepository.AddAsync(mapper.Map<Tutor>(accountRegDto));
                    await tutorRepository.SaveChangesAsync();
                    break;
            }

            return await Authenticate(new AccountAuthDTO
            {
                Login = accountRegDto.Login,
                Password = accountRegDto.Password
            });
        }

        public async Task<Result<string>> Authenticate(AccountAuthDTO accountAuthDto)
        {
            var user = await userRepository.GetUserAsync(accountAuthDto.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");

            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash, accountAuthDto.Password);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            return Result.Ok(jwtService.CreateToken(user.Login, user.Role));
        }

        public async Task<Result<string>> ChangePassword(AccountChangePasswordDTO accountChangePasswordDto)
        {
            var user = await userRepository.GetUserAsync(accountChangePasswordDto.Login);
            if (user == null)
                return Result.Fail<string>("Аккаунта с таким логином не существует");

            var isCorrectPassword = PasswordHasher.ComparePasswordWithHashed(user.PasswordHash, accountChangePasswordDto.OldPassword);
            if (!isCorrectPassword)
                return Result.Fail<string>("Неправильный пароль");

            user.PasswordHash = PasswordHasher.ComputeHash(accountChangePasswordDto.NewPassword);
            await userRepository.UpdateAndSaveAsync(user);

            return Result.Ok("");
        }
    }
}
