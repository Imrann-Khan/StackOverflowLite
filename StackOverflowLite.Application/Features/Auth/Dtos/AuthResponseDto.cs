namespace StackOverflowLite.Application.Features.Auth.Dtos;

public record AuthResponseDto(
    string Id,
    string UserName,
    string Email,
    string Token
);