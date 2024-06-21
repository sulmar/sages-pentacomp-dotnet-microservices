using Auth.Api.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Auth.Api.Infrastructure;

public class FakeTokenService : ITokenService
{
    public string CreateAccessToken(Model.IdentityUser identityUser) => "abc";
}



public class MyClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }
}
