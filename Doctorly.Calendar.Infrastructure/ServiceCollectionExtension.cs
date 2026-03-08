using Doctorly.Calendar.Application.Common.Interfaces;
using Doctorly.Calendar.Infrastructure.Persistence;
using Doctorly.Calendar.Infrastructure.Repositories;
using Doctorly.Calendar.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doctorly.Calendar.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");

            services.AddDbContext<CalendarDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<IIcalBuilder, IcalBuilderService>();

            return services;
        }
    }
}
