using QMessage.Core.DTOs;
using QMessage.Core.Entities;
using QMessage.Core.Interfaces;

namespace QMessage.API.Extensions;

public static class UserExtensions
{
    public static async Task<UserDto> ToDto(this User user, ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ImageUrl = user.ImageUrl,
            Token = await tokenService.GenerateToken(user)
        };
    }
}