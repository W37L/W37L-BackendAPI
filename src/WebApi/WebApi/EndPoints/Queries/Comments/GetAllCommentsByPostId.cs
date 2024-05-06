using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

public class GetAllCommentsByPostId
    : ApiEndpoint
        .WithoutRequest
        .WithResponse<GetAllCommentsByPostId.GetAllCommentsByPostIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetAllCommentsByPostId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("comments/post/{postId}")]
    public override async Task<ActionResult<GetAllCommentsByPostIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["postId"];

        var postId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllCommentsByPostIdQuery.Query(postId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetAllCommentsByPostIdQuery.Answer>(query);

            if (answer.Comments is null)
                return NotFound();

            var response = new GetAllCommentsByPostIdResponse(answer.Comments);

            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetAllCommentsByPostIdResponse(List<ContentDTO> Comments);
}