namespace StackOverflowLite.Application.Features.Auth.Dtos;

public record UserProfileDto(
    string Id,
    string Username,
    string Email,
    int Reputation,
    DateTime CreatedAt
);