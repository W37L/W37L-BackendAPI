using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

public class GetHowManyCommentsByPostId : ApiEndpoint.WithoutRequest.WithResponse<
    GetHowManyCommentsByPostId.GetHowManyCommentsByPostIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetHowManyCommentsByPostId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("comments/post/count/{postId}")]
    public override async Task<ActionResult<GetHowManyCommentsByPostIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["postId"];

        var postId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetHowManyCommentsByPostIdQuery.Query(postId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetHowManyCommentsByPostIdQuery.Answer>(query);
            var response = new GetHowManyCommentsByPostIdResponse(answer.Count);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetHowManyCommentsByPostIdResponse(int Count);
}