using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;

namespace DocumentPipeline.Domain.Interfaces;

public interface IDocumentWriter
{
    void Write(DocumentContent content, string outputPath);
    StorageType SupportedStorage { get; }
}