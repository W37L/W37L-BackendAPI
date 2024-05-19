using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

/// <summary>
///  API endpoint for unreporting a user.
/// </summary>
public class Unreport :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Unreport(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///   Handles the HTTP POST request to unreport a user.
    /// </summary>
    /// <param name="userId">The user ID of the user to unreport.</param>
    /// <returns> An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/unreport/{userId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToUnreportId = RouteData.Values["userId"]?.ToString();

        if (string.IsNullOrEmpty(userToUnreportId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to unreport themselves
        if (userId == userToUnreportId) return BadRequest(Error.CannotUnreportYourself);

        var cmdResult = UnReportUserCommand.Create(
            userId,
            userToUnreportId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}