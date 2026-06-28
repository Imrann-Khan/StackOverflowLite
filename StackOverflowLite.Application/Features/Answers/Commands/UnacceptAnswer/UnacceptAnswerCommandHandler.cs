using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public class UnacceptAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<UnacceptAnswerCommand, bool>
{
    public async Task<bool> Handle(UnacceptAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        if (question.AuthorId != request.UserId)
            throw new UnauthorizedAccessException("Only the question author can unmark an answer");

        var answer = question.Answers.FirstOrDefault(a => a.Id == request.AnswerId);
        if (answer == null)
            throw new KeyNotFoundException("Answer not found");

        if (!answer.IsAccepted)
            throw new Exception("This answer is not currently accepted");

        answer.IsAccepted = false;
        question.AcceptedAnswerId = null;
        
        var author = await dbContext.Users.FindAsync(new object[] { answer.AuthorId }, cancellationToken);
        if (author != null)
        {
            author.Reputation -= 15;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
