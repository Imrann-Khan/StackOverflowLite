using FluentValidation;
using StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    public CreateAnswerCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MinimumLength(5);
    }
}
