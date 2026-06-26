using MediatR;
using StackOverflowLite.Application.Features.Questions.Dtos;
using System;
using System.Collections.Generic;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

public record GetQuestionsQuery(string? Tag = null) : IRequest<List<QuestionDto>>;
