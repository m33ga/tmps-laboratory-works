using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using DocumentPipeline.Infrastructure.Processors;

namespace DocumentPipeline.Infrastructure.Factories;

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
    