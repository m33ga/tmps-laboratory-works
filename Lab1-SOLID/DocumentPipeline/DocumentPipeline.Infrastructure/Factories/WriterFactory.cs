using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Enums;
using DocumentPipeline.Infrastructure.Writers;

namespace DocumentPipeline.Infrastructure.Factories;


public class WriterFactory : IWriterFactory
{
    private readonly IStorageClientProvider _clientProvider;

    public WriterFactory(IStorageClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
    }

    public IDocumentWriter CreateWriter(StorageType type)
    {
        return type switch
        {
            StorageType.Local => new LocalFileWriter(),
            StorageType.MinIO => new MinioWriter(_clientProvider),
            _ => throw new NotSupportedException($"No writer available for {type}")
        };
    }
}
