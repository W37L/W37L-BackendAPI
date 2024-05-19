using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for muting another user.
///</summary>
[Authorize]
public class Mute :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Mute(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to mute another user.
    ///</summary>
    /// <param name="userToMuteId">The user ID of the user to mute.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/mute/{userToMuteId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToMuteId = RouteData.Values["userToMuteId"]?.ToString();

        if (string.IsNullOrEmpty(userToMuteId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to mute themselves
        if (userId == userToMuteId) return BadRequest(Error.CannotMuteYourself);

        var cmdResult = MuteUserCommand.Create(
            userId,
            userToMuteId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}