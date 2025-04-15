using System.Text.Json.Serialization;

namespace StudentPortal.PageEditor.Models;

[JsonDerivedType(typeof(UrlImageData))]
public interface IImageData
{
    string GetUrl();
}

public record UrlImageData(string Url) : IImageData
{
    public string GetUrl() => Url;
}


public record FileData(Guid Id, string Name, IImageData ImageData);