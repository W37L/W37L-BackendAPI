using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

[Authorize]
public class
    UpdateProfileBanner
    : ApiEndpoint
        .WithRequest<UpdateProfileBanner.UpdateProfileBannerRequest>
        .WithResponse<UpdateProfileBanner.UpdateProfileBannerResponse> {
    private readonly ICommandDispatcher dispatcher;

    public UpdateProfileBanner(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

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

    public record UpdateProfileBannerRequest(
        string UserId,
        string Banner);

    public record UpdateProfileBannerResponse(bool Success, Error error);
}