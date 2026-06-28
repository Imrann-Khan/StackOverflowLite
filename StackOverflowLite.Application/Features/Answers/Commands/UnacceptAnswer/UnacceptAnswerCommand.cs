using MediatR;
using System;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public record UnacceptAnswerCommand(Guid QuestionId, Guid AnswerId, string UserId) : IRequest<bool>;
