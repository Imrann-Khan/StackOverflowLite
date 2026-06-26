using System;
using System.Collections.Generic;

namespace StackOverflowLite.Application.Features.Questions.Dtos;

public record QuestionDto(
    Guid Id,
    string Title,
    string Content,
    string AuthorUsername,
    int ViewCount,
    DateTime CreatedAt,
    List<string> Tags);
