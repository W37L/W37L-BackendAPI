using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

public class UpdateAvatar : ApiEndpoint.WithRequest<UpdateAvatar.UpdateAvatarRequest>.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UpdateAvatar(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPatch("user/update/avatar")]
    public override async Task<ActionResult> HandleAsync(UpdateAvatarRequest request) {
        var cmd = UpdateAvatarUserCommand.Create(
            request?.UserId,
            request.Avatar
        ).Payload;

        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record UpdateAvatarRequest(
        string UserId,
        string Avatar);
}