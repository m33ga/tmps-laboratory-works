using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Processors
{
    public class MarkdownProcessor : IDocumentProcessor
    {
        public bool CanProcess(DocumentType type) => type == DocumentType.Markdown;

        public void Process(Document document)
        {
            Console.WriteLine($"Processing Markdown: {document.Name}");
            Console.WriteLine($"Size: {document.SizeKb}KB -> {document.SizeKb * 0.5:F1}KB");
        }
    }
}