using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.User;

public class
    UpdateProfileBanner : ApiEndpoint.WithRequest<UpdateProfileBanner.UpdateProfileBannerRequest>.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public UpdateProfileBanner(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    [HttpPatch("user/update/banner")]
    public override async Task<ActionResult> HandleAsync(UpdateProfileBannerRequest request) {
        var cmd = UpdateProfileBannerCommand.Create(
            request?.UserId,
            request.Banner
        ).Payload;

        var result = await dispatcher.DispatchAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }

    public record UpdateProfileBannerRequest(
        string UserId,
        string Banner);
}