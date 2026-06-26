using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Features.Questions.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Linq;

namespace StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler(IAppDbContext dbContext, ICacheService cacheService) : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            Title = request.Title,
            Description = request.Content,
            AuthorId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            ViewCount = 0
        };

        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (request.Tags != null && request.Tags.Any())
        {
            var existingTags = await dbContext.Tags
                .Where(t => request.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);
            
            var existingTagNames = existingTags.Select(t => t.Name).ToList();
            var newTagNames = request.Tags.Except(existingTagNames).ToList();

            foreach (var newTagName in newTagNames)
            {
                var newTag = new Tag { Name = newTagName };
                dbContext.Tags.Add(newTag);
                existingTags.Add(newTag);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            foreach (var tag in existingTags)
            {
                dbContext.QuestionTags.Add(new QuestionTag
                {
                    QuestionId = question.Id,
                    TagId = tag.Id
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var author = await dbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

        var dto = new QuestionDto(
            question.Id,
            question.Title,
            question.Description,
            author?.UserName ?? "Unknown",
            question.ViewCount,
            question.CreatedAt,
            request.Tags ?? new List<string>()
        );

        await cacheService.RemoveAsync("questions_list");
        return dto;
    }
}
