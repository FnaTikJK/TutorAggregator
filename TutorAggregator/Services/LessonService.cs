using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class LessonService : ILessonService
    {
        private readonly DataContext _database;
        private readonly IMapper _mapper;

        public LessonService(DataContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Result<bool>> TryCreateLesson(string tutorLogin, LessonDTO lessonDto)
        {
            var validRes = await ValidateDto(tutorLogin, lessonDto);
            if (!validRes.IsSuccess) 
                return validRes;

            var lesson = _mapper.Map<Lesson>(lessonDto);
            lesson.AllowedTemplates = _database.LessonTemplates
                .Where(e => lessonDto.AllowedTemplatesId.Contains(e.Id))
                .ToList();
            lesson.Student = await _database.Students.FirstOrDefaultAsync(e => e.Id == lessonDto.StudentId);
            
            await _database.Lessons.AddAsync(lesson);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<List<LessonDTO>>> GetLessons(string userLogin)
        {
            var (user, role) = await _database.FindUserAsync(userLogin);
            if (user == null)
                return Result.Fail<List<LessonDTO>>("Такого пользователя не существует");

            var lessons = _database.Lessons
                .Include(l => l.AllowedTemplates)
                .Include(l => l.Student)
                .Where(e => role == "Student" && e.Student.Id == user.Id
                            || role == "Tutor" && e.AllowedTemplates.First().Tutor.Id == user.Id)
                .Select(e => _mapper.Map<Lesson, LessonDTO>(e))
                .ToList();

            return Result.Ok(lessons);
        }

        public async Task<Result<bool>> Delete(string tutorLogin, int lessonId)
        {
            var lessonToDel = await _database.Lessons
                .Include(l => l.AllowedTemplates)
                .FirstOrDefaultAsync(e => e.Id == lessonId);
            if (lessonToDel == null)
                return Result.Fail<bool>("Такого урока не существует");
            if ((await _database.LessonTemplates.Include(t => t.Tutor)
                    .FirstOrDefaultAsync(e => e.Id == lessonToDel.AllowedTemplates[0].Id)).Tutor.Login != tutorLogin)
                return Result.Fail<bool>("У вас нет доступа к уроку");

            _database.Lessons.Remove(lessonToDel);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> SubscribeStudent(string studentLogin, int lessonId, int chosenTemplateId)
        {
            var (user, role) = await _database.FindUserAsync(studentLogin);
            var lesson = await _database.Lessons.FirstOrDefaultAsync(e => e.Id == lessonId);
            if (lesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (lesson.Student != null)
                return Result.Fail<bool>("Урок занят");

            var studentLessons = await GetLessons(studentLogin);
            if (studentLessons.Value.Any(e => e.DateTime > lesson.DateTime
                                            && e.DateTime.AddMinutes(e.LengthInMinutes) <
                                                lesson.DateTime.AddMinutes(lesson.LengthInMinutes)
                                        || lesson.DateTime > e.DateTime
                                            && lesson.DateTime.AddMinutes(lesson.LengthInMinutes) <
                                                e.DateTime.AddMinutes(e.LengthInMinutes)))
                return Result.Fail<bool>("Уроки пересекаются");

            lesson.Student = await _database.Students.FirstOrDefaultAsync(e => e.Id == user.Id);
            lesson.ChosenTemplateId = chosenTemplateId;
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> UnsubscribeStudent(string userLogin, int lessonId)
        {
            var (user, role) = await _database.FindUserAsync(userLogin);
            var lesson = await _database.Lessons
                .Include(l => l.AllowedTemplates)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(e => e.Id == lessonId);

            if (lesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (role == "Tutor" && lesson.AllowedTemplates.First().Tutor.Login != userLogin
                || role == "Student" && lesson.Student?.Login != userLogin)
                return Result.Fail<bool>("У вас нет доступа");

            lesson.Student = null;
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        public async Task<Result<bool>> ChangeLesson(string tutorLogin, LessonDTO lessonDto)
        {
            var existedLesson = await _database.Lessons
                .Include(l => l.AllowedTemplates)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(e => e.Id == lessonDto.Id);

            if (existedLesson == null)
                return Result.Fail<bool>("Такого урока не существует");
            if (await _database.LessonTemplates
                    .FirstOrDefaultAsync(e => e.Id == existedLesson.AllowedTemplates.First().Id) == null)
                return Result.Fail<bool>("У вас нет доступа к уроку");
            var validRes = await ValidateDto(tutorLogin, lessonDto);
            if (!validRes.IsSuccess)
                return validRes;

            var newLesson = _mapper.Map<Lesson>(lessonDto);
            newLesson.Student = existedLesson.Student;
            newLesson.AllowedTemplates = lessonDto.AllowedTemplatesId
                .Select(e => _database.LessonTemplates.FirstOrDefault(t => t.Id == e))
                .ToList();

            _database.Entry(existedLesson)
                .CurrentValues.SetValues(newLesson);
            await _database.SaveChangesAsync();
            return Result.Ok(true);
        }

        private async Task<Result<bool>> ValidateDto(string tutorLogin, LessonDTO lessonDto)
        {
            if (lessonDto.AllowedTemplatesId.Count == 0 || lessonDto.AllowedTemplatesId.Any(t =>
                    _database.LessonTemplates.FirstOrDefault(e => e.Id == t && e.Tutor.Login == tutorLogin) == null))
                return Result.Fail<bool>(
                    "Некорректные разрешённые шаблоны(их не существует или у вас нет к ним доступа)");
            if (lessonDto.StudentId != null && lessonDto.StudentId > 0
                && await _database.Students.FirstOrDefaultAsync(s => s.Id == lessonDto.StudentId) == null)
                return Result.Fail<bool>("Такого ученика не существует");
            if (lessonDto.ChosenTemplateId != null
                && lessonDto.ChosenTemplateId > 0
                && !lessonDto.AllowedTemplatesId.Contains((int)lessonDto.ChosenTemplateId))
                return Result.Fail<bool>("Нельзя выбрать неразрешённый шаблон");
            if (await _database.Lessons.FirstOrDefaultAsync(e =>
                    e.AllowedTemplates.First().Tutor.Login == tutorLogin
                    && (e.DateTime > lessonDto.DateTime && e.DateTime < lessonDto.DateTime.AddMinutes(lessonDto.LengthInMinutes)
                        || lessonDto.DateTime > e.DateTime && lessonDto.DateTime < e.DateTime.AddMinutes(e.LengthInMinutes))) != null)
                return Result.Fail<bool>("Уроки пересекаются");

            return Result.Ok(true);
        }
    }
}
