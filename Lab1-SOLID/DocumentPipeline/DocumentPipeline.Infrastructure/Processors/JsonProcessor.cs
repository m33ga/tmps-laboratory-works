using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DocumentPipeline.Infrastructure.Processors;

public class JsonProcessor : IDocumentProcessor
{
    public bool CanProcess(DocumentType type) => type == DocumentType.Json;

    public DocumentContent Process(DocumentContent content)
    {
        try
        {
            var jsonDocument = JsonDocument.Parse(content.ProcessedContent);
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            content.ProcessedContent = JsonSerializer.Serialize(jsonDocument, options);
            content.Metadata["Formatted"] = "PrettyPrinted";
        }
        catch (JsonException ex)
        {
            content.Metadata["FormatError"] = ex.Message;
        }

        return content;
    }
}

