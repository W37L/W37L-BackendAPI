using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Agregates.User.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetUserByUsername :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<GetUserByUsername.GetUserByUsernameResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetUserByUsername(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("users/username/{Username}")]
    public override async Task<ActionResult<GetUserByUsernameResponse>> HandleAsync() {
        var username = UserNameType.Create(RouteData.Values["Username"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetUserByUsernameQuery.Query(username.Payload);
        var answer = await _dispatcher.DispatchAsync<GetUserByUsernameQuery.Answer>(query);

        if (answer.User is null)
            return NotFound();

        return new GetUserByUsernameResponse(answer.User);
    }


    public record GetUserByUsernameResponse(UserDTO User);
}