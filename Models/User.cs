using nfx_auth.Utils;

namespace nfx_auth.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public void SetPassword(string password, IEncryptor encryptor)
    {
        Salt = encryptor.GetSalt();
        Password = encryptor.GetHash(password, Salt);
    }

    public bool ValidatePassword(string password, IEncryptor encryptor) =>
        Password == encryptor.GetHash(password, Salt);

}