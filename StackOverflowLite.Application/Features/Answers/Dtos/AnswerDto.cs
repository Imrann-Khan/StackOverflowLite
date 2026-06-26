using System;

namespace StackOverflowLite.Application.Features.Answers.Dtos;

public record AnswerDto(
    Guid Id,
    string Content,
    string AuthorUsername,
    bool IsAccepted,
    DateTime CreatedAt,
    Guid QuestionId);
