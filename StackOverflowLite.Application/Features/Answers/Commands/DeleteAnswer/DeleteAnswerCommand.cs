using MediatR;
using System;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public record DeleteAnswerCommand(
    Guid AnswerId,
    string UserId) : IRequest<bool>;
