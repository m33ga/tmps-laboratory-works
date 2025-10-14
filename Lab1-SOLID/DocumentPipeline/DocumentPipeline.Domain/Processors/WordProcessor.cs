using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Processors
{
    public class WordProcessor : IDocumentProcessor
    {
        public bool CanProcess(DocumentType type) => type == DocumentType.Word;

        public void Process(Document document)
        {
            Console.WriteLine($"Processing Word: {document.Name}");
            Console.WriteLine($"Size: {document.SizeKb}KB -> {document.SizeKb * 0.6:F1}KB");
        }
    }
}