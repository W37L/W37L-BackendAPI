using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Post;

public class
    CreatePost : ApiEndpoint.WithRequest<CreatePost.CreatePostRequest>.WithResponse<CreatePost.CreatePostResponse> {
    private readonly ICommandDispatcher _dispatcher;

    public CreatePost(ICommandDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpPost("post/create")]
    public override async Task<ActionResult<CreatePostResponse>> HandleAsync(CreatePostRequest request) {
        var cmd = CreatePostCommand.Create(
            request?.PostId,
            request.Content,
            request.UserId,
            request.Signature,
            request.Type,
            request.MediaUrl!,
            request.MediaType!,
            request.ParentPostId!
        );

        if (cmd.IsFailure)
            return BadRequest(cmd.Error);


        var result = await _dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new CreatePostResponse(cmd.Payload.Id.Value))
            : BadRequest(result.Error.Message);
    }

    public record CreatePostRequest(
        string? PostId,
        string UserId,
        string Content,
        string Signature,
        string Type,
        string? MediaUrl,
        string? MediaType,
        string? ParentPostId);

    public record CreatePostResponse(string Id);
}