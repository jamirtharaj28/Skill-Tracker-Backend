using Profile.Application.Models;
using Profile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Application.Contracts
{
    public interface IProfileRepository : IAsyncRepository<ProfileEntity>
    {
        Task PutItem(ProfileEntity entity);
        Task<ProfileEntity> GetItem(string hadhKey);
    }
}
