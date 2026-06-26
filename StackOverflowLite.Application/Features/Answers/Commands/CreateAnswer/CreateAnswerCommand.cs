using MediatR;
using StackOverflowLite.Application.Features.Answers.Dtos;
using System;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public record CreateAnswerCommand(
    Guid QuestionId,
    string Content,
    string UserId) : IRequest<AnswerDto>;
