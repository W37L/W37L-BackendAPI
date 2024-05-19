using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Agregates.User.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries;

/// <summary>
///     API endpoint for getting all posts by username.
/// </summary>
public class GetPostsByUsername : ApiEndpoint.WithoutRequest.WithResponse<GetPostsByUsername.GetPostsByUsernameResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetPostsByUsername"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetPostsByUsername(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all posts by username.
    /// </summary>
    /// <returns></returns>
    [HttpGet("posts/user/username/{Username}")]
    public override async Task<ActionResult<GetPostsByUsernameResponse>> HandleAsync() {
        var username = UserNameType.Create(RouteData.Values["Username"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostsByUsernameQuery.Query(username.Payload.Value);

        try {
            var answer = await _dispatcher.DispatchAsync<GetPostsByUsernameQuery.Answer>(query);

            if (answer.Posts is null) {
                return NotFound();
            }

            return new GetPostsByUsernameResponse(answer.Posts);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    ///  Represents a response after attempting to get all posts by username.
    /// </summary>
    /// <param name="Posts"></param>
    public record GetPostsByUsernameResponse(List<ContentDTO> Posts);
}