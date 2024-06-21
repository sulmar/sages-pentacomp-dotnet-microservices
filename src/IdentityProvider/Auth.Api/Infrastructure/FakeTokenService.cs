using Auth.Api.Abstractions;

namespace Auth.Api.Infrastructure;

public class FakeTokenService : ITokenService
{
    public string CreateToken(Model.IdentityUser identity) => "abc";
}