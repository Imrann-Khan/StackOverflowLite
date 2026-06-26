using MediatR;
using StackOverflowLite.Application.Features.Answers.Dtos;
using System;

namespace StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;

public record AcceptAnswerCommand(
    Guid QuestionId,
    Guid AnswerId,
    string UserId) : IRequest<bool>;
