using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

[Authorize] // Ensure the endpoint is secured and accessible only by authenticated users
public class Follow :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Follow(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    // Endpoint to follow another user, only requires the ID of the user to follow
    [HttpPost("interaction/follow/{userToFollowId}")]
    public override async Task<ActionResult> HandleAsync() {
        // Extract the user ID from the JWT token claims
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified.Message);

        var userToFollowId = RouteData.Values["userToFollowId"]?.ToString();
        if (string.IsNullOrEmpty(userToFollowId)) return BadRequest(Error.UserNotFound.Message);

        // Create the command to handle the follow action
        var cmdResult = FollowAUserCommand.Create(
            userId,
            userToFollowId);

        if (cmdResult.IsFailure)
            return BadRequest(cmdResult.Error.Message);


        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error.Message);
    }
}