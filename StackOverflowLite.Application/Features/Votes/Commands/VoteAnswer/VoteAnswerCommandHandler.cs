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

        if (answer.AuthorId == request.UserId)
            throw new Exception("Users cannot vote on their own content");

        var existingVote = await dbContext.Votes.FirstOrDefaultAsync(v => v.AnswerId == request.AnswerId && v.UserId == request.UserId, cancellationToken);
        var author = await dbContext.Users.FindAsync(new object[] { answer.AuthorId }, cancellationToken);

        if (existingVote != null)
        {
            if (existingVote.Type == request.VoteType)
                throw new Exception("User has already voted on this answer with this vote type");

            if (author != null)
            {
                // Revert old vote
                if (existingVote.Type == VoteType.Upvote) author.Reputation -= 10;
                else if (existingVote.Type == VoteType.Downvote) author.Reputation += 2;

                // Apply new vote
                if (request.VoteType == VoteType.Upvote) author.Reputation += 10;
                else if (request.VoteType == VoteType.Downvote) author.Reputation -= 2;

                if (author.Reputation < 0) author.Reputation = 0;
            }

            existingVote.Type = request.VoteType;
        }
        else
        {
            var vote = new Vote
            {
                Type = request.VoteType,
                UserId = request.UserId,
                AnswerId = request.AnswerId,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Votes.Add(vote);

            if (author != null)
            {
                if (request.VoteType == VoteType.Upvote) author.Reputation += 10;
                else if (request.VoteType == VoteType.Downvote) author.Reputation -= 2;

                if (author.Reputation < 0) author.Reputation = 0;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
