using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

/// <summary>
///  API endpoint for getting all comments by user ID.
/// </summary>
public class GetAllCommentsByUserId : ApiEndpoint.WithoutRequest.WithResponse<GetAllCommentsByUserId.GetAllCommentsByUserIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetAllCommentsByUserId"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetAllCommentsByUserId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all comments by user ID.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("comments/user/{userId}")]
    public override async Task<ActionResult<GetAllCommentsByUserIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["userId"];

        var userId = UserID.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllCommentsByUserIdQuery.Query(userId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetAllCommentsByUserIdQuery.Answer>(query);

            var response = new GetAllCommentsByUserIdResponse(answer.Comments);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get all comments by user ID.
    /// </summary>
    /// <param name="Comments"></param>
    public record GetAllCommentsByUserIdResponse(List<ContentDTO> Comments);
}