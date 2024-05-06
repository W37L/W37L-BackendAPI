using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

[Authorize]
public class Unfollow :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Unfollow(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

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