using MediatR;
using StackOverflowLite.Application.Features.Questions.Dtos;
using System;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;

public record GetQuestionByIdQuery(Guid Id) : IRequest<QuestionDto>;
