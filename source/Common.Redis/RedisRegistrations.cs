using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Common.Redis;

public static class RedisRegistrations
{
    public static IServiceCollection AddRedisServices(
        this IServiceCollection services,
        IConfiguration redisConfiguration,
        string? name = null,
        bool registerConnectionMultiplexer = false)
    {
        var redisOptions = redisConfiguration.Get<RedisOptions>();
        if (redisOptions == null)
            throw new ArgumentNullException("Redis is not configured.");
        
        services
            .AddSingleton<RedisOptions>(redisOptions)
            .AddSingleton<IRedisContext, RedisContext>();

        if (registerConnectionMultiplexer)
        {
            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var redisContext = serviceProvider.GetRequiredService<IRedisContext>();
                return redisContext.Connection;
            });
        }

        return services;
    }
}