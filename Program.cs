using Microsoft.EntityFrameworkCore;
using nfx_auth.Data;
using nfx_auth.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql("Host=localhost;Port=8006;Database=db;Username=root;Password=root"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
