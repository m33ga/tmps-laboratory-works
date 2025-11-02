using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Interfaces;

public interface IDocumentProcessor
{
    DocumentContent Process(DocumentContent content);
    bool CanProcess(DocumentType type);
}