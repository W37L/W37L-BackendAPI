using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for reporting another user.
///</summary>
[Authorize]
public class Report :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public Report(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to report another user.
    ///</summary>
    /// <param name="userId">The user ID of the user to report.</param> 
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/report/{userId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var userToReportId = RouteData.Values["userId"]?.ToString();

        if (string.IsNullOrEmpty(userToReportId)) return BadRequest(Error.UserNotFound);

        // Ensure the user is not trying to report themselves
        if (userId == userToReportId) return BadRequest(Error.CannotReportYourself);

        var cmdResult = ReportUserCommand.Create(
            userId,
            userToReportId).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmdResult.Payload);

        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}