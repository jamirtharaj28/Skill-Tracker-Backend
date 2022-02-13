
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Infrastructure
{
    public static class HostExtension
    {
        public static async Task SetupDatabase(IServiceProvider serviceProvider)
        {
            try
            {
                var client = serviceProvider.GetService(typeof(AmazonDynamoDBClient)) as AmazonDynamoDBClient;
                var tablesDontExist = await CheckTablesExist(client, new List<string>() { "st-personal_info", "st-skill" }); //, "st-profile_repository"
                if (tablesDontExist.Contains("st-personal_info"))
                {
                    await CreatePersonalInfoTable(client);
                }
                if (tablesDontExist.Contains("st-skill"))
                {
                    await CreateSkillTable(client);
                }
                //if (tablesDontExist.Contains("st-profile_repository"))
                //{
                //    await CreateTable(client, "st-profile_repository", "EmpId", "S");
                //}
            }
            catch(Exception ex)
            {
                var logger = serviceProvider.GetService(typeof(ILogger)) as ILogger;
                logger.LogError("Dynamo DB creation failed :" + ex.ToString());
            }
        }

        private static async Task<List<string>> CheckTablesExist(AmazonDynamoDBClient client, List<string> tables)
        {
            // Initial value for the first page of table names.
            string lastEvaluatedTableName = null;
            do
            {
                // Create a request object to specify optional parameters.
                var request = new ListTablesRequest
                {
                    Limit = 10, // Page size.
                    ExclusiveStartTableName = lastEvaluatedTableName
                };

                var response = await client.ListTablesAsync(request);
                foreach (string name in response.TableNames)
                {
                    if (tables.Contains(name))
                    {
                        tables.Remove(name);
                    }
                }

                lastEvaluatedTableName = response.LastEvaluatedTableName;

            } while (lastEvaluatedTableName != null);

            return tables;
        }

        private static async Task CreateTable (AmazonDynamoDBClient client, string tableName, string hashkey, string hashKeyType, string rangekey = null, string rangekeyType=null)
        {
 
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                  {
                    new AttributeDefinition
                    {
                      AttributeName = hashkey,
                      AttributeType = hashKeyType
                    }
                  },
                KeySchema = new List<KeySchemaElement>()
                  {
                    new KeySchemaElement
                    {
                      AttributeName = hashkey,
                      KeyType = "HASH"  //Partition key
                    }
                  },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };

            if (!string.IsNullOrWhiteSpace(rangekey) &&
                !string.IsNullOrWhiteSpace(rangekeyType))
            {
                request.AttributeDefinitions.Add(
                    new AttributeDefinition
                    {
                        AttributeName = rangekey,
                        AttributeType = rangekeyType
                    });
                request.KeySchema.Add(
                    new KeySchemaElement
                    {
                        AttributeName = rangekey,
                        KeyType = "RANGE"  //Partition key
                    });
            }

            await client.CreateTableAsync(request);
        }

        private static async Task CreatePersonalInfoTable(AmazonDynamoDBClient client)
        {
            // Attribute definitions
            var attributeDefinitions = new List<AttributeDefinition>()
            {
                new AttributeDefinition
                {
                    AttributeName = "empId",
                    AttributeType = "S"
                },
                new AttributeDefinition
                {
                    AttributeName = "name",
                    AttributeType = "S"
                }
            };

            // Key schema for table
            var tableKeySchema = new List<KeySchemaElement>()
            {
                new KeySchemaElement
                {
                    AttributeName = "empId",
                    KeyType = "HASH"  //Partition key
                },
                new KeySchemaElement
                {
                    AttributeName = "name",
                    KeyType = "RANGE"  //Sort key
                }
            };

            //// Initial provisioned throughput settings for the indexes
            //ProvisionedThroughput ptIndex = new ProvisionedThroughput
            //{
            //    ReadCapacityUnits = 1L,
            //    WriteCapacityUnits = 1L
            //};

            //// NameIndex
            //var NameIndex = new GlobalSecondaryIndex()
            //{
            //    IndexName = "NameIndex",
            //    ProvisionedThroughput = ptIndex,
            //    KeySchema = {
            //        new KeySchemaElement {
            //            AttributeName = "name", KeyType = "HASH" //Partition key
            //        }
            //    },
            //    Projection = new Projection
            //    {
            //        ProjectionType = "ALL"
            //    }
            //};

            var request = new CreateTableRequest
            {
                TableName = "st-personal_info",

                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1L,
                    WriteCapacityUnits = 1L
                },
                AttributeDefinitions = attributeDefinitions,
                KeySchema = tableKeySchema,
                //GlobalSecondaryIndexes = {
                //    NameIndex
                //}
            };

            await client.CreateTableAsync(request);
        }

        private static async Task CreateSkillTable(AmazonDynamoDBClient client)
        {
            // Attribute definitions
            var attributeDefinitions = new List<AttributeDefinition>()
            {
                new AttributeDefinition
                {
                    AttributeName = "empId",
                    AttributeType = "S"
                },
                new AttributeDefinition
                {
                    AttributeName = "skillId",
                    AttributeType = "N"
                },
                new AttributeDefinition
                {
                    AttributeName = "name",
                    AttributeType = "S"
                },
                new AttributeDefinition
                {
                    AttributeName = "proficiency",
                    AttributeType = "N"
                }
            };

            // Key schema for table
            var tableKeySchema = new List<KeySchemaElement>()
            {
                new KeySchemaElement
                {
                    AttributeName = "empId",
                    KeyType = "HASH"  //Partition key
                },
                new KeySchemaElement
                {
                    AttributeName = "skillId",
                    KeyType = "RANGE"  //Sort key
                }
            };

            // Initial provisioned throughput settings for the indexes
            var ptIndex = new ProvisionedThroughput
            {
                ReadCapacityUnits = 1L,
                WriteCapacityUnits = 1L
            };

            // NameIndex
            var NameIndex = new GlobalSecondaryIndex()
            {
                IndexName = "NameIndex",
                ProvisionedThroughput = ptIndex,
                KeySchema = {
                    new KeySchemaElement {
                        AttributeName = "name", KeyType = "HASH" //Partition key
                    },
                    new KeySchemaElement {
                        AttributeName = "proficiency", KeyType = "RANGE" //Sort key
                    }
                },
                Projection = new Projection
                {
                    ProjectionType = "ALL"
                }
            };

            var request = new CreateTableRequest
            {
                TableName = "st-skill",

                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1L,
                    WriteCapacityUnits = 1L
                },
                AttributeDefinitions = attributeDefinitions,
                KeySchema = tableKeySchema,
                GlobalSecondaryIndexes = {
                    NameIndex
                }
            };

            await client.CreateTableAsync(request);
        }
    }
}
