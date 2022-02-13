using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Contracts
{
    public interface ISkillProvider
    {
        Task<List<SkillEntity>> GetAsync(string hashKey);

        Task<List<SkillEntity>> SearchAsync(string skill);

        Task<List<string>> SearchForKeyAsync(string skill);

    }
}
