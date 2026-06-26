using MediatR;
using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Features.Auth.Dtos;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Auth.Queries.GetProfile;

public record GetProfileQuery(string UserId) : IRequest<UserProfileDto>;

public class GetProfileHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId) ?? throw new KeyNotFoundException("User not found");

        return new UserProfileDto(user.Id, user.UserName!, user.Email!, user.Reputation, user.CreatedAt);
    }
}

