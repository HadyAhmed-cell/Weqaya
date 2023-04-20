using Microsoft.AspNetCore.Identity;
using VirtualClinic.Identity;

namespace VirtualClinic.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user, UserManager<AppUser> manager);
    }
}