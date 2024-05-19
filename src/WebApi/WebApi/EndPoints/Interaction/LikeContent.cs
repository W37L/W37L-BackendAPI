using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using W3TL.Core.Application.CommandDispatching.Commands.Post;
using W3TL.Core.Application.CommandDispatching.Dispatcher;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Interaction;

///<summary>
/// API endpoint for liking content.
///</summary>
[Authorize]
public class LikeContent :
    ApiEndpoint.WithoutRequest.WithoutResponse {
    private readonly ICommandDispatcher dispatcher;

    public LikeContent(ICommandDispatcher dispatcher) {
        this.dispatcher = dispatcher;
    }

    ///<summary>
    /// Handles the HTTP POST request to like content.
    ///</summary>
    /// <param name="contentId">The content ID of the content to like.</param>
    ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpPost("interaction/like/{contentId}")]
    public override async Task<ActionResult> HandleAsync() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return Unauthorized(Error.NotVerified);

        var contentId = RouteData.Values["contentId"]?.ToString();

        if (string.IsNullOrEmpty(contentId)) return BadRequest(Error.ContentNotFound);

        var cmdResult = LikeContentCommand.Create(contentId, userId).OnFailure(error => BadRequest(error));


        var result = await dispatcher.DispatchAsync(cmdResult.Payload);
        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error.Message);
    }
}