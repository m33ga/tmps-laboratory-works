using DocumentPipeline.Domain.Models;
using DocumentPipeline.Domain.Enums;
using DocumentPipeline.Domain.Builders;
using DocumentPipeline.Infrastructure.Factories;
using DocumentPipeline.Infrastructure.Services;
using DocumentPipeline.Infrastructure.Storage;
using DocumentPipeline.Infrastructure.Readers;

namespace DocumentPipeline.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Document Processing Pipeline");
            
            var testCases = new[]
            {
                ProcessWithLocalStorage(),
                ProcessWithEncryption(),
                ProcessWithMinIO()
            };

            foreach (var testCase in testCases)
            {
                RunTestCase(testCase);
            }

            System.Console.WriteLine("All processing completed. Check output folders.");
            System.Console.WriteLine("\nPress any key to exit...");
            System.Console.ReadKey();
        }

        static (string name, Action action) ProcessWithLocalStorage()
        {
            return ("Local Storage - No Encryption", () =>
            {
                var options = new ProcessingOptionsBuilder()
                    .WithStorage(StorageType.Local, "./output/local")
                    .Build();

                var processor = CreateProcessor(options);

                processor.Process(new Document("./TestFiles/sample_txt.txt"));
                processor.Process(new Document("./TestFiles/sample_json.json"));
                processor.Process(new Document("./TestFiles/sample_csv.csv"));
            });
        }

        static (string name, Action action) ProcessWithEncryption()
        {
            return ("Local Storage - With AES256 Encryption", () =>
            {
                var options = new ProcessingOptionsBuilder()
                    .WithEncryption(EncryptionType.AES256, "topsecretkeyyouarenotgoingtoguess")
                    .WithStorage(StorageType.Local, "./output/encrypted")
                    .Build();

                var processor = CreateProcessor(options);

                processor.Process(new Document("./TestFiles/sample_txt.txt"));
                processor.Process(new Document("./TestFiles/sample_json.json"));
                processor.Process(new Document("./TestFiles/sample_csv.csv"));
            });
        }

        static (string name, Action action) ProcessWithMinIO()
        {
            return ("MinIO Storage - No Encryption", () =>
            {
                var options = new ProcessingOptionsBuilder()
                    .WithStorage(StorageType.MinIO, "faf-233/documents")
                    .Build();

                var processor = CreateProcessor(options);

                processor.Process(new Document("./TestFiles/sample_txt.txt"));
                processor.Process(new Document("./TestFiles/sample_json.json"));
                processor.Process(new Document("./TestFiles/sample_csv.csv"));
            });
        }

        static DocumentProcessor CreateProcessor(ProcessingOptions options)
        {
            var documentReader = new DocumentReader();
            var processorFactory = new ProcessorFactory();
            var encryptorFactory = new EncryptorFactory();
            var writerFactory = new WriterFactory(MinioClientProvider.GetInstance);

            return new DocumentProcessor(documentReader, processorFactory, writerFactory, encryptorFactory, options);
        }

        static void RunTestCase((string name, Action action) testCase)
        {
            System.Console.WriteLine($"Test Case: {testCase.name}");

            try
            {
                testCase.action();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}