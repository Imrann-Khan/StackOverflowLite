using FluentValidation;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
{
    public DeleteAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
