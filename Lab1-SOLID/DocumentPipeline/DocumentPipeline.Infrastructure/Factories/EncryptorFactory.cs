using DocumentPipeline.Domain.Enums;
using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using DocumentPipeline.Infrastructure.Encryptors;
namespace DocumentPipeline.Infrastructure.Factories;

public class EncryptorFactory : IEncryptorFactory
{
    public IDocumentEncryptor CreateEncryptor(ProcessingOptions options)
    {
        return options.EncryptionType switch
        {
            EncryptionType.None => new NoEncryptor(),
            EncryptionType.AES256 => new AesEncryptor(options.EncryptionKey!),
            _ => throw new NotSupportedException($"Encryption type {options.EncryptionType} not supported")
        };    }
}