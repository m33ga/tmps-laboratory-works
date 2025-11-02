using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;

namespace DocumentPipeline.Domain.Interfaces;

public interface IProcessorFactory
{
    IDocumentProcessor CreateProcessor(DocumentType type);
}
