using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;
using StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;
using StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetQuestions()
    {
        return Ok(await mediator.Send(new GetQuestionsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionById(Guid id)
    {
        return Ok(await mediator.Send(new GetQuestionByIdQuery(id)));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new CreateQuestionCommand(request.Title, request.Content, request.Tags, userId);
        return Ok(await mediator.Send(command));
    }
}

public record CreateQuestionRequest(string Title, string Content, List<string> Tags);
