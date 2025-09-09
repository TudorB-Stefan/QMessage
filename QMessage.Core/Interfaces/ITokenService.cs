using QMessage.Core.Entities;

namespace QMessage.Core.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}