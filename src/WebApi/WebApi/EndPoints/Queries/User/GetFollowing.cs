using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting all followers of a user.
/// </summary>
public class GetFollowing : ApiEndpoint.WithoutRequest.WithResponse<GetFollowing.GetFollowingResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetFollowing"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetFollowing(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///     Handles the HTTP GET request to get all followers of a user.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("user/{userId}/following")]
    public override async Task<ActionResult<GetFollowingResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;

        var query = new GetFollowingQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetFollowingResponse(answer.Users);
    }

    /// <summary>
    ///   Represents a response after attempting to get all followers of a user.
    /// </summary>
    /// <param name="Following"></param>
    public record GetFollowingResponse(List<UserID> Following);
}