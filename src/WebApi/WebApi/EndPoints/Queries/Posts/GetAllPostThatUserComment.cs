using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

/// <summary>
///  API endpoint for getting all posts that a user commented on.
/// </summary>
public class GetAllPostThatUserComment : ApiEndpoint.WithoutRequest.WithResponse<GetAllPostThatUserComment.GetAllPostThatUserCommentResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetAllPostThatUserComment"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetAllPostThatUserComment(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all posts that a user commented on.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("posts/user/comments/{userId}")]
    public override async Task<ActionResult<GetAllPostThatUserCommentResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["userId"];

        var userId = UserID.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllPostThatUserCommentQuery.Query(userId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync(query);

            var response = new GetAllPostThatUserCommentResponse(answer.Posts);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get all posts that a user commented on.
    /// </summary>
    /// <param name="Posts"></param>
    public record GetAllPostThatUserCommentResponse(List<ContentDTO> Posts);
}