using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetAllUsers : ApiEndpoint.WithoutRequest.WithResponse<GetAllUsers.GetAllUsersResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetAllUsers(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

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


    public record GetAllUsersResponse(List<UserDTO> Users);
}