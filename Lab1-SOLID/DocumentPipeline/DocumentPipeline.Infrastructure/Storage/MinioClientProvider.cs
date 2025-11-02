using DocumentPipeline.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;

namespace DocumentPipeline.Infrastructure.Storage;

public sealed class MinioClientProvider : IStorageClientProvider
{
    private static readonly MinioClientProvider Instance = new();
    public static MinioClientProvider GetInstance => Instance;

    private readonly IMinioClient _client;

    private MinioClientProvider()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var section = config.GetSection("Minio");

        string endpoint = section["Endpoint"]!;
        var accessKey = section["AccessKey"]!;
        var secretKey = section["SecretKey"]!;
        var useSSL = bool.Parse(section["UseSSL"] ?? "true");

        var client = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey);

        if (useSSL)
            client = client.WithSSL();

        _client = client.Build();
    }

    public object GetClient() => _client;
}