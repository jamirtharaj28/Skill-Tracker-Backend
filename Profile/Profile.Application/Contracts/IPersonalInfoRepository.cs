using SkillTracker.Entities;

namespace Profile.Application.Contracts
{
    public interface IPersonalInfoRepository: IAsyncRepository<PersonalInfoEntity>
    {
    }
}
