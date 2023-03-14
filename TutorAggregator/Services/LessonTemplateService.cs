using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResultOfTask;
using TutorAggregator.Data;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;
using TutorAggregator.ServiceInterfaces;

namespace TutorAggregator.Services
{
    public class LessonTemplateService : ILessonTemplatesService
    {
        private DataContext _database;
        private IMapper _mapper;

        public LessonTemplateService(DataContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<Result<LessonTemplateDTO[]>> GetTemplates(string tutorLogin)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            if (tutor == null)
                return Result.Fail<LessonTemplateDTO[]>("Такого рептитора не существует");

            var templates = await _database.LessonTemplates
                .Where(e => e.Tutor == tutor)
                .Select(e => _mapper.Map<LessonTemplateDTO>(e))
                .ToArrayAsync();

            return Result.Ok(templates);
        }

        public async Task<Result<LessonTemplateDTO>> GetTemplate(int id)
        {
            var template = await _database.LessonTemplates.FirstOrDefaultAsync(e => e.Id == id);
            if (template == null)
                return Result.Fail<LessonTemplateDTO>("Такого шаблона не существует");

            return Result.Ok(_mapper.Map<LessonTemplateDTO>(template));
        }

        public async Task<Result<bool>> TryCreateTemplate(string tutorLogin, LessonTemplateDTO templateDto)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            if (tutor == null)
                return Result.Fail<bool>("Такого репетитора не существует");
            if (_database.LessonTemplates
                    .Any(e  => e.Tutor == tutor && e.Name == templateDto.Name))
                return Result.Fail<bool>("Шаблон с таким именем уже существует");

            var template = _mapper.Map<LessonTemplate>(templateDto);
            template.Tutor = tutor;
            await _database.LessonTemplates.AddAsync(template);
            await _database.SaveChangesAsync();

            return Result.Ok(true);
        }

        public async Task<Result<bool>> TryChangeTemplate(string tutorLogin, LessonTemplateDTO templateDto)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            var newTemplate = _mapper.Map<LessonTemplate>(templateDto);
            newTemplate.Tutor = tutor;
            var template = await _database.LessonTemplates
                .FirstOrDefaultAsync(e => e.Id == templateDto.Id && e.Tutor == tutor);
            if (template == null) 
                return Result.Fail<bool>("Такого шаблона не существует или у вас нет к нему доступа");

            _database.Entry(template)
                .CurrentValues.SetValues(newTemplate);
            await _database.SaveChangesAsync();

            return Result.Ok(true);
        }

        public async Task<Result<bool>> TryDeleteTemplate(string tutorLogin, int templateId)
        {
            var tutor = await _database.Tutors.FirstOrDefaultAsync(e => e.Login == tutorLogin);
            var template = await _database.LessonTemplates
                .FirstOrDefaultAsync(e => e.Id == templateId && e.Tutor == tutor);
            if (template == null) 
                return Result.Fail<bool>("Такого шаблона не существует или у вас нет к нему доступа");

            _database.Remove(template);
            await _database.SaveChangesAsync();

            return Result.Ok(true);
        }
    }
}
