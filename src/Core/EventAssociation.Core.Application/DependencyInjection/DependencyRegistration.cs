﻿using System.Reflection;
using EventAssociation.Core.Application.Dispatch;
using EventAssociation.Core.Application.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssociation.Core.Application.DependencyInjection
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            var asm = Assembly.GetExecutingAssembly();
            var handlerTypes = asm.GetTypes()
                .SelectMany(t => t.GetInterfaces(), (t, i) => (Impl: t, Intf: i))
                .Where(x => x.Intf.IsGenericType
                    && x.Intf.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .ToList();

            foreach (var (impl, intf) in handlerTypes)
                services.AddScoped(intf, impl);

            services.Decorate<ICommandDispatcher, LoggingDispatcher>();
            // services.Decorate<ICommandDispatcher, SomeOtherDecorator>();
            // Last in first out - Decorators runs in reverse order of registration

            return services;
        }
    }
}
