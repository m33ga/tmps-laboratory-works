using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;


namespace DocumentPipeline.Domain.Services
{
    public class DocumentPipeline
    {
        private readonly IEnumerable<IDocumentProcessor> _processors;
        public DocumentPipeline(IEnumerable<IDocumentProcessor> processors)
        {
            _processors = processors ?? throw new ArgumentNullException(nameof(processors));
        }
        
        public void ProcessDocument(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            Console.WriteLine($"\n[Pipeline] Processing: {document.Name} ({document.Type})");
            
            var processor = _processors.FirstOrDefault(p => p.CanProcess(document.Type));

            if (processor == null)
            {
                Console.WriteLine($"No processor found for {document.Type}");
                return;
            }
            
            processor.Process(document);
            Console.WriteLine("Done");
        }
    }
}