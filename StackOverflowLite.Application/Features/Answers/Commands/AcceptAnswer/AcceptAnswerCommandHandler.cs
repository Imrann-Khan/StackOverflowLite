using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;

public class AcceptAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<AcceptAnswerCommand, bool>
{
    public async Task<bool> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        if (question.AuthorId != request.UserId)
            throw new UnauthorizedAccessException("Only the question author can accept an answer");

        var answer = question.Answers.FirstOrDefault(a => a.Id == request.AnswerId);
        if (answer == null)
            throw new KeyNotFoundException("Answer not found");

        if (answer.IsAccepted)
            return true; 

        var previousAccepted = question.Answers.FirstOrDefault(a => a.IsAccepted);
        if (previousAccepted != null)
        {
            previousAccepted.IsAccepted = false;
            var prevAuthor = await dbContext.Users.FindAsync(new object[] { previousAccepted.AuthorId }, cancellationToken);
            if (prevAuthor != null) prevAuthor.Reputation -= 15;
        }

        answer.IsAccepted = true;
        
        var author = await dbContext.Users.FindAsync(new object[] { answer.AuthorId }, cancellationToken);
        if (author != null)
        {
            author.Reputation += 15;
        }

        question.AcceptedAnswerId = answer.Id;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
