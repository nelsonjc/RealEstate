using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;

namespace RealEstate.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(User? User, string Message)> Authenticate(AuthDto auth);
        Task<string> GenerateToken(User user);
    }
}
