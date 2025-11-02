using DocumentPipeline.Domain.Models;

namespace DocumentPipeline.Domain.Interfaces;

public interface IDocumentEncryptor
{
    byte[] Encrypt(byte[] content);
    byte[] Decrypt(byte[] encryptedContent);
}
