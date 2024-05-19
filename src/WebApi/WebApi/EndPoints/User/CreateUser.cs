using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

/// <summary>
///   API endpoint for creating a user.
/// </summary>
[Authorize]
public class CreateUser : ApiEndpoint.WithRequest<CreateUser.CreateUserRequest>.WithResponse<CreateUser.CreateUserResponse> {
    private readonly ICommandDispatcher dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="CreateUser"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public CreateUser(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP POST request to create a user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("user/create")]
    public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(Error.UnAuthorized);

        var cmd = CreateUserCommand.Create(
            request?.UserId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Pub
        ).OnFailure(error => BadRequest(error));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new CreateUserResponse(cmd.Payload.UserId.Value))
            : BadRequest(result.Error);
    }

    /// <summary>
    ///  Represents a request to create a user.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="UserName"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Email"></param>
    /// <param name="Pub"></param>
    public record CreateUserRequest(
        string UserId,
        string UserName,
        string FirstName,
        string LastName,
        string Email,
        string Pub);

    /// <summary>
    ///  Represents a response after attempting to create a user.
    /// </summary>
    /// <param name="UserId"></param>
    public record CreateUserResponse(string UserId);
}