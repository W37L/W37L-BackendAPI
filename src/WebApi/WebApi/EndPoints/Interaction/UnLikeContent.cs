using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

[Authorize]
public class UnLikeContent :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UnLikeContent(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPost("interaction/unlike/{contentId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified.Message);

        var contentId = RouteData.Values["contentId"]?.ToString();
        if (string.IsNullOrEmpty(contentId)) return BadRequest(Error.ContentNotFound.Message);

        var cmdResult = UnlikeContentCommand.Create(
            contentId,
            userId);

        if (cmdResult.IsFailure)
            return BadRequest(cmdResult.Error.Message);

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error.Message);
    }
}