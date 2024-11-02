using System.ComponentModel.DataAnnotations;
using FluentValidation;
using StudentPortalServer.Entities;
using StudentPortalServer.Helpers;

namespace StudentPortalServer.Models;

public class RegisterRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public User ToUser()
    {
        var user = new User()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email
        };

        user.SetPassword(Password);

        return user;
    }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.FirstName).MinimumLength(1).MaximumLength(40);
        RuleFor(request => request.LastName).MinimumLength(1).MaximumLength(40);
        RuleFor(request => request.Password).MinimumLength(6).MaximumLength(40);
    }
}