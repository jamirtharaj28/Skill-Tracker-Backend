using Admin.Application.Contracts;
using Admin.Domain.Entities;
using Admin.Infrastructure.Cache;
using Admin.Infrastructure.ESCache;
using Admin.Infrastructure.Repositories;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Admin.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEnyimMemcached(configuration);
            AmazonDynamoDBClient dbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
            {
                ServiceURL = configuration["DynamoDBServiceURL"],
                AuthenticationRegion = configuration["DynamoDbRegion"]
            });
            services.AddSingleton<AmazonDynamoDBClient>(dbClient);
            var dbContext = new DynamoDBContext(dbClient);
            services.AddSingleton<DynamoDBContext>(dbContext);

            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<ICacheRepository, CacheRepository>();

            services.AddSingleton<IPersonalInfoProvider, PersonalInfoProvider>();
            services.AddSingleton<ISkillProvider, SkillProvider>();
            
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
