using AutoMapper;
using DAL.Entities;
using Logic.Models;

namespace Logic.Helpers.Mapper
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
