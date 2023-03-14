using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class TutorWorksService : ITutorWorksService
    {
        private readonly DataContext _database;
        private readonly IMapper _mapper;

        public TutorWorksService(DataContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Create(string tutorLogin, TutorWorkDto workDto)
        {
            var validation = await ValidateDto(tutorLogin, workDto);
            if (!validation.IsSuccess)
                return validation;
            //if ((await GetWorks(tutorLogin)).Value.Any(e =>
            //        e.Start > workDto.Start && e.Start < workDto.End || e.End > workDto.Start && e.End < workDto.End
            //        || workDto.Start > e.Start && workDto.Start < e.End || workDto.End > e.Start && workDto.End < e.End))
            //    return Result.Fail<bool>("Работы пересекаются");
            var work = _mapper.Map<TutorWork>(workDto);
            work.Tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);

            await _database.TutorWorks.AddAsync(work);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<List<TutorWorkDto>>> GetWorks(string tutorLogin)
        {
            var (user, role) = await _database.FindUserAsync(tutorLogin);
            if (user == null)
                return Result.Fail<List<TutorWorkDto>>("Такого пользователя не существует");
            if (role != "Tutor")
                return Result.Fail<List<TutorWorkDto>>("Такого преподавателя не существует");

            var works = await _database.TutorWorks
                .Where(e => e.Tutor.Login == tutorLogin)
                .Select(e => _mapper.Map<TutorWorkDto>(e))
                .ToListAsync();
            return Result.Ok(works);
        }

        public async Task<Result<bool>> DeleteWork(string tutorLogin, int id)
        {
            var work = await _database.TutorWorks.Include(w => w.Tutor)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (work == null)
                return Result.Fail<bool>("Такой записи нет");
            if (work.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");

            _database.TutorWorks.Remove(work);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> ChangeWork(string tutorLogin, TutorWorkDto workDto)
        {
            var work = await _database.TutorWorks.Include(w => w.Tutor)
                .FirstOrDefaultAsync(e => e.Id == workDto.Id);
            if (work == null)
                return Result.Fail<bool>("Такой записи не существует");
            if (work.Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа");
            if ((await GetWorks(tutorLogin)).Value.Any(e => e.Id != workDto.Id &&
                    (e.Start > workDto.Start && e.Start < workDto.End || e.End > workDto.Start && e.End < workDto.End
                    || workDto.Start > e.Start && workDto.Start < e.End || workDto.End > e.Start && workDto.End < e.End)))
                return Result.Fail<bool>("Работы пересекаются");

            var newWork = _mapper.Map<TutorWork>(workDto);
            newWork.Tutor = work.Tutor;
            _database.Entry(work)
                .CurrentValues.SetValues(newWork);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        private async Task<Result<bool>> ValidateDto(string tutorLogin, TutorWorkDto workDto)
        {
            if (workDto.Start >= workDto.End)
                return Result.Fail<bool>("Некорректное время урока");
            if ((await GetWorks(tutorLogin)).Value.Any(e => e.Id != workDto.Id &&
                                                            (e.Start > workDto.Start && e.Start < workDto.End || e.End > workDto.Start && e.End < workDto.End
                                                                || workDto.Start > e.Start && workDto.Start < e.End || workDto.End > e.Start && workDto.End < e.End)))
                return Result.Fail<bool>("Работы пересекаются");

            return Result.Ok(true);
        } 
    }
}
