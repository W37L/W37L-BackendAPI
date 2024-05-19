using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting all followers of a user.
/// </summary>
public class GetFollowers : ApiEndpoint.WithoutRequest.WithResponse<GetFollowers.GetFollowersResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetFollowers"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetFollowers(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }
    
    /// <summary>
    ///  Handles the HTTP GET request to get all followers of a user.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("user/{userId}/followers")]
    public override async Task<ActionResult<GetFollowersResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;

        var query = new GetFollowersQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetFollowersResponse(answer.Users);
    }

    /// <summary>
    ///  Represents a response after attempting to get all followers of a user.
    /// </summary>
    /// <param name="Followers"></param>
    public record GetFollowersResponse(List<UserID> Followers);
}