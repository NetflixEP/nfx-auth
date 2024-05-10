namespace nfx_auth.Utils;

public interface IEncryptor
{
    string GetSalt();
    string GetHash(string value, string salt);
}