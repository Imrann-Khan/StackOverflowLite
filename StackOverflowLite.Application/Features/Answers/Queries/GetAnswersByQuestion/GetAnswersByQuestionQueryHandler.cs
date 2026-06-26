using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Features.Answers.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public class GetAnswersByQuestionQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAnswersByQuestionQuery, List<AnswerDto>>
{
    public async Task<List<AnswerDto>> Handle(GetAnswersByQuestionQuery request, CancellationToken cancellationToken)
    {
        var answers = await dbContext.Answers
            .Include(a => a.Author)
            .Where(a => a.QuestionId == request.QuestionId)
            .OrderByDescending(a => a.IsAccepted) // Accepted answer first
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return answers.Select(a => new AnswerDto(
            a.Id,
            a.Content,
            a.Author?.UserName ?? "Unknown",
            a.IsAccepted,
            a.CreatedAt,
            a.QuestionId
        )).ToList();
    }
}
