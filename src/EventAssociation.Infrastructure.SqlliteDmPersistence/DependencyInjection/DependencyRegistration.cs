using System.Reflection;
using System.Runtime.CompilerServices;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.DependencyInjection
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration) // We need configuration to get the connection string
        {
            services.AddDbContext<VeaDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            // Auto-register all repository implementations - great for extendibility
            var asm = Assembly.GetExecutingAssembly();
            var repositoryTypes = asm.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();
            foreach (var repoType in repositoryTypes)
            {
                var interfaceType = repoType.GetInterface($"I{repoType.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repoType);
                }
            }

            // These are one-off services, so we register them explicitly
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Note - if the connection fails, add a connection string in appsettings.json
            // It should look like this:
            // "ConnectionStrings": {
            //     "DefaultConnection": "Data Source=*some string here*"
            // }
            // 
            // Given the project is only a dummy, this may seem like overkill. But in production,
            // sensitive info like connection strings will be fetched from a keyvault at runtime,
            // and then it would be set up in the configuration.

            return services;
        }
    }
}
