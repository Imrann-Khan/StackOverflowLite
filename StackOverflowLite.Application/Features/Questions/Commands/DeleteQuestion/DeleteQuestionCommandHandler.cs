using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler(IAppDbContext dbContext) : IRequestHandler<DeleteQuestionCommand, bool>
{
    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions.FindAsync(new object[] { request.Id }, cancellationToken);

        if (question == null)
            throw new Exception("Question not found");

        if (question.AuthorId != request.UserId)
            throw new Exception("Only the author can delete this question");

        dbContext.Questions.Remove(question);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
