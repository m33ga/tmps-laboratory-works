using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;

namespace DocumentPipeline.Domain.Interfaces;

public interface IWriterFactory
{
    IDocumentWriter CreateWriter(StorageType type);
}