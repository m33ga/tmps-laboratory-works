using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;
using System.Text;

namespace DocumentPipeline.Infrastructure.Services;

public class DocumentProcessor
{
    private readonly IDocumentReader _documentReader;
    private readonly IProcessorFactory _processorFactory;
    private readonly IWriterFactory _writerFactory;
    private readonly IEncryptorFactory _encryptorFactory;
    private readonly ProcessingOptions _options;

    public DocumentProcessor(
        IDocumentReader documentReader,
        IProcessorFactory processorFactory,
        IWriterFactory writerFactory,
        IEncryptorFactory encryptorFactory,
        ProcessingOptions options)
    {
        _documentReader = documentReader;
        _processorFactory = processorFactory;
        _writerFactory = writerFactory;
        _encryptorFactory = encryptorFactory;
        _options = options;
    }

    public void Process(Document document)
    {
        Console.WriteLine($"\nProcessing: {document.Name}");
        
        Console.WriteLine($"[1/4] Reading file...");
        var content = _documentReader.Read(document.FilePath);

        var processor = _processorFactory.CreateProcessor(content.Type);
        Console.WriteLine($"[2/4] Formatting {content.Type} content...");
        content = processor.Process(content);
        
        var encryptor = _encryptorFactory.CreateEncryptor(_options); 

        Console.WriteLine($"[3/4] Applying encryption: {_options.EncryptionType}");
        byte[] contentBytes = Encoding.UTF8.GetBytes(content.ProcessedContent);
        byte[] processedBytes = encryptor.Encrypt(contentBytes);
        
        if (_options.EncryptionType != EncryptionType.None)
        {
            content.ProcessedContent = Convert.ToBase64String(processedBytes);
        }

        var writer = _writerFactory.CreateWriter(_options.StorageType);
        string outputPath = GenerateOutputPath(document, content.Type);
        Console.WriteLine($"[4/4] Writing to {_options.StorageType}: {outputPath}");
        writer.Write(content, outputPath);

        Console.WriteLine($"Completed");
        Console.WriteLine($"Metadata: {string.Join(", ", content.Metadata)}");
    }

    private string GenerateOutputPath(Document document, DocumentType actualType)
    {
        string fileName = Path.GetFileNameWithoutExtension(document.Name);
        string extension = Path.GetExtension(document.Name);
        
        return Path.Combine(_options.OutputPath, $"{fileName}_processed{extension}");
    }
}