using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Votes.Commands.VoteAnswer;

public class VoteAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<VoteAnswerCommand, bool>
{
    public async Task<bool> Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await dbContext.Answers.FindAsync(new object[] { request.AnswerId }, cancellationToken);
        if (answer == null)
            throw new Exception("Answer not found");

        var existingVote = await dbContext.Votes.FirstOrDefaultAsync(v => v.AnswerId == request.AnswerId && v.UserId == request.UserId, cancellationToken);
        if (existingVote != null)
            throw new Exception("User has already voted on this answer");

        var vote = new Vote
        {
            Type = request.VoteType,
            UserId = request.UserId,
            AnswerId = request.AnswerId,
            CreatedAt = DateTime.UtcNow
        };
        dbContext.Votes.Add(vote);

        var author = await dbContext.Users.FindAsync(new object[] { answer.AuthorId }, cancellationToken);
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
