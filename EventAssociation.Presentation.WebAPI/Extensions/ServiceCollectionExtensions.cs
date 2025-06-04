// File: EventAssociation.Presentation.WebAPI/Extensions/ServiceCollectionExtensions.cs
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Application.Queries;
using EventAssociation.Core.Application.Commands.Event;
using EventAssociation.Core.Domain.ReositoryInterfaces;
using EventAssociation.Infrastructure.Persistence;      // adjust namespace if your Infrastructure project differs
using EventAssociation.Infrastructure.Persistence.Repositories; // adjust accordingly
using EventAssociation.Presentation.WebAPI.Mapping;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssociation.Presentation.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods to register all necessary services in DI:
    /// - CommandDispatcher & command handlers
    /// - Repositories (via Infrastructure)
    /// - ObjectMapper & mappings
    /// - (Optional) Query handlers
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 1) CommandDispatcher and CommandHandlers
            //    (Assumes you have an ICommandHandler<TCommand> storage in your Application layer)
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            //    If you manually register each handler, do e.g.:
            // services.AddScoped<ICommandHandler<CreateNewEventCommand>, CreateNewEventCommandHandler>();
            // services.AddScoped<ICommandHandler<SetEventTitleCommand>, SetEventTitleCommandHandler>();
            // etc.  (Or if you wrote a scanning utility, call it here.)

            // 2) QueryDispatcher and QueryHandlers (if you have IQueryDispatcher)
            //    Example:
            // services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            // services.AddScoped<IQueryHandler<GetEventByIdQuery, VeaEvent>, GetEventByIdQueryHandler>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // 1) Register your DbContexts & Repositories
            //    Example (Entity Framework Core):
            // services.AddDbContext<WriteDbContext>(options => ...);
            // services.AddDbContext<ReadDbContext>(options => ...);

            // 2) Register the concrete repository that implements IEventRepository
            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }

        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            // 1) ObjectMapper itself
            services.AddScoped<IObjectMapper, ObjectMapper>();

            // 2) Custom mapping VeaEvent → EventDto
            services.AddScoped<IMapping<VeaEvent, EventDto>, VeaEventToEventDtoMapping>();

            return services;
        }
    }
}
