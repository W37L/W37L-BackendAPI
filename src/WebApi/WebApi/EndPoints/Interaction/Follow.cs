using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for following another user.
///</summary>
[Authorize] // Ensure the endpoint is secured and accessible only by authenticated users
public class Follow :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Follow(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to follow another user.
    ///</summary>
    /// <param name="userToFollowId">The user ID of the user to follow.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/follow/{userToFollowId}")]
    public override async Task<ActionResult> HandleAsync() {
        // Extract the user ID from the JWT token claims
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToFollowId = RouteData.Values["userToFollowId"]?.ToString();

        if (string.IsNullOrEmpty(userToFollowId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to follow themselves
        if (userId == userToFollowId) return BadRequest(Error.CannotFollowYourself);

        // Create the command to handle the follow action
        var cmdResult = FollowAUserCommand.Create(
            userId,
            userToFollowId).OnFailure(error => BadRequest(error));
        
        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}