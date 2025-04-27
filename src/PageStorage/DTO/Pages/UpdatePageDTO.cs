using System.Text.Json.Serialization;
using FluentValidation;
using StudentPortal.ComponentData;
using StudentPortal.ComponentData.Serialization;

namespace StudentPortal.PageStorage.DTO;

#nullable disable
public class UpdatePageDTO
{
    [JsonConverter(typeof(PrimitiveDictionaryConverter))]
    public Dictionary<string, object> Metadata { get; set; }
    public Document Content { get; set; } = null;
    public string Name { get; set; } = null;
}
#nullable restore

public class UpdatePageDTOValidator : AbstractValidator<UpdatePageDTO>
{
    public UpdatePageDTOValidator()
    {
        RuleFor(dto => dto.Content).NotNull();
        RuleFor(dto => dto.Name).NotEmpty();
    }
}