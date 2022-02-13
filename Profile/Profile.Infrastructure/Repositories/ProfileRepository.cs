using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Profile.Application.Contracts;
using Profile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _dbcontext;
        private readonly string tableName = "st-profile_repository";

        public ProfileRepository(DynamoDBContext dbcontext, AmazonDynamoDBClient client)
        {
            this._dbcontext = dbcontext;
            this._client = client;
        }

        public Task<ProfileEntity> AddAsync(ProfileEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProfileEntity>> AddRangeAsync(IList<ProfileEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object hashKey, object rangeKey)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileEntity> GetItem(string hadhKey)
        {
            throw new NotImplementedException();
        }

        public async Task<ProfileEntity> GetTime(string hadhKey)
        {
            var table = Table.LoadTable(_client, tableName);
            var doc = await table.GetItemAsync(hadhKey);
            return _dbcontext.FromDocument<ProfileEntity>(doc);
        }

        public async Task PutItem(ProfileEntity entity)
        {
            var table = Table.LoadTable(_client, tableName);
            var doc = _dbcontext.ToDocument(entity);
            await table.PutItemAsync(doc);
        }

    }
}
