using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Infrastructure.Auth;
using StackOverflowLite.Infrastructure.Caching;

namespace StackOverflowLite.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis");

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection + ",abortConnect=false"));
        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }
}
