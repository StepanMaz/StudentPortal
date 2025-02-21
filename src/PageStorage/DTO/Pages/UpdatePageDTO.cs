using StudentPortal.ComponentData;

namespace StudentPortal.PageStorage.DTO;

public class UpdatePageDTO
{
    public Dictionary<string, object>? Metadata { get; set; } = null;
    public Document? Content { get; set; } = null;
}