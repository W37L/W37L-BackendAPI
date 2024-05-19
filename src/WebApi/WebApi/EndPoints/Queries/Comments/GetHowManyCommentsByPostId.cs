using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

/// <summary>
///   API endpoint for getting how many comments are associated with a post.
/// </summary>
public class GetHowManyCommentsByPostId : ApiEndpoint.WithoutRequest.WithResponse<GetHowManyCommentsByPostId.GetHowManyCommentsByPostIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetHowManyCommentsByPostId"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetHowManyCommentsByPostId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get how many comments are associated with a post.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("comments/post/count/{postId}")]
    public override async Task<ActionResult<GetHowManyCommentsByPostIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["postId"];

        var postId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetHowManyCommentsByPostIdQuery.Query(postId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetHowManyCommentsByPostIdQuery.Answer>(query);
            var response = new GetHowManyCommentsByPostIdResponse(answer.Count);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get how many comments are associated with a post.
    /// </summary>
    /// <param name="Count"></param>
    public record GetHowManyCommentsByPostIdResponse(int Count);
}