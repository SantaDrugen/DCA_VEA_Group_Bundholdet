using Core.Tools.ObjectMapper;
using EventAssociation.Core.Application.DependencyInjection;
using EventAssociation.Infrastructure.SqlliteDmPersistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IObjectMapper, ObjectMapper>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

//    (Later) We will call our own extension methods here, such as:
//    builder.Services.AddCommandHandlers(); // from the Application layer
//    builder.Services.AddQueryHandlers();   // from the Application layer
//    builder.Services.AddObjectMapper();    // from our new Core/Tools project
//    // TODO: builder.Services.AddDbContexts(); // once DbContext is available

// Changed to use Swagger instead of OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();