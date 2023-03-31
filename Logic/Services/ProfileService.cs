using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using DAL;
using DAL.Entities;
using ResultOfTask;
using Logic.Models;
using Logic.Interfaces;

namespace Logic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext _database;
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly ISearchService _tree;

        public ProfileService(DataContext database, IMapper mapper, ILessonService lessonService, ISearchService tree)
        {
            _database = database;
            _mapper = mapper;
            _lessonService = lessonService;
            _tree = tree;
        }

        public async Task<Result<ProfileDTO>> GetProfileInfo(string login, string? senderLogin)
        {
            var (user, role) = await _database.FindUserAsync(login);
            if (user == null)
                return Result.Fail<ProfileDTO>("Такого пользователя не существует");

            switch (role)
            {
                case "Student":
                    {
                        return Result.Ok(
                            _mapper.Map<ProfileDTO>(await _database.Set<Student>().FirstAsync(e => e.Login == login)));
                    }
                case "Tutor":
                    {
                        var profileDto = _mapper.Map<ProfileDTO>(await _database.Set<Tutor>().FirstAsync(e => e.Login == login));
                        if (senderLogin == null)
                        {
                            var (userSender, roleSender) = await _database.FindUserAsync(senderLogin);
                            if (userSender == null || roleSender == "Student"
                                               && (await _lessonService.GetLessons(login)).Value.All(e =>
                                                   e.StudentId != userSender.Id))
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
            var (user, role) = await _database.FindUserAsync(login);
            if (user == null)
                return Result.Fail<ProfileDTO>("Такого пользователя не существует");

            switch (role)
            {
                case "Student":
                    {
                        var student = await _database.Students.FirstOrDefaultAsync(e => e.Login == login);

                        var newStudent = _mapper.Map<Student>(profileDTO);
                        newStudent.Id = student.Id;
                        newStudent.Login = student.Login;
                        newStudent.PasswordHash = student.PasswordHash;

                        _database.Entry(student)
                            .CurrentValues.SetValues(newStudent);
                        await _database.SaveChangesAsync();
                        return Result.Ok(profileDTO);
                    }
                case "Tutor":
                    {
                        var tutor = await _database.Set<Tutor>().FirstAsync(e => e.Login == login);
                        var tutorDTO = new TutorSearchDTO(tutor);

                        var newTutor = _mapper.Map<Tutor>(profileDTO);
                        newTutor.Id = tutor.Id;
                        newTutor.Login = tutor.Login;
                        newTutor.PasswordHash = tutor.PasswordHash;

                        _database.Entry(tutor)
                            .CurrentValues.SetValues(newTutor);
                        tutorDTO = new TutorSearchDTO(newTutor);
                        await _database.SaveChangesAsync();
                        return Result.Ok(profileDTO);
                    }
                default:
                    return Result.Fail<ProfileDTO>("");
            }
        }
    }
}
