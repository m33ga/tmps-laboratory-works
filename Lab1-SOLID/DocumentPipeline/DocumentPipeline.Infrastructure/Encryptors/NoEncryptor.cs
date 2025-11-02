using DocumentPipeline.Domain.Interfaces;

namespace DocumentPipeline.Infrastructure.Encryptors;

public class NoEncryptor : IDocumentEncryptor
{
    public byte[] Encrypt(byte[] content) => content;
    
    public byte[] Decrypt(byte[] encryptedContent) => encryptedContent;
}
    