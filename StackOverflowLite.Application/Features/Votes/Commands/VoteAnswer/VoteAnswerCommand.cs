using MediatR;
using StackOverflowLite.Domain.Entities;
using System;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteAnswer;

public record VoteAnswerCommand(
    Guid AnswerId,
    string UserId,
    VoteType VoteType) : IRequest<bool>;
