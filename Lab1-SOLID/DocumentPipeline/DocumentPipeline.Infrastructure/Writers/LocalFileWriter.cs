using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;
using Minio;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DocumentPipeline.Infrastructure.Writers;
public class LocalFileWriter : IDocumentWriter
{
    public StorageType SupportedStorage => StorageType.Local;

    public void Write(DocumentContent content, string outputPath)
    {
        string directory = Path.GetDirectoryName(outputPath) ?? "./";
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(outputPath, content.ProcessedContent, Encoding.UTF8);
    }
}
