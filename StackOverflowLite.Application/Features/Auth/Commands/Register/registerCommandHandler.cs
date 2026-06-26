using MediatR;
using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Features.Auth.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        var token = jwtService.GenerateToken(user);
        return new AuthResponseDto(user.Id, user.UserName!, user.Email!, token);
    }
}
