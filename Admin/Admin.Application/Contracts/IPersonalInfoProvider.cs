using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Contracts
{
    public interface IPersonalInfoProvider
    {
        Task<List<PersonalInfoEntity>> SearchByNameAsync(string name);

        Task<List<string>> GetEmployeeIdsByname(string name);

        Task<PersonalInfoEntity> SearchByEmpIdAsync(string empId);
    }
}
