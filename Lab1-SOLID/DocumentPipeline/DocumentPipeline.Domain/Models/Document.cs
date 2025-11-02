namespace DocumentPipeline.Domain.Models;

public class Document
{
    public string Name { get; set; }
    public DocumentType Type { get; set; }
    public string FilePath { get; set; }

    public Document(string filePath)
    {
        FilePath = filePath;
        Name = Path.GetFileName(filePath);
        Type = DetermineTypeFromExtension(filePath);
    }

    private DocumentType DetermineTypeFromExtension(string filePath)
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

    public override string ToString()
    {
        return $"{Name} ({Type})";
    }
}