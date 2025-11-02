using DocumentPipeline.Domain.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DocumentPipeline.Infrastructure.Encryptors;
public class AesEncryptor : IDocumentEncryptor
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesEncryptor(string key)
    {
        using var sha256 = SHA256.Create();
        _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        _iv = new byte[16];
        Array.Copy(_key, _iv, 16);
    }

    public byte[] Encrypt(byte[] content)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        
        csEncrypt.Write(content, 0, content.Length);
        csEncrypt.FlushFinalBlock();
        
        return msEncrypt.ToArray();
    }

    public byte[] Decrypt(byte[] encryptedContent)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var msDecrypt = new MemoryStream(encryptedContent);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var msResult = new MemoryStream();
        
        csDecrypt.CopyTo(msResult);
        return msResult.ToArray();
    }
}
