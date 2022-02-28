using Admin.Application.Contracts;
using Admin.Domain.Entities;
using Admin.Infrastructure.Cache;
using Admin.Infrastructure.ESCache;
using Admin.Infrastructure.Repositories;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Admin.Infrastructure
{
    public static class InfrastructureServiceRegistration
    { 
        private static readonly string EndpointUri = "https://skilltracker.documents.azure.com:443/";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "aal67Ry0PBjpcihpXSkOqSoetqKglmyNpxxhRmb3z1v7CebJD5AwFnRjiH8M36mK4TOvirCB2MPIk2JY7unsKg==";
        private static readonly string ConnectionString = "AccountEndpoint=https://skilltracker.documents.azure.com:443/;AccountKey=aal67Ry0PBjpcihpXSkOqSoetqKglmyNpxxhRmb3z1v7CebJD5AwFnRjiH8M36mK4TOvirCB2MPIk2JY7unsKg==;";

        // The name of the database and container we will create
        private static string databaseId = "SkillTracker";
        private static string containerId = "SkillTrackerContainer";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEnyimMemcached(configuration);
            //AmazonDynamoDBClient dbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
            //{
            //    ServiceURL = configuration["DynamoDBServiceURL"],
            //    AuthenticationRegion = configuration["DynamoDbRegion"]
            //});
            //services.AddSingleton<AmazonDynamoDBClient>(dbClient);
            //var dbContext = new DynamoDBContext(dbClient);
            //services.AddSingleton<DynamoDBContext>(dbContext);
            
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            services.AddSingleton<CosmosClient>(cosmosClient);
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;
            database.CreateContainerIfNotExistsAsync(containerId, "/empId");
            services.AddDbContext<CosmosDbContext>(option => option.UseCosmos(ConnectionString, databaseId));

            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<ICacheRepository, CacheRepository>();

            services.AddScoped<IProfileRepository, ProfileRepository>();
            //services.AddSingleton<IPersonalInfoProvider, PersonalInfoProvider>();
            //services.AddSingleton<ISkillProvider, SkillProvider>();
            
            // services.AddSingleton<IElasticsearchRepository, ElasticsearchRepository>();
            
            return services;
        }

        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))

                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<ESDocument>(m => m
                    .PropertyName(c => c.EmpId, "empId")
                    .PropertyName(c => c.Name, "name")
                    .PropertyName(c => c.Skills, "skills")
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
