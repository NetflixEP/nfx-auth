using Microsoft.EntityFrameworkCore;
using nfx_auth.Models;

namespace nfx_auth.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}