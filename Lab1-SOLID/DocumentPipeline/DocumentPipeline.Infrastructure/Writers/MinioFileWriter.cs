using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;
using Minio;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Minio.DataModel.Args;

namespace DocumentPipeline.Infrastructure.Writers;
    public class MinioWriter : IDocumentWriter
{
    private readonly IStorageClientProvider _clientProvider;

    public StorageType SupportedStorage => StorageType.MinIO;

    public MinioWriter(IStorageClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
    }

    public void Write(DocumentContent content, string outputPath)
    {
        var client = (IMinioClient)_clientProvider.GetClient();

        string[] parts = outputPath.Split('/', 2);
        if (parts.Length < 2)
            throw new ArgumentException("Output path must be in format: bucket/object-name");

        string bucketName = parts[0];
        string objectName = parts[1];

        EnsureBucketExistsAsync(client, bucketName).Wait();
        UploadContentAsync(client, bucketName, objectName, content.ProcessedContent).Wait();
    }

    private async Task EnsureBucketExistsAsync(IMinioClient client, string bucketName)
    {
        bool found = await client.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucketName));

        if (!found)
        {
            await client.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(bucketName));
        }
    }

    private async Task UploadContentAsync(IMinioClient client, string bucketName, string objectName, string content)
    {
        byte[] data = Encoding.UTF8.GetBytes(content);
        using var stream = new MemoryStream(data);

        await client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType("text/plain"));
    }
}
