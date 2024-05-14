namespace nfx_auth.Dtos;

public class RegisterDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; } = false;
}
