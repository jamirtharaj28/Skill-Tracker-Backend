using Admin.Application.Features.Commands.CacheProfile;
using Admin.Domain.Models;
using SkillTracker.Entities;

namespace Admin.Application.Mappings
{
    public class MappingProfile: AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<CacheProfileCommand, Profile>().ReverseMap();
            CreateMap<PersonalInfoEntity, Profile>().ReverseMap();
            CreateMap<Skill, SkillEntity>().ReverseMap();
        }
    }
}
