using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nfx_auth.Data;
using nfx_auth.Dtos;
using nfx_auth.Models;
using nfx_auth.Utils;

namespace nfx_auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthDbContext dbContext, IJwtBuilder jwtBuilder, IEncryptor encryptor) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user is null)
        {
            return NotFound("User not found.");
        }

        if (user.PasswordHash != encryptor.GetHash(loginDto.Password, user.Salt))
        {
            return Unauthorized("Could not authenticate user.");
        }

        var token = jwtBuilder.GetToken(user.Email, user.IsAdmin);

        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

        if (user is not null)
        {
            return Unauthorized("User already exists.");
        }

        var salt = encryptor.GetSalt();

        var newUser = new User
        {
            Email = registerDto.Email,
            Salt = salt,
            PasswordHash = encryptor.GetHash(registerDto.Password, salt),
            IsAdmin = registerDto.IsAdmin
        };

        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        return Ok();
    }
}