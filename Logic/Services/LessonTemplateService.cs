using AutoMapper;
using DAL.Entities;
using Logic.Helpers;
using Logic.Interfaces;
using DAL.Interfaces;
using Logic.Models.LessoonTemplate;

namespace Logic.Services
{
    public class LessonTemplateService : ILessonTemplatesService
    {
        private readonly IMapper mapper;
        private readonly ILessonTemplatesRepository templatesRepository;
        private readonly ITutorRepository tutorRepository;

        public LessonTemplateService(IMapper mapper, ILessonTemplatesRepository templatesRepository, ITutorRepository tutorRepository)
        {
            this.mapper = mapper;
            this.templatesRepository = templatesRepository;
            this.tutorRepository = tutorRepository;
        }

        public async Task<Result<LessonTemplateOutDTO[]>> GetTemplatesAsync(string tutorLogin)
        {
            var templates = (await templatesRepository.GetAllByTutorAsync(tutorLogin))
                .Select(mapper.Map<LessonTemplateOutDTO>)
                .ToArray();

            return Result.Ok(templates);
        }

        public async Task<Result<LessonTemplateOutDTO>> GetTemplateAsync(int id)
        {
            var template = await templatesRepository.GetByIdAsync(id);
            if (template == null)
                return Result.Fail<LessonTemplateOutDTO>("Такого шаблона не существует");

            return Result.Ok(mapper.Map<LessonTemplateOutDTO>(template));
        }

        public async Task<Result<(int id, bool isInserted)>> InsertOrUpdateTemplateAsync(string tutorLogin, LessonTemplateAddDTO templateAddDto)
        {
            var tutor = await tutorRepository.GetByLoginAsync(tutorLogin);
            if (tutor == null)
                return Result.Fail<(int id, bool isInserted)>("Такого репетитора не существует");

            var existed = await templatesRepository.GetByIdAsync(templateAddDto.Id);
            if (existed == null)
            {
                var template = mapper.Map<LessonTemplate>(templateAddDto);
                template.Tutor = tutor;
                await templatesRepository.AddAsync(template);
                await templatesRepository.SaveChangesAsync();
                return Result.Ok((template.Id, true));
            }
            
            if (existed.Tutor.Login != tutorLogin)
                return Result.Fail<(int id, bool isInserted)>("У вас нет доступа к шаблону");

            mapper.Map(templateAddDto, existed);
            await templatesRepository.SaveChangesAsync();
            return Result.Ok((-1, false));
        }

        public async Task<Result<bool>> DeleteTemplateAsync(string tutorLogin, int templateId)
        {
            var template = await templatesRepository.GetByIdAsync(templateId);

            if (template.Tutor.Login == tutorLogin)
            {
                templatesRepository.Delete(template);
                await templatesRepository.SaveChangesAsync();
            }

            return Result.Ok(true);
        }
    }
}
