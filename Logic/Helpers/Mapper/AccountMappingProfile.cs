using AutoMapper;
using DAL.Entities;
using Logic.Models.Account;
using Logic.Services.Helpers;

namespace Logic.Helpers.Mapper
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountRegDTO, Student>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.ConvertUsing<AccountConverter, string>(src => src.Password));
            CreateMap<AccountRegDTO, Tutor>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.ConvertUsing<AccountConverter, string>(src => src.Password));
        }

        public class AccountConverter : IValueConverter<string, string>
        {
            public string Convert(string sourceMember, ResolutionContext context)
                => PasswordHasher.ComputeHash(sourceMember);
        }
    }
}
