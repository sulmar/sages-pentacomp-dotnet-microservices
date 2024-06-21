using Auth.Api.Model;

namespace Auth.Api.Abstractions;

public interface IIdentityUserRepository
{
    Task<IdentityUser> GetAsync(string username);
}
