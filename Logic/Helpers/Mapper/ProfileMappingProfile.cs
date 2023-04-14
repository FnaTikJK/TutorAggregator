using AutoMapper;
using DAL.Entities;
using Logic.Models;

namespace Logic.Helpers.Mapper
{
    public class ProfileMappingProfile : Profile
    {
        public ProfileMappingProfile()
        {
            CreateMap<Student, ProfileDTO>().ReverseMap();
            CreateMap<Tutor, ProfileDTO>().ReverseMap();
        }
    }
}
