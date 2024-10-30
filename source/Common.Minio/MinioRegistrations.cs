using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace Common.Minio;

public static class MinioRegistrations
{
    public static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration minioConfiguration)
    {
        // Register MinioOptions
        var minioOptions = minioConfiguration.Get<MinioOptions>();
        if (minioOptions == null)
            throw new ArgumentNullException("Minio is not configured.");
        services
            .AddSingleton<MinioOptions>(minioOptions);

        // Register MinioContext
        services.AddSingleton<IMinioContext, MinioContext>();

        // Register MinioClient
        services.AddSingleton<IMinioClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.Secret)
                .Build();
        });

        return services;
    }
}