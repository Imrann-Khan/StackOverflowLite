using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Features.Answers.Dtos;
using StackOverflowLite.Application.Services.Interfaces;
using StackOverflowLite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandHandler(IAppDbContext dbContext) : IRequestHandler<CreateAnswerCommand, AnswerDto>
{
    public async Task<AnswerDto> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var questionExists = await dbContext.Questions.AnyAsync(q => q.Id == request.QuestionId, cancellationToken);
        if (!questionExists)
            throw new KeyNotFoundException("Question not found");

        var answer = new Answer
        {
            Content = request.Content,
            QuestionId = request.QuestionId,
            AuthorId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            IsAccepted = false
        };

        dbContext.Answers.Add(answer);
        await dbContext.SaveChangesAsync(cancellationToken);

        var author = await dbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

        return new AnswerDto(
            answer.Id,
            answer.Content,
            author?.UserName ?? "Unknown",
            answer.IsAccepted,
            answer.CreatedAt,
            answer.QuestionId
        );
    }
}
