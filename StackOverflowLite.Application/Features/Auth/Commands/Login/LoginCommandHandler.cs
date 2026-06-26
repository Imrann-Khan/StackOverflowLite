using MediatR;
using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Features.Auth.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System.Security.Authentication;

namespace StackOverflowLite.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = jwtService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.UserName!, user.Email!, token);
    }
}
