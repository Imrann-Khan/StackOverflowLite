using MediatR;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<UpdateAnswerCommand, bool>
{
    public async Task<bool> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await dbContext.Answers.FindAsync(new object[] { request.AnswerId }, cancellationToken);

        if (answer == null)
            throw new Exception("Answer not found");

        if (answer.AuthorId != request.UserId)
            throw new Exception("Only the author can edit this answer");

        answer.Content = request.Content;
        answer.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
