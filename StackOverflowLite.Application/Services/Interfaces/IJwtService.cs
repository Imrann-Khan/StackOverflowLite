using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Services.Interfaces;


public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}
