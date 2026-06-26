using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteQuestion;

public class VoteQuestionCommandHandler(IAppDbContext dbContext) : IRequestHandler<VoteQuestionCommand, bool>
{
    public async Task<bool> Handle(VoteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions.FindAsync(new object[] { request.QuestionId }, cancellationToken);
        if (question == null)
            throw new Exception("Question not found");

        var existingVote = await dbContext.Votes.FirstOrDefaultAsync(v => v.QuestionId == request.QuestionId && v.UserId == request.UserId, cancellationToken);
        if (existingVote != null)
            throw new Exception("User has already voted on this question");

        var vote = new Vote
        {
            Type = request.VoteType,
            UserId = request.UserId,
            QuestionId = request.QuestionId,
            CreatedAt = DateTime.UtcNow
        };
        dbContext.Votes.Add(vote);

        var author = await dbContext.Users.FindAsync(new object[] { question.AuthorId }, cancellationToken);
        if (author != null)
        {
            if (request.VoteType == VoteType.Upvote)
                author.Reputation += 5;
            else if (request.VoteType == VoteType.Downvote)
                author.Reputation -= 2;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
