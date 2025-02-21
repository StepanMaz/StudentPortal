namespace StudentPortal.PageStorage.Config;

#nullable disable

public record MongoConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}