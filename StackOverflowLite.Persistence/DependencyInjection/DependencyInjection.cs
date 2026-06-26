using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackOverflowLite.Persistence.Data;

namespace StackOverflowLite.Persistence.DependencyInjection;


public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<StackOverflowLite.Application.Services.Interfaces.IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }
}