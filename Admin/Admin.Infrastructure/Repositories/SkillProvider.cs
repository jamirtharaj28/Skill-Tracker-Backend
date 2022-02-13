using Admin.Application.Contracts;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Infrastructure.Repositories
{
    public class SkillProvider : ISkillProvider
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _dbcontext;
        private readonly string _tableName = "st-skill";

        public SkillProvider(AmazonDynamoDBClient client, DynamoDBContext dbcontext)
        {
            _client = client;
            _dbcontext = dbcontext;
        }

        public async Task<List<SkillEntity>> GetAsync(string hashKey)
        {
            return await _dbcontext.QueryAsync<SkillEntity>(hashKey).GetNextSetAsync();
        }


        public async Task<List<SkillEntity>> SearchAsync(string skill)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "NameIndex",
                KeyConditionExpression = "#nm = :v_name and proficiency > 10",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#nm", "name"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_name", new AttributeValue { S =  skill }}
                },
                ScanIndexForward = true
            };

            var result = await _client.QueryAsync(queryRequest);

            var reponse = new List<SkillEntity>();

            foreach (var item in result.Items)
            {
                reponse.Add(new SkillEntity
                {
                    EmpId = item["empId"].S,
                    SkillId = int.Parse(item["skillId"].N),
                    IsTechnical = item["technical"].BOOL,
                    Name = item["name"].S,
                    Proficiency = int.Parse(item["proficiency"].N),
                    // CreatedBy = item["createdBy"].S,
                    CreatedDate = DateTime.Parse(item["createdDate"].S),
                    // LastModifiedBy = item["modifiedBy"].S,
                    LastModifiedDate = DateTime.Parse(item["modifiedDate"].S)
                });
            }

            return reponse;
        }

        public async Task<List<string>> SearchForKeyAsync(string skill)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "NameIndex",
                KeyConditionExpression = "#nm = :v_name and proficiency >= :v_prof",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#nm", "name"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_name", new AttributeValue { S =  skill }},
                    {":v_prof", new AttributeValue { N =  "1" }}
                },
                ScanIndexForward = true,
                ProjectionExpression = "empId"
            };

            var result = await _client.QueryAsync(queryRequest);

            var reponse = new List<string>();
            return result.Items.Select(item => item["empId"].S).ToList();
        }
    }
}
