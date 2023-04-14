using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Logic.Helpers;
using Logic.Models;
using Logic.Interfaces;
using DAL.Core;

namespace Logic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext database;
        private readonly IMapper mapper;
        private readonly ILessonService lessonService;

        public ProfileService(DataContext database, IMapper mapper, ILessonService lessonService)
        {
            this.database = database;
            this.mapper = mapper;
            this.lessonService = lessonService;
        }

        public async Task<Result<ProfileDTO>> GetProfileInfo(string login, string? senderLogin)
        {
            var (user, role) = await database.FindUserAsync(login);
            if (user == null)
                return Result.Fail<ProfileDTO>("Такого пользователя не существует");

            switch (role)
            {
                case "Student":
                    {
                        return Result.Ok(
                            mapper.Map<ProfileDTO>(await database.Set<Student>().FirstAsync(e => e.Login == login)));
                    }
                case "Tutor":
                    {
                        var profileDto = mapper.Map<ProfileDTO>(await database.Set<Tutor>().FirstAsync(e => e.Login == login));
                        if (senderLogin == null)
                        {
                            var (userSender, roleSender) = await database.FindUserAsync(senderLogin);
                            if (userSender == null || roleSender == "Student"
                                               && (await lessonService.GetAllByLoginAsync(login)).Value.All(e =>
                                                   e.Student.Login != userSender.Login))
                                profileDto.PhoneNumber = "Скрыто";
                        }
                        return Result.Ok(profileDto);
                    }
                default:
                    return Result.Fail<ProfileDTO>("");
            };
        }

        public async Task<Result<ProfileDTO>> ChangeProfile(string login, ProfileDTO profileDTO)
        {
            var (user, role) = await database.FindUserAsync(login);
            if (user == null)
                return Result.Fail<ProfileDTO>("Такого пользователя не существует");

            switch (role)
            {
                case "Student":
                    {
                        var student = await database.Students.FirstOrDefaultAsync(e => e.Login == login);

                        mapper.Map(profileDTO, student);
                        await database.SaveChangesAsync();
                        return Result.Ok(profileDTO);
                    }
                case "Tutor":
                    {
                        var tutor = await database.Set<Tutor>().FirstAsync(e => e.Login == login);

                        mapper.Map(profileDTO, tutor);
                        await database.SaveChangesAsync();
                        return Result.Ok(profileDTO);
                    }
                default:
                    return Result.Fail<ProfileDTO>("");
            }
        }
    }
}
