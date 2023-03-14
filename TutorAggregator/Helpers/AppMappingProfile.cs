using AutoMapper;
using TutorAggregator.DataEntities;
using TutorAggregator.Models;

namespace TutorAggregator.Helpers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Student, ProfileDTO >().ReverseMap();
            CreateMap<Tutor, ProfileDTO>().ReverseMap();
        }
    }
}
