using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for retweeting a post.
///</summary>
[Authorize]
public class Retweet :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Retweet(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to retweet a post.
    ///</summary>
    /// <param name="postId">The post ID of the post to retweet.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>

    [HttpPost("interaction/retweet/{postId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var postId = RouteData.Values["postId"]?.ToString();

        if (string.IsNullOrEmpty(postId)) return BadRequest(Error.PostNotFound);

        var cmdResult = RetweetPostCommand.Create(
            postId,
            userId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}