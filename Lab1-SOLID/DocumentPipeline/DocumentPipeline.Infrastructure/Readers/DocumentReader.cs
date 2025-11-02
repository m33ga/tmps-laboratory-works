using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Infrastructure.Readers;

public class DocumentReader : IDocumentReader
{
    public bool CanRead(DocumentType type) => true;

    public DocumentContent Read(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        DocumentType type = DetermineDocumentType(filePath);
        string content = File.ReadAllText(filePath);
        
        var docContent = new DocumentContent(content, type);
        docContent.Metadata["OriginalSize"] = content.Length.ToString();
        docContent.Metadata["Extension"] = Path.GetExtension(filePath);
        docContent.Metadata["LineCount"] = content.Split('\n').Length.ToString();
        
        return docContent;
    }

    private DocumentType DetermineDocumentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        
        return extension switch
        {
            ".txt" => DocumentType.Txt,
            ".json" => DocumentType.Json,
            ".csv" => DocumentType.Csv,
            _ => throw new NotSupportedException($"File extension {extension} is not supported")
        };
    }
}