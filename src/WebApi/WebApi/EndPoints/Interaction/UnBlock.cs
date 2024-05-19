using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for unblocking another user.
///</summary>
[Authorize]
public class UnBlock :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UnBlock(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to unblock another user.
    ///</summary>
    /// <param name="userToUnblockId">The user ID of the user to unblock.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/unblock/{userToUnblockId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToUnblockId = RouteData.Values["userToUnblockId"]?.ToString();

        if (string.IsNullOrEmpty(userToUnblockId)) return BadRequest(Error.UserNotFound);

        // Ensure that the user is not trying to unblock themselves
        if (userId == userToUnblockId) return BadRequest(Error.CannotUnblockYourself);

        var cmdResult = UnblockUserCommand.Create(
            userId,
            userToUnblockId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}