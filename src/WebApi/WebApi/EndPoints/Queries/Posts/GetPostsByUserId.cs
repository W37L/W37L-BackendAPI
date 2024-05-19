using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries;

/// <summary>
///  API endpoint for getting all posts by user ID.
/// </summary>
public class GetPostsByUserId : ApiEndpoint.WithoutRequest.WithResponse<GetPostsByUserId.GetPostsByUserIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetPostsByUserId"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetPostsByUserId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all posts by user ID.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("posts/user/id/{UserId}")]
    public override async Task<ActionResult<GetPostsByUserIdResponse>> HandleAsync() {
        var uId = UserID.Create(RouteData.Values["UserId"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostsByUserIdQuery.Query(uId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetPostsByUserIdQuery.Answer>(query);

            if (answer.Posts is null)
                return NotFound();
            
            return new GetPostsByUserIdResponse(answer.Posts);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    ///  Represents a response after attempting to get all posts by user ID.
    /// </summary>
    /// <param name="Posts"></param>
    public record GetPostsByUserIdResponse(List<ContentDTO> Posts);
}