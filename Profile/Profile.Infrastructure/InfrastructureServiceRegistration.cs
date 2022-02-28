using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Profile.Application.Contracts;
using Profile.Infrastructure.Repositories;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;
using System;
using Microsoft.Azure.Cosmos;
using Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Profile.Infrastructure
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
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            services.AddSingleton<CosmosClient>(cosmosClient);
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;
            database.CreateContainerIfNotExistsAsync(containerId, "/empId");
            services.AddDbContext<SkillTrackerContext>(option => option.UseCosmos(ConnectionString, databaseId));
            
            services.AddScoped<IProfileRepository, ProfileRepository>();         
            return services;
        }
    }
}
