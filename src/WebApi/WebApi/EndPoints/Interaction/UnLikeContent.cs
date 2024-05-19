using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

/// <summary>
///  API endpoint for unliking content.
/// </summary>
[Authorize]
public class UnLikeContent :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UnLikeContent(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP POST request to unlike content.
    /// </summary>
    /// <param name="contentId">The content ID of the content to unlike.</param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/unlike/{contentId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var contentId = RouteData.Values["contentId"]?.ToString();

        if (string.IsNullOrEmpty(contentId)) return BadRequest(Error.ContentNotFound);

        var cmdResult = UnlikeContentCommand.Create(
            contentId,
            userId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error.Message);
    }
}