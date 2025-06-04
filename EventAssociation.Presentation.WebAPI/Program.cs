using EventAssociation.Core.Application.DependencyInjection;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EventAssociation.Infrastructure.SqlliteDmPersistence.DependencyInjection;
using EventAssociation.Presentation.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers (REPR pattern uses controllers under /Endpoints)
builder.Services.AddControllers();

// Register Application layer services (ICommandDispatcher + all ICommandHandler<>) 
builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

// builder.Services.AddQueryServices();  -- remove comment after pushed to master -- method only exists in master branch

// 3) Register Presentation services (object‐mapper + custom mappings)
builder.Services.AddPresentationServices();

builder.Services.AddDbContext<VeaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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
