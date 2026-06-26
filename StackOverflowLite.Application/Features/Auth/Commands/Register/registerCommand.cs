using MediatR;
using StackOverflowLite.Application.Features.Auth.Dtos;

namespace StackOverflowLite.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password) : IRequest<AuthResponseDto>;
