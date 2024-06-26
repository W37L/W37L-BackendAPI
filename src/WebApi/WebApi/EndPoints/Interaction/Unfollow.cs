using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for unfollowing a user.
/// </summary>
[Authorize]
public class Unfollow :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Unfollow(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP POST request to unfollow a user.
    /// </summary>
    /// <param name="userToUnfollowId">The user ID of the user to unfollow.</param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/unfollow/{userToUnfollowId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToUnfollowId = RouteData.Values["userToUnfollowId"]?.ToString();

        if (string.IsNullOrEmpty(userToUnfollowId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to unfollow themselves
        if (userId == userToUnfollowId) return BadRequest(Error.CannotUnfollowYourself);

        var cmdResult = UnfollowAUserCommand.Create(
            userId,
            userToUnfollowId).OnFailure(error => BadRequest(error));


        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error.Message);
    }
}