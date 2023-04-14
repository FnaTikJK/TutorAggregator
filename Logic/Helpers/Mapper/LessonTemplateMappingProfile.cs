using AutoMapper;
using DAL.Entities;
using Logic.Models.LessoonTemplate;

namespace Logic.Helpers.Mapper
{
    public class LessonTemplateMappingProfile : Profile
    {
        public LessonTemplateMappingProfile()
        {
            CreateMap<LessonTemplate, LessonTemplate>();
            CreateMap<LessonTemplate, LessonTemplateOutDTO>()
                .ForMember(dest => dest.TutorLogin, opt => opt.MapFrom(src => src.Tutor.Login));
            CreateMap<LessonTemplateAddDTO, LessonTemplate>();
        }
    }
}
