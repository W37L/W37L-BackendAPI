using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Agregates.User.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting a user by their username.
/// </summary>
public class GetUserByUsername : ApiEndpoint.WithoutRequest.WithResponse<GetUserByUsername.GetUserByUsernameResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetUserByUsername"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetUserByUsername(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get a user by their username.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("user/username/{Username}")]
    public override async Task<ActionResult<GetUserByUsernameResponse>> HandleAsync() {
        var username = UserNameType.Create(RouteData.Values["Username"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetUserByUsernameQuery.Query(username.Payload);
        var answer = await _dispatcher.DispatchAsync<GetUserByUsernameQuery.Answer>(query);

        if (answer.User is null)
            return NotFound();

        return new GetUserByUsernameResponse(answer.User);
    }
    
    /// <summary>
    ///     Represents a response after attempting to get a user by their username.
    /// </summary>
    /// <param name="User"></param>
    public record GetUserByUsernameResponse(UserDTO User);
}