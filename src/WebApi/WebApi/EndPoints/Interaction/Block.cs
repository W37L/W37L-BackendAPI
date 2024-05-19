using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for blocking a user.
///</summary>
[Authorize]
public class Block : ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Block(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to block a user.
    ///</summary>
    /// <param name="userToBlockId">The user ID of the user to block.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/block/{userToBlockId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToBlockId = RouteData.Values["userToBlockId"]?.ToString();

        if (string.IsNullOrEmpty(userToBlockId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to block themselves
        if (userId == userToBlockId) return BadRequest(Error.CannotBlockYourself);

        var cmdResult = BlockUserCommand.Create(
            userId,
            userToBlockId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}