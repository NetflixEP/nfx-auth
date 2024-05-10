using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nfx_auth.Data;
using nfx_auth.Models;
using nfx_auth.Utils;

namespace nfx_auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController(AuthDbContext dbContext, IJwtBuilder jwtBuilder, IEncryptor encryptor) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user, [FromQuery(Name = "d")] string destination = "frontend")
    {
        var u = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (u == null)
        {
            return NotFound("User not found.");
        }

        if (destination == "backend" && !u.IsAdmin)
        {
            return Unauthorized("Could not authenticate user.");
        }

        var isValid = u.ValidatePassword(user.Password, encryptor);

        if (!isValid)
        {
            return Unauthorized("Could not authenticate user.");
        }

        var token = jwtBuilder.GetToken(u.Email, u.IsAdmin);

        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var u = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (u != null)
        {
            return Unauthorized("User already exists.");
        }

        user.SetPassword(user.Password, encryptor);
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}