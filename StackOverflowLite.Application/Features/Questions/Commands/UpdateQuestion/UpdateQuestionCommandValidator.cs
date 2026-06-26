using FluentValidation;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(10);
    }
}
