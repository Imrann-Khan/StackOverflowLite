using MediatR;
using StackOverflowLite.Domain.Entities;
using System;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteQuestion;

public record VoteQuestionCommand(
    Guid QuestionId,
    string UserId,
    VoteType VoteType) : IRequest<bool>;
