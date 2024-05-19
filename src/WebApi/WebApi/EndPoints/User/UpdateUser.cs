using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

/// <summary>
///  API endpoint for updating a user.
/// </summary>
// [Authorize]
public class UpdateUser : ApiEndpoint.WithRequest<UpdateUser.UpdateUserRequest>.WithResponse<UpdateUser.UpdateUserResponse> {
    private readonly ICommandDispatcher dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="UpdateUser"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public UpdateUser(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }
    
    /// <summary>
    ///  Handles the HTTP PUT request to update a user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPut("user/update")]
    public override async Task<ActionResult<UpdateUserResponse>> HandleAsync(UpdateUserRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || userId != request.UserId)
            return Unauthorized(Error.UnAuthorized);

        var cmd = UpdateUserCommand.Create(
            request?.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request?.Bio!,
            request?.Location!,
            request?.Website!
        ).OnFailure(error => BadRequest(new UpdateUserResponse(false, error)));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new UpdateUserResponse(true, null))
            : BadRequest(new UpdateUserResponse(false, result.Error));
    }

    /// <summary>
    ///  Represents a request to update a user.
    /// </summary>
    /// <param name="Success"></param>
    /// <param name="error"></param>
    public record UpdateUserResponse(bool Success, Error error);

    /// <summary>
    ///  Represents a response after attempting to update a user.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="UserName"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Bio"></param>
    /// <param name="Location"></param>
    /// <param name="Website"></param>
    public record UpdateUserRequest(
        string UserId,
        string UserName,
        string FirstName,
        string LastName,
        string? Bio,
        string? Location,
        string? Website);
}