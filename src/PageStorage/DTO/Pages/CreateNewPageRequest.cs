using System.Text.Json.Serialization;
using FluentValidation;
using StudentPortal.ComponentData;
using StudentPortal.ComponentData.Serialization;

namespace StudentPortal.PageStorage.DTO;

#nullable disable

public class CreateNewPageRequest
{
    public string Name { get; set; }
    public Document Content { get; set; }
    [JsonConverter(typeof(PrimitiveDictionaryConverter))]
    public Dictionary<string, object> Metadata { get; set; }
}

public class CreateNewPageRequestValidator : AbstractValidator<CreateNewPageRequest>
{
    public CreateNewPageRequestValidator()
    {
        RuleFor(request => request.Content).NotNull();
        RuleFor(request => request.Name).NotEmpty();
    }
}