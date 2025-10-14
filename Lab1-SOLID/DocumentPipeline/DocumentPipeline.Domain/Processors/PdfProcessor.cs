using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Processors
{
    public class PdfProcessor : IDocumentProcessor
    {
        public bool CanProcess(DocumentType type) => type == DocumentType.Pdf;

        public void Process(Document document)
        {
            Console.WriteLine($"Processing PDF: {document.Name}");
            Console.WriteLine($"Size: {document.SizeKb}KB -> {document.SizeKb * 0.7:F1}KB");
        }
    }
}