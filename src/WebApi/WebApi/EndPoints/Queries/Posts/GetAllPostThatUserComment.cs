using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

public class GetAllPostThatUserComment : ApiEndpoint.WithoutRequest.WithResponse<
    GetAllPostThatUserComment.GetAllPostThatUserCommentResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetAllPostThatUserComment(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("posts/user/comments/{userId}")]
    public override async Task<ActionResult<GetAllPostThatUserCommentResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["userId"];

        var userId = UserID.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllPostThatUserCommentQuery.Query(userId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync(query);

            var response = new GetAllPostThatUserCommentResponse(answer.Posts);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetAllPostThatUserCommentResponse(List<ContentDTO> Posts);
}