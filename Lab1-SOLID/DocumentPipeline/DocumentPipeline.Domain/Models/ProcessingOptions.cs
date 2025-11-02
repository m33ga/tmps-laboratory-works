using DocumentPipeline.Domain.Enums;

namespace DocumentPipeline.Domain.Models;

public class ProcessingOptions
{
    public EncryptionType EncryptionType { get; set; }
    public string? EncryptionKey { get; set; }
    public StorageType StorageType { get; set; }
    public string OutputPath { get; set; }

    public ProcessingOptions()
    {
        EncryptionType = EncryptionType.None;
        StorageType = StorageType.Local;
        OutputPath = "./output";
    }
}