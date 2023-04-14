using AutoMapper;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Logic.Helpers;
using Logic.Interfaces;
using DAL.Core;
using DAL.Interfaces;
using DAL.Repositories;
using Logic.Models.Lesson;
using Logic.Models.Tutor;

namespace Logic.Services
{
    public class LessonService : ILessonService
    {
        private readonly DataContext database;
        private readonly IMapper mapper;
        private readonly ILessonRepository lessonsRepository;

        public LessonService(DataContext database, IMapper mapper, ILessonRepository lessonRepository)
        {
            this.database = database;
            this.mapper = mapper;
            this.lessonsRepository = lessonRepository;
        }

        public async Task<Result<bool>> AddAsync(string tutorLogin, LessonCreateOrUpdateDTO lessonCreateOrUpdateDto)
        {
            var lesson = mapper.Map<Lesson>(lessonCreateOrUpdateDto);
            lesson.Student = null;
            lesson.AllowedTemplates = database.LessonTemplates
                .Where(e => lessonCreateOrUpdateDto.AllowedTemplatesId.Contains(e.Id))
                .ToList();

            var validRes = await ValidateDto(tutorLogin, lessonCreateOrUpdateDto);
            if (!validRes.IsSuccess)
                return validRes;

            await lessonsRepository.AddAsync(lesson);
            await lessonsRepository.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<List<LessonOutDTO>>> GetAllByLoginAsync(string userLogin)
        {
            var (user, role) = await database.FindUserAsync(userLogin);
            if (user == null)
                return Result.Fail<List<LessonOutDTO>>("Такого пользователя не существует");

            var lessons = database.Lessons
                .Include(l => l.AllowedTemplates)
                    .ThenInclude(t => t.Tutor)
                .Include(l => l.Student)
                .Where(e => role == "Student" && e.Student.Id == user.Id
                            || role == "Tutor" && e.AllowedTemplates.First().Tutor.Id == user.Id);
            var tutorDto = mapper.Map<TutorOutDTO>(lessons.First().AllowedTemplates.First().Tutor);
            var lessonOutDto = lessons.Select(e => mapper.Map<Lesson, LessonOutDTO>(e))
                .ToList();
            lessonOutDto.ForEach(e => e.Tutor = tutorDto);

            return Result.Ok(lessonOutDto);
        }

        public async Task<Result<LessonOutDTO>> GetByIdAsync(int id)
        {
            return new Result<LessonOutDTO>(null);
        }

        public async Task<Result<bool>> DeleteAsync(string tutorLogin, int lessonId)
        {
            var lessonToDel = await database.Lessons
                .Include(l => l.AllowedTemplates)
                .FirstOrDefaultAsync(e => e.Id == lessonId);
            if (lessonToDel == null)
                return Result.Fail<bool>("Такого урока не существует");
            if ((await database.LessonTemplates.Include(t => t.Tutor)
                    .FirstOrDefaultAsync(e => e.Id == lessonToDel.AllowedTemplates[0].Id)).Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа к уроку");

            database.Lessons.Remove(lessonToDel);
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> SubscribeStudent(string studentLogin, int lessonId, int chosenTemplateId)
        {
            var (user, role) = await database.FindUserAsync(studentLogin);
            var lesson = await database.Lessons.FirstOrDefaultAsync(e => e.Id == lessonId);
            if (lesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (lesson.Student != null)
                return Result.Fail<bool>("Урок занят");

            var studentLessons = await GetAllByLoginAsync(studentLogin);
            if (studentLessons.Value.Any(e => e.Start.ToTimeSpan() > lesson.Start
                                            && e.Start.ToTimeSpan() < lesson.End
                                            || lesson.Start > e.Start.ToTimeSpan()
                                            && lesson.Start < e.End.ToTimeSpan()))
                return Result.Fail<bool>("Уроки пересекаются");

            lesson.Student = await database.Students.FirstOrDefaultAsync(e => e.Id == user.Id);
            lesson.ChosenTemplateId = chosenTemplateId;
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> UnsubscribeStudent(string userLogin, int lessonId)
        {
            var (user, role) = await database.FindUserAsync(userLogin);
            var lesson = await database.Lessons
                .Include(l => l.AllowedTemplates)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(e => e.Id == lessonId);

            if (lesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (role == "Tutor" && lesson.AllowedTemplates.First().Tutor.Login != userLogin
                || role == "Student" && lesson.Student?.Login != userLogin)
                return Result.Fail<bool>("У вас нет доступа");

            lesson.Student = null;
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> ChangeLesson(string tutorLogin, LessonCreateOrUpdateDTO lessonCreateOrUpdateDto)
        {
            var existedLesson = await database.Lessons
                .Include(l => l.AllowedTemplates)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(e => e.Id == lessonCreateOrUpdateDto.Id);

            if (existedLesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (await database.LessonTemplates
                    .FirstOrDefaultAsync(e => e.Id == existedLesson.AllowedTemplates.First().Id) == null)
                return Result.Fail<bool>("У вас нет доступа к уроку");
            var validRes = await ValidateDto(tutorLogin, lessonCreateOrUpdateDto);
            if (!validRes.IsSuccess)
                return validRes;

            var newLesson = mapper.Map<Lesson>(lessonCreateOrUpdateDto);
            newLesson.Student = existedLesson.Student;
            newLesson.AllowedTemplates = lessonCreateOrUpdateDto.AllowedTemplatesId
                .Select(e => database.LessonTemplates.FirstOrDefault(t => t.Id == e))
                .ToList();

            database.Entry(existedLesson)
                .CurrentValues.SetValues(newLesson);
            await database.SaveChangesAsync();
            return Result.Ok(true);
        }

        private async Task<Result<bool>> ValidateDto(string tutorLogin, LessonCreateOrUpdateDTO lessonCreateOrUpdateDto)
        {
            if (lessonCreateOrUpdateDto.Start >= lessonCreateOrUpdateDto.End)
                return Result.Fail<bool>("Некорректное время");
            if (lessonCreateOrUpdateDto.AllowedTemplatesId.Count == 0 || lessonCreateOrUpdateDto.AllowedTemplatesId.Any(t =>
                    database.LessonTemplates.FirstOrDefault(e => e.Id == t && e.Tutor.Login == tutorLogin) == null))
                return Result.Fail<bool>(
                    "Некорректные разрешённые шаблоны(их не существует или у вас нет к ним доступа)");
            //if (lessonCreateOrUpdateDto.StudentLogin != null && lessonCreateOrUpdateDto.StudentLogin != null
            //    && await database.Students.FirstOrDefaultAsync(s => s.Login == lessonCreateOrUpdateDto.StudentLogin) == null)
            //    return Result.Fail<bool>("Такого ученика не существует");
            if (lessonCreateOrUpdateDto.ChosenTemplateId != null
                && lessonCreateOrUpdateDto.ChosenTemplateId > 0
                && !lessonCreateOrUpdateDto.AllowedTemplatesId.Contains((int)lessonCreateOrUpdateDto.ChosenTemplateId))
                return Result.Fail<bool>("Нельзя выбрать неразрешённый шаблон");
            if (await database.Lessons.FirstOrDefaultAsync(e =>
                    e.AllowedTemplates.First().Tutor.Login == tutorLogin
                    && e.Start > lessonCreateOrUpdateDto.Start.ToTimeSpan() && e.End < lessonCreateOrUpdateDto.End.ToTimeSpan()) != null)
                return Result.Fail<bool>("Уроки пересекаются");

            return Result.Ok(true);
        }
    }
}
