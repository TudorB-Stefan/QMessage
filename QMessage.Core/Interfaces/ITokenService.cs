using QMessage.Core.Entities;

namespace QMessage.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(User user);
}