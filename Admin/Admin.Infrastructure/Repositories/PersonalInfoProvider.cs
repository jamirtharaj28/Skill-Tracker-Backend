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
    public class PersonalInfoProvider : IPersonalInfoProvider
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _dbcontext;
        private readonly string _tableName = "st-personal_info";

        public PersonalInfoProvider(AmazonDynamoDBClient client, DynamoDBContext dbcontext)
        {
            _client = client;
            _dbcontext = dbcontext;
        }

        public async Task<List<string>> GetEmployeeIdsByname(string name)
        {  
            Condition titleCondition = new Condition
            {
                ComparisonOperator = "BEGINS_WITH"
            };

            titleCondition.AttributeValueList.Add(new AttributeValue { S = name });

            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>
            {
                ["name"] = titleCondition
            };

            var scanRequest = new ScanRequest
            {
                TableName = _tableName,
                ScanFilter = conditions,
            };

            var result = await _client.ScanAsync(scanRequest);

            return result.Items.Select(item => item["empId"].S).ToList();
        }

        public async Task<PersonalInfoEntity> SearchByEmpIdAsync(string empId)
        {
            return (await _dbcontext.QueryAsync<PersonalInfoEntity>(empId).GetNextSetAsync()).FirstOrDefault();
        }

        public async Task<List<PersonalInfoEntity>> SearchByNameAsync(string name)
        {
            /*
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "NameIndex",
                KeyConditionExpression = "#nm begins_with :v_name",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#nm", "name"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_name", new AttributeValue { S =  name }}
                },
                ScanIndexForward = true
            };

            var result = await _client.QueryAsync(queryRequest);
            */

            // Define scan conditions
            Condition titleCondition = new Condition
            {
                ComparisonOperator = "BEGINS_WITH"
            };
            titleCondition.AttributeValueList.Add(new AttributeValue { S = name });

            Dictionary<string, Condition> conditions = new Dictionary<string, Condition>
            {
                ["name"] = titleCondition
            };

            var scanRequest = new ScanRequest
            {
                TableName = _tableName,
                ScanFilter = conditions
            };

            //var scanRequest = new ScanRequest
            //{
            //    TableName = _tableName,
            //    FilterExpression = "#nm begins_with :v_name",
            //    ExpressionAttributeNames = new Dictionary<String, String> {
            //        {"#nm", "name"}
            //    },
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
            //        {":v_name", new AttributeValue { S =  name }}
            //    }
            //};

            var result = await _client.ScanAsync(scanRequest);
            
            var reponse = new List<PersonalInfoEntity>();

            foreach (var item in result.Items)
            {
                reponse.Add(new PersonalInfoEntity
                {
                    EmpId = item["empId"].S,
                    Email = item["email"].S,
                    Mobile = item["mobile"].S,
                    Name = item["name"].S,
                    UserId = item["userId"].S,
                    // CreatedBy = item["createdBy"].S,
                    CreatedDate = DateTime.Parse(item["createdDate"].S),
                    // LastModifiedBy = item["modifiedBy"].S,
                    LastModifiedDate = DateTime.Parse(item["modifiedDate"].S)
                });
            }

            return reponse;
        }
    }
}
