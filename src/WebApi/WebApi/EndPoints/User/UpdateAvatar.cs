using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

/// <summary>
///  API endpoint for updating a user's avatar.
/// </summary>
[Authorize]
public class UpdateAvatar : ApiEndpoint.WithRequest<UpdateAvatar.UpdateAvatarRequest>.WithResponse<UpdateAvatar.UpdateAvatarResponse> {
    private readonly ICommandDispatcher dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="UpdateAvatar"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public UpdateAvatar(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP PATCH request to update a user's avatar.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
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

    /// <summary>
    ///  Represents a request to update a user's avatar.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="Avatar"></param>
    public record UpdateAvatarRequest(
        string UserId,
        string Avatar);

    /// <summary>
    ///  Represents a response after attempting to update a user's avatar.
    /// </summary>
    /// <param name="Success"></param>
    /// <param name="error"></param>
    public record UpdateAvatarResponse(bool Success, Error error);
}