using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using nfx_auth.Data;
using nfx_auth.Utils;

namespace nfx_auth.Services;

public class AuthService(AuthDbContext dbContext, IJwtBuilder jwtBuilder) : Auth.AuthBase
{
    public override async Task<ValidateResponse> Validate(ValidateRequest request, ServerCallContext context)
    {
        var validate = jwtBuilder.ValidateToken(request.Token);

        if (validate.Item1 == string.Empty)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token"));
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == validate.Item1);

        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token"));
        }

        return await Task.FromResult(new ValidateResponse
        {
            Email = user.Email,
            IsAdmin = user.IsAdmin
        });
    }
}
