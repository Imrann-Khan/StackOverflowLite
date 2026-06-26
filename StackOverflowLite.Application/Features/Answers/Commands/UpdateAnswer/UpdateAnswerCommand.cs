using MediatR;
using System;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public record UpdateAnswerCommand(
    Guid AnswerId,
    string Content,
    string UserId) : IRequest<bool>;
