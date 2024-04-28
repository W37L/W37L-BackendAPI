using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

public class UpdateUser : ApiEndpoint.WithRequest<UpdateUser.UpdateUserRequest>.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UpdateUser(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPut("user/update")]
    public override async Task<ActionResult> HandleAsync(UpdateUserRequest request) {
        var cmd = UpdateUserCommand.Create(
            request?.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request?.Bio!,
            request?.Location!,
            request?.Website!
        ).Payload;

        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record UpdateUserRequest(
        string UserId,
        string UserName,
        string FirstName,
        string LastName,
        string? Bio,
        string? Location,
        string? Website);
}