using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<DeleteAnswerCommand, bool>
{
    public async Task<bool> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await dbContext.Answers.FindAsync(new object[] { request.AnswerId }, cancellationToken);

        if (answer == null)
            throw new Exception("Answer not found");

        if (answer.AuthorId != request.UserId)
            throw new Exception("Only the author can delete this answer");

        if (answer.IsAccepted)
            throw new Exception("A deleted answer cannot be the accepted answer. Acceptance must be removed first.");

        dbContext.Answers.Remove(answer);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
