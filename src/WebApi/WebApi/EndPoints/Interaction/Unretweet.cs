using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

/// <summary>
///  API endpoint for unretweeting a post.
/// </summary>
public class Unretweet :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Unretweet(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP POST request to unretweet a post.
    /// </summary>
    /// <param name="postId">The post ID of the post to unretweet.</param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/unretweet/{postId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var postId = RouteData.Values["postId"]?.ToString();

        if (string.IsNullOrEmpty(postId)) return BadRequest(Error.PostNotFound);

        var cmdResult = UnretweetPostCommand.Create(
            postId,
            userId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}