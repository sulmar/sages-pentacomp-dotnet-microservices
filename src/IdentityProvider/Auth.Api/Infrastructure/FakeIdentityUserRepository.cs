using Auth.Api.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class FakeIdentityUserRepository(IPasswordHasher<Model.IdentityUser> _passwordHasher) : IIdentityUserRepository
{
    public Task<Model.IdentityUser> GetAsync(string username)
    {
        var identity = new Model.IdentityUser { Username = "john", FirstName = "John", LastName = "Smith", Email = "john@domain.com"};

        identity.HashedPassword = _passwordHasher.HashPassword(identity, "123");

        return Task.FromResult(identity);
    }
}
