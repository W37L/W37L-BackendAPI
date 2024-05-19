using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

/// <summary>
///  API endpoint for getting all posts.
/// </summary>
public class GetAllPost : ApiEndpoint.WithoutRequest.WithResponse<GetAllPost.GetAllPostResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetAllPost"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetAllPost(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///     Handles the HTTP GET request to get all posts.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("posts")]
    public override async Task<ActionResult<GetAllPostResponse>> HandleAsync() {
        var query = new GetAllPostQuery.Query();

        var answer = await _dispatcher.DispatchAsync<GetAllPostQuery.Answer>(query);

        try {
            var response = new GetAllPostResponse(answer.Posts);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get all posts.
    /// </summary>
    /// <param name="Posts"></param>
    public record GetAllPostResponse(List<ContentDTO> Posts);
}