using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Post;

/// <summary>
///   API endpoint for creating a post.
/// </summary>
[Authorize]
public class
    CreatePost : ApiEndpoint.WithRequest<CreatePost.CreatePostRequest>.WithResponse<CreatePost.CreatePostResponse> {
    private readonly ICommandDispatcher _dispatcher;

    public CreatePost(ICommandDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP POST request to create a post.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("post/create")]
    public override async Task<ActionResult<CreatePostResponse>> HandleAsync(CreatePostRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(Error.UnAuthorized);

        var cmd = CreatePostCommand.Create(
            request?.PostId,
            request.Content,
            userId,
            request.Signature,
            request.Type,
            request.MediaUrl!,
            request.MediaType!,
            request.ParentPostId!
        ).OnFailure(error => BadRequest(error));


        var result = await _dispatcher.DispatchAsync(cmd.Payload);

        return result.IsSuccess
            ? Ok(new CreatePostResponse(cmd.Payload.Id.Value, true, null))
            : BadRequest(new CreatePostResponse(null, false, result.Error));
    }

    /// <summary>
    ///  Represents a request to create a post.
    /// </summary>
    /// <param name="PostId"></param>
    /// <param name="UserId"></param>
    /// <param name="Content"></param>
    /// <param name="Signature"></param>
    /// <param name="Type"></param>
    /// <param name="MediaUrl"></param>
    /// <param name="MediaType"></param>
    /// <param name="ParentPostId"></param>
    public record CreatePostRequest(
        string? PostId,
        string UserId,
        string Content,
        string Signature,
        string Type,
        string? MediaUrl,
        string? MediaType,
        string? ParentPostId);

    /// <summary>
    ///  Represents a response after attempting to create a post.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Success"></param>
    /// <param name="error"></param>
    public record CreatePostResponse(string Id, bool Success, Error error);
}