using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Processors;

namespace DocumentPipeline.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var processors = new List<Domain.Interfaces.IDocumentProcessor>
            {
                new PdfProcessor(),
                new WordProcessor(),
                new MarkdownProcessor()
            };
            
            var pipeline = new Domain.Services.DocumentPipeline(processors);
            
            var documents = new List<Document>
            {
                new Document("report.pdf", DocumentType.Pdf, 2048),
                new Document("proposal.docx", DocumentType.Word, 1536),
                new Document("readme.md", DocumentType.Markdown, 512)
            };
            
            foreach (var doc in documents)
            {
                pipeline.ProcessDocument(doc);
            }

            System.Console.WriteLine("\nPress any key to exit...");
            System.Console.ReadKey();
        }
    }
}