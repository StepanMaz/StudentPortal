using StudentPortal.ComponentData;

namespace StudentPortal.PageEditor.Models;

public class PageData
{
    public required Guid Id {get; set; }
    public required string Name { get; set; }
    public required Document Content { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = [];
}