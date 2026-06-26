using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Features.Questions.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQueryHandler(IAppDbContext dbContext, ICacheService cacheService) : IRequestHandler<GetQuestionsQuery, List<QuestionDto>>
{
    public async Task<List<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "questions_list";
        var cachedQuestions = await cacheService.GetAsync<List<QuestionDto>>(cacheKey);

        if (cachedQuestions != null)
        {
            return cachedQuestions;
        }

        var questions = await dbContext.Questions
            .Include(q => q.Author)
            .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);

        var dtos = questions.Select(q => new QuestionDto(
            q.Id,
            q.Title,
            q.Description,
            q.Author?.UserName ?? "Unknown",
            q.ViewCount,
            q.CreatedAt,
            q.QuestionTags.Select(qt => qt.Tag.Name).ToList()
        )).ToList();

        await cacheService.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(10));

        return dtos;
    }
}
