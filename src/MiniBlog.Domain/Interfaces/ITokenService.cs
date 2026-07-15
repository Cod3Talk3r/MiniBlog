using MiniBlog.Domain.Entities;

namespace MiniBlog.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}