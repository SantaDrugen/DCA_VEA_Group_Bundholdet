using EventAssociation.Infrastructure.SqlliteDmPersistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueryContracts.Contracts;
using QueryContracts.Dispatch;

namespace EfcQueries.DependencyInjection
{
    public static class QueryDependencyRegistration
    {
        public static IServiceCollection AddQueryServices(
            this IServiceCollection services,
            IConfiguration configuration) // We need configuration to get the connection string
        {
            // Auto-register all query handlers - great for extendibility
            var asm = typeof(QueryDependencyRegistration).Assembly;
            var queryHandlerTypes = asm.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("QueryHandler"))
                .ToList();
            foreach (var handlerType in queryHandlerTypes)
            {
                var interfaceType = handlerType.GetInterface($"I{handlerType.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, handlerType);
                }
            }

            services.AddDbContext<VeadatabaseProductionContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            return services;
        }
    }
}
