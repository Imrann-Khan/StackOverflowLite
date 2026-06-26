using MediatR;
using System;

namespace StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;

public record DeleteQuestionCommand(
    Guid Id,
    string UserId) : IRequest<bool>;
