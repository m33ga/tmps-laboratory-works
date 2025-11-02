using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Interfaces;

public interface IDocumentReader
{
    DocumentContent Read(string filePath);
    bool CanRead(DocumentType type);
}