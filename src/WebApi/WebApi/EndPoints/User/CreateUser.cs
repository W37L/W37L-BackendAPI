using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

[Authorize]
public class CreateUser
    : ApiEndpoint
        .WithRequest<CreateUser.CreateUserRequest>
        .WithResponse<CreateUser.CreateUserResponse> {
    private readonly ICommandDispatcher dispatcher;

    public CreateUser(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPost("user/create")]
    public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(Error.UnAuthorized);

        var cmd = CreateUserCommand.Create(
            request?.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Pub
        ).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new CreateUserResponse(cmd.Payload.UserId.Value))
            : BadRequest(result.Error);
    }

    public record CreateUserRequest(
        string UserId,
        string UserName,
        string FirstName,
        string LastName,
        string Email,
        string Pub);

    public record CreateUserResponse(string UserId);
}