using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Interfaces;

public interface IEncryptorFactory
{
    IDocumentEncryptor CreateEncryptor(ProcessingOptions options);
}