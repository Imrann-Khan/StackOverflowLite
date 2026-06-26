using MediatR;
using StackOverflowLite.Application.Features.Auth.Dtos;

namespace StackOverflowLite.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Username,
    string Password) : IRequest<AuthResponseDto>;
