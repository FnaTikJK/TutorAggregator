using AutoMapper;
using DAL.Entities;
using Logic.Models;

namespace Logic.Services.Helpers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Student, ProfileDTO>().ReverseMap();
            CreateMap<Tutor, ProfileDTO>().ReverseMap();
        }
    }
}
