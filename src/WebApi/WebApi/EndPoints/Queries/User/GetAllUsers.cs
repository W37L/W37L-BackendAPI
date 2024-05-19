using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting all users.
/// </summary>
public class GetAllUsers : ApiEndpoint.WithoutRequest.WithResponse<GetAllUsers.GetAllUsersResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetAllUsers"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetAllUsers(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP GET request to get all users.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("users")]
    public override async Task<ActionResult<GetAllUsersResponse>> HandleAsync() {
        try {
            var query = new GetAllUsersQuery.Query();
            var answer = await _dispatcher.DispatchAsync(query);

            if (answer.Users is null)
                return NotFound();

            var users = answer.Users;
            return new GetAllUsersResponse(answer.Users);
        }
        catch (Exception ex) {
            return StatusCode(500);
        }
    }

    /// <summary>
    ///  Represents a response after attempting to get all users.
    /// </summary>
    /// <param name="Users"></param>
    public record GetAllUsersResponse(List<UserDTO> Users);
}