using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

/// <summary>
///  API endpoint for updating a user's profile banner.
/// </summary>
[Authorize]
public class UpdateProfileBanner : ApiEndpoint.WithRequest<UpdateProfileBanner.UpdateProfileBannerRequest>.WithResponse<UpdateProfileBanner.UpdateProfileBannerResponse> {
    private readonly ICommandDispatcher dispatcher;
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UpdateProfileBanner"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public UpdateProfileBanner(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    ///  Handles the HTTP PATCH request to update a user's profile banner.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPatch("user/update/banner")]
    public override async Task<ActionResult<UpdateProfileBannerResponse>> HandleAsync(
        UpdateProfileBannerRequest request) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || userId != request.UserId)
            return Unauthorized(Error.UnAuthorized);

        var cmd = UpdateProfileBannerCommand.Create(
            request?.UserId,
            request.Banner
        ).OnFailure(error => BadRequest(new UpdateProfileBannerResponse(false, error)));

        var result = await dispatcher.DispatchAsync(cmd.Payload);
        return result.IsSuccess
            ? Ok(new UpdateProfileBannerResponse(true, null))
            : BadRequest(new UpdateProfileBannerResponse(false, result.Error));
    }

    /// <summary>
    ///  Represents a request to update a user's profile banner.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="Banner"></param>
    public record UpdateProfileBannerRequest(
        string UserId,
        string Banner);

    /// <summary>
    ///  Represents a response after attempting to update a user's profile banner.
    /// </summary>
    /// <param name="Success"></param>
    /// <param name="error"></param>
    public record UpdateProfileBannerResponse(bool Success, Error error);
}