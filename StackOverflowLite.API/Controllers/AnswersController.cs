using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;
using System;
using System.Threading.Tasks;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/questions/{questionId}/answers")]
public class AnswersController(ISender mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAnswer(Guid questionId, [FromBody] CreateAnswerRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new CreateAnswerCommand(questionId, request.Content, userId);
        return Ok(await mediator.Send(command));
    }

    [HttpPost("{answerId}/accept")]
    [Authorize]
    public async Task<IActionResult> AcceptAnswer(Guid questionId, Guid answerId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new AcceptAnswerCommand(questionId, answerId, userId);
        return Ok(await mediator.Send(command));
    }
}

public record CreateAnswerRequest(string Content);
