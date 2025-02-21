
namespace StudentPortal.AuthService;

public class JWTConfig
{
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public string SecretKey { get; set; } = "";
}