using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

[Authorize]
public class UpdateAvatar
    : ApiEndpoint
        .WithRequest<UpdateAvatar.UpdateAvatarRequest>
        .WithResponse<UpdateAvatar.UpdateAvatarResponse> {
    private readonly ICommandDispatcher dispatcher;

    public UpdateAvatar(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPatch("user/update/avatar")]
    public override async Task<ActionResult<UpdateAvatarResponse>> HandleAsync(UpdateAvatarRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || userId != request.UserId)
            return Unauthorized(Error.UnAuthorized);

        var cmd = UpdateAvatarUserCommand.Create(
            request?.UserId,
            request.Avatar
        ).OnFailure(error => BadRequest(new UpdateAvatarResponse(false, error)));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new UpdateAvatarResponse(true, null))
            : BadRequest(new UpdateAvatarResponse(false, result.Error));
    }

    public record UpdateAvatarRequest(
        string UserId,
        string Avatar);

    public record UpdateAvatarResponse(bool Success, Error error);
}