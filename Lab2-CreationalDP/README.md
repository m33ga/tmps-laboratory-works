# Creational Design Patterns

For this laboratory work, I continued working on the project from the [1st laboratory work](../Lab1-SOLID) and updated it by introducing actual functionality in order to demonstrate 3 creational design patterns out of 6 total.

- Abstract Factory
- Builder
- Factory Method
- Prototype
- Singleton
- Object Pool


## Implementation

Out of them, I managed to use 3: `Factory`, `Builder` and `Singleton`.  

1. Factory

In order to centralize selection logic for `Writers`, `Encryptors` and `Processors` based on file type or selected processing options, `Factories` were used.

```csharp
public class ProcessorFactory : IProcessorFactory
{
    public IDocumentProcessor CreateProcessor(DocumentType type)
    {
        return type switch
        {
            DocumentType.Txt => new TxtProcessor(),
            DocumentType.Json => new JsonProcessor(),
            DocumentType.Csv => new CsvProcessor(),
            _ => throw new NotSupportedException($"No processor available for {type}")
        };
    }
}
```

Factories were also used to create Writers and Encryptors. Reader class did not need Factory as a single reader is used for all file types (for now).

2. Builder

In order to offer the possibility to create a `DocumentPipeline` with different `ProcessingOptions`, a `ProcessingOptionsBuilder` was used.
It gives the possibility of choosing `encryptionType` and `storageType` used

```csharp
public class ProcessingOptionsBuilder
{
    private EncryptionType _encryptionType = EncryptionType.None;
    private string? _encryptionKey;
    private StorageType _storageType = StorageType.Local;
    private string _outputPath = "./output";

    public ProcessingOptionsBuilder WithEncryption(EncryptionType type, string? key = null)
    {
        _encryptionType = type;
        _encryptionKey = key;
        return this;
    }

    public ProcessingOptionsBuilder WithStorage(StorageType type, string path)
    {
        _storageType = type;
        _outputPath = path;
        return this;
    }

    public ProcessingOptions Build()
    {
        if (_encryptionType != EncryptionType.None && string.IsNullOrEmpty(_encryptionKey))
        {
            throw new InvalidOperationException("Encryption key is required when encryption is enabled");
        }

        return new ProcessingOptions
        {
            EncryptionType = _encryptionType,
            EncryptionKey = _encryptionKey,
            StorageType = _storageType,
            OutputPath = _outputPath
        };
    }
}
```

Using the builder is simple: 
```csharp
var options = new ProcessingOptionsBuilder()
    .WithEncryption(EncryptionType.AES256, "topsecretkeyyouarenotgoingtoguess")
    .WithStorage(StorageType.Local, "./output/encrypted")
    .Build();
```

3. Singleton

Storing the processed files can be done in `MinIO` object storage. To use the MinIO client easily with the same config a `MinioClientProvider` class was used.
It uses **eager initialization** to create a single client provider based on the config from `appsettings.json`. 
This is more of a proof of concept as simple DI would be enough.

```csharp
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
```

## Conclusion

The DocumentPipeline project got some life and now can read files, process them with independent rules for each file type (txt, csv, json), encrypt them (optionally) and store them locally or in MinIO.
At the same time it provided a nice chance to work with Creational Design Patterns.
