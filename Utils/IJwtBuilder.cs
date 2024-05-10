namespace nfx_auth.Utils;

public interface IJwtBuilder
{
    string GetToken(string email, bool isAdmin);
    (string, bool)  ValidateToken(string token);
}