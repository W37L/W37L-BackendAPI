using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries;

public class GetPostById : ApiEndpoint.WithoutRequest.WithResponse<GetPostById.GetPostByIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetPostById"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetPostById(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }
    
    /// <summary>
    ///  Handles the HTTP GET request to get a post by ID.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("posts/{PostId}")]
    public override async Task<ActionResult<GetPostByIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["PostId"];

        var pId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostByIdQuery.Query(pId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetPostByIdQuery.Answer>(query);

            if (answer.Post is null)
                return NotFound();

            return new GetPostByIdResponse(answer.Post);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get a post by ID.
    /// </summary>
    /// <param name="Post"></param>
    public record GetPostByIdResponse(ContentDTO Post);
}