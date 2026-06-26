using MediatR;
using System;
using System.Collections.Generic;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid Id,
    string Title,
    string Content,
    List<string> Tags,
    string UserId) : IRequest<bool>;
