using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetFollowing
    : ApiEndpoint.WithoutRequest.WithResponse<GetFollowing.GetFollowingResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetFollowing(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("user/{userId}/following")]
    public override async Task<ActionResult<GetFollowingResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;

        var query = new GetFollowingQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetFollowingResponse(answer.Users);
    }

    public record GetFollowingResponse(List<UserID> Following);
}