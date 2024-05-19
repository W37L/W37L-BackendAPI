using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting a user by their ID.
/// </summary>
public class GetUserById : ApiEndpoint.WithoutRequest.WithResponse<GetUserById.GetUserByIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetUserById"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetUserById(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get a user by their ID.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("user/uid/{UserId}")]
    public override async Task<ActionResult<GetUserByIdResponse>> HandleAsync() {
        var uId = UserID.Create(RouteData.Values["UserId"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetUserByIdQuery.Query(uId.Payload);

        var answer = await _dispatcher.DispatchAsync<GetUserByIdQuery.Answer>(query);

        if (answer.User is null)
            return NotFound();

        return new GetUserByIdResponse(answer.User);
    }

    /// <summary>
    ///  Represents a response after attempting to get a user by their ID.
    /// </summary>
    /// <param name="User"></param>
    public record GetUserByIdResponse(UserDTO User);
}