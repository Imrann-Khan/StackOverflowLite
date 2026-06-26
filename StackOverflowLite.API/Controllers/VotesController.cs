using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Votes.Commands.VoteQuestion;
using StackOverflowLite.Application.Features.Votes.Commands.VoteAnswer;
using StackOverflowLite.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api")]
public class VotesController(ISender mediator) : ControllerBase
{
    [HttpPost("questions/{questionId}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteQuestion(Guid questionId, [FromBody] VoteRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new VoteQuestionCommand(questionId, userId, request.VoteType);
        return Ok(await mediator.Send(command));
    }

    [HttpPost("answers/{answerId}/vote")]
    [Authorize]
    public async Task<IActionResult> VoteAnswer(Guid answerId, [FromBody] VoteRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new VoteAnswerCommand(answerId, userId, request.VoteType);
        return Ok(await mediator.Send(command));
    }
}

public record VoteRequest(VoteType VoteType);
