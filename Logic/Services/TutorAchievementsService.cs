using AutoMapper;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Logic.Helpers;
using Logic.Models;
using Logic.Interfaces;
using DAL.Core;

namespace Logic.Services
{
    public class TutorAchievementsService : ITutorAchievementsService
    {
        private readonly DataContext database;
        private readonly IMapper mapper;

        public TutorAchievementsService(DataContext database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public async Task<Result<bool>> Create(string tutorLogin, TutorAchievementsDto achievementsDto)
        {
            if (await database.TutorAchievements.FirstOrDefaultAsync(e => e.Name == achievementsDto.Name) != null)
                return Result.Fail<bool>("Достижение с таким названием уже существует");

            var (user, role) = await database.FindUserAsync(tutorLogin);
            var achievement = mapper.Map<TutorAchievements>(achievementsDto);
            achievement.Tutor = await database.Tutors.FirstOrDefaultAsync(e => e.Id == user.Id);
            await database.TutorAchievements.AddAsync(achievement);
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<List<TutorAchievementsDto>>> GetAchievements(string tutorLogin)
        {
            var (user, role) = await database.FindUserAsync(tutorLogin);
            if (user == null || role != "Tutor")
                return Result.Fail<List<TutorAchievementsDto>>("Такого преподавателя не существует");

            var achievements = await database.TutorAchievements
                .Where(e => e.Tutor.Login == tutorLogin)
                .Select(e => mapper.Map<TutorAchievementsDto>(e))
                .ToListAsync();
            return Result.Ok(achievements);
        }

        public async Task<Result<bool>> DeleteAchievement(string tutorLogin, int id)
        {
            var achievement = await database.TutorAchievements.Include(a => a.Tutor)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (achievement == null)
                return Result.Fail<bool>("Такого достижения не существует");
            if (achievement.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");

            database.TutorAchievements.Remove(achievement);
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> ChangeAchievement(string tutorLogin, TutorAchievementsDto achievementsDto)
        {
            var achievement = await database.TutorAchievements.Include(a => a.Tutor)
                .FirstOrDefaultAsync(e => e.Id == achievementsDto.Id);
            if (achievement == null)
                return Result.Fail<bool>("Такого достижения не существует");
            if (achievement.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");

            var newAchievement = mapper.Map<TutorAchievements>(achievementsDto);
            newAchievement.Tutor = achievement.Tutor;
            database.Entry(achievement)
                .CurrentValues.SetValues(newAchievement);
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }
    }
}
