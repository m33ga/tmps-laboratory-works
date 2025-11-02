using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;

namespace DocumentPipeline.Domain.Builders;

public class ProcessingOptionsBuilder
{
    private EncryptionType _encryptionType = EncryptionType.None;
    private string? _encryptionKey;
    private StorageType _storageType = StorageType.Local;
    private string _outputPath = "./output";

    public ProcessingOptionsBuilder WithEncryption(EncryptionType type, string? key = null)
    {
        _encryptionType = type;
        _encryptionKey = key;
        return this;
    }

    public ProcessingOptionsBuilder WithStorage(StorageType type, string path)
    {
        _storageType = type;
        _outputPath = path;
        return this;
    }

    public ProcessingOptions Build()
    {
        if (_encryptionType != EncryptionType.None && string.IsNullOrEmpty(_encryptionKey))
        {
            throw new InvalidOperationException("Encryption key is required when encryption is enabled");
        }

        return new ProcessingOptions
        {
            EncryptionType = _encryptionType,
            EncryptionKey = _encryptionKey,
            StorageType = _storageType,
            OutputPath = _outputPath
        };
    }
}
