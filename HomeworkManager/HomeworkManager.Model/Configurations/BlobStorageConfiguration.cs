namespace HomeworkManager.Model.Configurations;

public class BlobStorageConfiguration
{
    public required string ConnectionString { get; set; }
    public required string ContainerName { get; set; }
}