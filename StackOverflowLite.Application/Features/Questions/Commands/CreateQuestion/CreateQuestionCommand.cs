using MediatR;
using StackOverflowLite.Application.Features.Questions.Dtos;

namespace StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(
    string Title,
    string Content,
    List<string> Tags,
    string UserId) : IRequest<QuestionDto>;
