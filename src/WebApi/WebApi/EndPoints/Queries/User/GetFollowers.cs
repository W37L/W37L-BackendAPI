using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetFollowers : ApiEndpoint.WithoutRequest.WithResponse<GetFollowers.GetFollowersResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetFollowers(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("user/{userId}/followers")]
    public override async Task<ActionResult<GetFollowersResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;

        var query = new GetFollowersQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetFollowersResponse(answer.Users);
    }

    public record GetFollowersResponse(List<UserID> Followers);
}