using EventAssociation.Core.Application.DependencyInjection;
using EventAssociation.Core.Domain.ReositoryInterfaces; // for IEventRepository, IUnitOfWork
using EventAssociation.Infrastructure.SqlliteDmPersistence; // for VeaDbContext, repository implementations
using Core.Tools.ObjectMapper;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Repositories;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs;
using EventAssociation.Presentation.WebAPI.Mapping; // for IObjectMapper, ObjectMapper
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers (REPR pattern uses controllers under /Endpoints)
builder.Services.AddControllers();

// Register Application layer services (ICommandDispatcher + all ICommandHandler<>) 
builder.Services.AddApplicationServices();

builder.Services.AddDbContext<VeaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the repository and unit-of-work from Infrastructure
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register the ObjectMapper (Core.Tools.ObjectMapper)
builder.Services.AddScoped<IObjectMapper, ObjectMapper>();
builder.Services.AddScoped<IMapping<VeaEvent, EventDto>, VeaEventToEventDtoMapping>();


// Add Swagger/OpenAPI (so we can hit endpoints via Swagger UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventAssociation API", Version = "v1" });
});

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventAssociation API V1"); });
}

// Standard middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
