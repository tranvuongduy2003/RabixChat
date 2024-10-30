using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cassandra;

public static class CassandraRegistrations
{
    public static IServiceCollection AddCassandra<TDbContextImplementation, TDbContext>(
        this IServiceCollection services,
        IConfiguration cassandraConfiguration)
        where TDbContext : class, ICassandraDbContext
        where TDbContextImplementation : CassandraDbContext, TDbContext
    {
        services.AddSingleton<TDbContextImplementation>();
        services.AddSingleton<ICassandraDbContext>(provider => provider.GetRequiredService<TDbContextImplementation>());

        var cassandraOptions = (CassandraOptions<TDbContext>)cassandraConfiguration.Get<CassandraOptions>();
        if (cassandraOptions == null)
            throw new ArgumentNullException("Cassandra is not configured.");

        services
            .AddSingleton<CassandraOptions<TDbContext>>(cassandraOptions);

        return services;
    }
}