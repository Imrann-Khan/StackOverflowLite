using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Features.Questions.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler(IAppDbContext dbContext, ICacheService cacheService) : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
{
    public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await dbContext.Questions
            .Include(q => q.Author)
            .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        var viewKey = "question_" + request.Id + "_views";
        var views = await cacheService.IncrementAsync(viewKey);

        question.ViewCount = (int)views;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new QuestionDto(
            question.Id,
            question.Title,
            question.Description,
            question.Author?.UserName ?? "Unknown",
            question.ViewCount,
            question.CreatedAt,
            question.QuestionTags.Select(qt => qt.Tag.Name).ToList()
        );
    }
}
