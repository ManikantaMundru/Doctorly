using Doctorly.Calendar.Api.Middleware;
using Doctorly.Calendar.Application;
using Doctorly.Calendar.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Doctorly Calendar API",
        Version = "v1",
        Description = "Backend API for managing doctor scheduling events with optimistic concurrency and notification support.",
        Contact = new OpenApiContact
        {
            Name = "Doctorly Team",
            Email = "support@doctorly.de"
        }
    });

    // Display enums as strings instead of integers
    options.UseInlineDefinitionsForEnums();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddLogging();

var app = builder.Build();

//Uncomment the following lines to apply pending migrations on application startup for testing only.
//Make sure this is not used in production environments as it can lead to performance issues
//and potential data loss if not handled carefully. I have only added this for testing purposes to simplify the setup process.
//In a real-world application, you would typically handle database migrations separately from application startup,
//using tools like EF Core CLI or a dedicated migration service.

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<CalendarDbContext>();
//    await dbContext.Database.MigrateAsync();
//}

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Doctorly Calendar API v1");
        options.DocumentTitle = "Doctorly Calendar API Docs";
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
