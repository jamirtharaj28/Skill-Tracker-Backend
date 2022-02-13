using Amazon.DynamoDBv2.DataModel;
using Profile.Application.Contracts;
using Profile.Domain.Entities;
using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Infrastructure.Repositories
{
    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        private readonly DynamoDBContext dbcontext;

        public PersonalInfoRepository(DynamoDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<PersonalInfoEntity> AddAsync(PersonalInfoEntity entity)
        {
            await this.dbcontext.SaveAsync<PersonalInfoEntity>(entity);
            return entity;
        }

        public Task<IList<PersonalInfoEntity>> AddRangeAsync(IList<PersonalInfoEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object hashKey, object rangeKey)
        {
            throw new NotImplementedException();
        }

    }
}
