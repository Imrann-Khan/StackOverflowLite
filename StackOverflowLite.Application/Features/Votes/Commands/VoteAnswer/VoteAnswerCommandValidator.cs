using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteAnswer;

public class VoteAnswerCommandValidator : AbstractValidator<VoteAnswerCommand>
{
    public VoteAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VoteType).IsInEnum();
    }
}
