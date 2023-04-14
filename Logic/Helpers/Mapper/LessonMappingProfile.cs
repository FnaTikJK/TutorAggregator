using AutoMapper;
using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;
using Logic.Helpers.Mapper.Config;
using Logic.Models.Lesson;
using Logic.Models.Student;
using Logic.Models.Tutor;

namespace Logic.Helpers.Mapper
{
    public class LessonMappingProfile : Profile
    {
        ITutorRepository tutorRepository;

        public LessonMappingProfile()
        {
            CreateMap<LessonCreateOrUpdateDTO, Lesson>()
                .ForMember(dest => dest.Start, opt => opt.ConvertUsing<TimeOnlyConverter, TimeOnly>())
                .ForMember(dest => dest.End, opt => opt.ConvertUsing<TimeOnlyConverter, TimeOnly>())
                .ForMember(dest => dest.AllowedTemplates,
                    opt => opt.ConvertUsing<LessonTemplateConverter, List<int>>(src => src.AllowedTemplatesId));
            CreateMap<Lesson, LessonOutDTO>()
                .ForMember(dest => dest.Start, opt => opt.ConvertUsing<TimeOnlyConverter, TimeSpan>())
                .ForMember(dest => dest.End, opt => opt.ConvertUsing<TimeOnlyConverter, TimeSpan>());
            CreateMap<Student, StudentOutDTO>();
            CreateMap<Tutor, TutorOutDTO>();
        }
    }

}