using System.Reflection;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.DependencyInjection
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
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
            services.AddScoped<VeaDbContext>();

            return services;
        }
    }
}
