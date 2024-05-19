using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

/// <summary>
///  API endpoint for getting all comments by post ID.
/// </summary>
public class GetAllCommentsByPostId
    : ApiEndpoint
        .WithoutRequest
        .WithResponse<GetAllCommentsByPostId.GetAllCommentsByPostIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetAllCommentsByPostId"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetAllCommentsByPostId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all comments by post ID.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("comments/post/{postId}")]
    public override async Task<ActionResult<GetAllCommentsByPostIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["postId"];

        var postId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllCommentsByPostIdQuery.Query(postId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetAllCommentsByPostIdQuery.Answer>(query);

            if (answer.Comments is null)
                return NotFound();

            var response = new GetAllCommentsByPostIdResponse(answer.Comments);

            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get all comments by post ID.
    /// </summary>
    /// <param name="Comments"></param>
    public record GetAllCommentsByPostIdResponse(List<ContentDTO> Comments);
}