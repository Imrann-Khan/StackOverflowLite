using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteQuestion;

public class VoteQuestionCommandValidator : AbstractValidator<VoteQuestionCommand>
{
    public VoteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VoteType).IsInEnum();
    }
}
