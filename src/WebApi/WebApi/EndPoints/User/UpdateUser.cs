using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

// [Authorize]
public class UpdateUser :
    ApiEndpoint
    .WithRequest<UpdateUser.UpdateUserRequest>
    .WithResponse<UpdateUser.UpdateUserResponse> {
    private readonly ICommandDispatcher dispatcher;

    public UpdateUser(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPut("user/update")]
    public override async Task<ActionResult<UpdateUserResponse>> HandleAsync(UpdateUserRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || userId != request.UserId)
            return Unauthorized(Error.UnAuthorized);

        var cmd = UpdateUserCommand.Create(
            request?.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request?.Bio!,
            request?.Location!,
            request?.Website!
        ).OnFailure(error => BadRequest(new UpdateUserResponse(false, error)));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new UpdateUserResponse(true, null))
            : BadRequest(new UpdateUserResponse(false, result.Error));
    }

    public record UpdateUserResponse(bool Success, Error error);

    public record UpdateUserRequest(
        string UserId,
        string UserName,
        string FirstName,
        string LastName,
        string? Bio,
        string? Location,
        string? Website);
}