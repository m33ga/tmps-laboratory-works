using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Interfaces
{
    public interface IDocumentProcessor
    {
        bool CanProcess(DocumentType type);

        void Process(Document document);
    }
}