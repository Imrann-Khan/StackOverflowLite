using MediatR;
using StackOverflowLite.Application.Features.Answers.Dtos;
using System;
using System.Collections.Generic;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public record GetAnswersByQuestionQuery(Guid QuestionId) : IRequest<List<AnswerDto>>;
