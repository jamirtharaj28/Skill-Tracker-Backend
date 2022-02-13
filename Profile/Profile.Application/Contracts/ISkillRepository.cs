using SkillTracker.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Profile.Application.Contracts
{
    public interface ISkillRepository : IAsyncRepository<SkillEntity>
    {
        Task<List<SkillEntity>> GetItems(string hadhKey);
    }
}
