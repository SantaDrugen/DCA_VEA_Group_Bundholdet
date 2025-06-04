// File: EventAssociation.Presentation.WebAPI/Extensions/ServiceCollectionExtensions.cs
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Presentation.WebAPI.Mapping;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;

namespace EventAssociation.Presentation.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods to register all necessary services in DI:
    /// - ObjectMapper & mappings
    /// </summary>
    public static class ServiceCollectionExtensions
    {
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
