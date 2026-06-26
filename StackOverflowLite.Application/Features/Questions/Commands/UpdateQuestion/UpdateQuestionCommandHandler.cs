using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler(IAppDbContext dbContext) : IRequestHandler<UpdateQuestionCommand, bool>
{
    public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions
            .Include(q => q.QuestionTags)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (question == null)
            throw new Exception("Question not found");

        if (question.AuthorId != request.UserId)
            throw new Exception("Only the author can edit this question");

        question.Title = request.Title;
        question.Description = request.Content;
        question.UpdatedAt = DateTime.UtcNow;

        // Update tags
        dbContext.QuestionTags.RemoveRange(question.QuestionTags);

        foreach (var tagName in request.Tags)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                dbContext.Tags.Add(tag);
            }
            dbContext.QuestionTags.Add(new QuestionTag { QuestionId = question.Id, TagId = tag.Id });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
