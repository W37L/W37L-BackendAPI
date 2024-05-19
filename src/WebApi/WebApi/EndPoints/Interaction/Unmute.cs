using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

/// <summary>
///  API endpoint for unmuting a user.
/// </summary>
[Authorize]
public class Unmute :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Unmute(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///     Handles the HTTP POST request to unmute a user.
    /// </summary>
    /// <param name="userToUnmuteId">The user ID of the user to unmute.</param>
    /// <returns></returns>
    [HttpPost("interaction/unmute/{userToUnmuteId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToUnmuteId = RouteData.Values["userToUnmuteId"]?.ToString();

        if (string.IsNullOrEmpty(userToUnmuteId)) return BadRequest(Error.UserNotFound);

        // Ensure that the user is not trying to mute themselves
        if (userId == userToUnmuteId) return BadRequest(Error.CannotUnmuteYourself);

        var cmdResult = UnMuteUserCommand.Create(
            userId,
            userToUnmuteId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}