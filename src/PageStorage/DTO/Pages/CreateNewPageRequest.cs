using FluentValidation;
using StudentPortal.ComponentData;
using StudentPortal.PageStorage.Entities;

namespace StudentPortal.PageStorage.DTO;

#nullable disable

public class CreateNewPageRequest
{
    public Document Content { get; set; }
}

public class CreateNewPageRequestValidator : AbstractValidator<CreateNewPageRequest>
{
    public CreateNewPageRequestValidator()
    {
        RuleFor(request => request.Content).NotNull();
    }
}