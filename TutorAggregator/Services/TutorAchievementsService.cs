using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class TutorAchievementsService : ITutorAchievementsService
    {
        private readonly DataContext _database;
        private readonly IMapper _mapper;

        public TutorAchievementsService(DataContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Create(string tutorLogin, TutorAchievementsDto achievementsDto)
        {
            if (await _database.TutorAchievements.FirstOrDefaultAsync(e => e.Name == achievementsDto.Name) != null)
                return Result.Fail<bool>("Достижение с таким названием уже существует");

            var (user, role) = await _database.FindUserAsync(tutorLogin);
            var achievement = _mapper.Map<TutorAchievements>(achievementsDto);
            achievement.Tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Id == user.Id);
            await _database.TutorAchievements.AddAsync(achievement);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<List<TutorAchievementsDto>>> GetAchievements(string tutorLogin)
        {
            var (user, role) = await _database.FindUserAsync(tutorLogin);
            if (user == null || role != "Tutor")
                return Result.Fail<List<TutorAchievementsDto>>("Такого преподавателя не существует");

            var achievements = await _database.TutorAchievements
                .Where(e => e.Tutor.Login == tutorLogin)
                .Select(e => _mapper.Map<TutorAchievementsDto>(e))
                .ToListAsync();
            return Result.Ok(achievements);
        }

        public async Task<Result<bool>> DeleteAchievement(string tutorLogin, int id)
        {
            var achievement = await _database.TutorAchievements.Include(a => a.Tutor)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (achievement == null)
                return Result.Fail<bool>("Такого достижения не существует");
            if (achievement.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");

            _database.TutorAchievements.Remove(achievement);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> ChangeAchievement(string tutorLogin, TutorAchievementsDto achievementsDto)
        {
            var achievement = await _database.TutorAchievements.Include(a => a.Tutor)
                .FirstOrDefaultAsync(e => e.Id == achievementsDto.Id);
            if (achievement == null)
                return Result.Fail<bool>("Такого достижения не существует");
            if (achievement.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");

            var newAchievement = _mapper.Map<TutorAchievements>(achievementsDto);
            newAchievement.Tutor = achievement.Tutor;
            _database.Entry(achievement)
                .CurrentValues.SetValues(newAchievement);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }
    }
}
