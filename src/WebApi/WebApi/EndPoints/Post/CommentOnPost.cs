using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Post;

/// <summary>
///   API endpoint for commenting on a post.
/// </summary>
[Authorize]
public class CommentOnPost :
    ApiEndpoint
    .WithRequest<CommentOnPost.CommentOnPostRequest>
    .WithResponse<CommentOnPost.CommentOnPostResponse> {
    private readonly ICommandDispatcher _dispatcher;

    public CommentOnPost(ICommandDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///   Handles the HTTP POST request to comment on a post.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("post/{ParentPostId}/comment")]
    public override async Task<ActionResult<CommentOnPostResponse>> HandleAsync(CommentOnPostRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(Error.UnAuthorized);

        var parentPostId = RouteData.Values["ParentPostId"]?.ToString();

        var cmd = CommentPostCommand.Create(
            request?.CommentId,
            request.Content,
            userId,
            request.Signature,
            parentPostId
        ).OnFailure(error => BadRequest(error));

        var result = await _dispatcher.DispatchAsync(cmd.Payload);

        return result.IsSuccess
            ? Ok(new CommentOnPostResponse(cmd.Payload.Id.Value, true, null))
            : BadRequest(new CommentOnPostResponse(null, false, result.Error));
    }

    /// <summary>
    ///  Represents a request to comment on a post.
    /// </summary>
    /// <param name="CommentId"></param>
    /// <param name="Content"></param>
    /// <param name="Signature"></param>
    public record CommentOnPostRequest(
        string? CommentId,
        string Content,
        string Signature);

    /// <summary>
    ///  Represents a response after attempting to comment on a post.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Success"></param>
    /// <param name="error"></param>
    public record CommentOnPostResponse(string Id, bool Success, Error error);
}