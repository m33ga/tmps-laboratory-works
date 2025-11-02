namespace DocumentPipeline.Domain.Interfaces;

public interface IStorageClientProvider
{
    object GetClient();
}
