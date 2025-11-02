namespace DocumentPipeline.Domain.Models;

public class DocumentContent
{
    public string RawContent { get; set; }
    public string ProcessedContent { get; set; }
    public DocumentType Type { get; set; }
    public Dictionary<string, string> Metadata { get; set; }

    public DocumentContent(string rawContent, DocumentType type)
    {
        RawContent = rawContent;
        ProcessedContent = rawContent;
        Type = type;
        Metadata = new Dictionary<string, string>();
    }
}