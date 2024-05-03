using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetUserById :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<GetUserById.GetUserByIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetUserById(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("users/uid/{UserId}")]
    public override async Task<ActionResult<GetUserByIdResponse>> HandleAsync() {
        var uId = UserID.Create(RouteData.Values["UserId"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetUserByIdQuery.Query(uId.Payload);

        var answer = await _dispatcher.DispatchAsync<GetUserByIdQuery.Answer>(query);

        if (answer.User is null)
            return NotFound();


        return new GetUserByIdResponse(answer.User);
    }

    public record GetUserByIdResponse(UserDTO User);
}