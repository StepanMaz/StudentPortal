using System.Security.Claims;

namespace StudentPortal.Auth;

public static class UserExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var idString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(idString))
        {
            return null;
        }

        return Guid.Parse(idString);
    }

    public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid id)
    {
        var userId = principal.GetUserId();

        if (userId is null)
        {
            id = Guid.Empty;
            return false;
        }

        id = userId.Value;

        return true;
    }
}