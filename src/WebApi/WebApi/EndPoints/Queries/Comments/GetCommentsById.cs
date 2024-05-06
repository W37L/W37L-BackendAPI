using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Agregates.Post.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

public class GetCommentsById
    : ApiEndpoint
        .WithoutRequest
        .WithResponse<GetCommentsById.GetCommentsResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetCommentsById(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("comments/{commentId}")]
    public override async Task<ActionResult<GetCommentsResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["commentId"];

        var commentId = CommentId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetCommentByIdQuery.Query(commentId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetCommentByIdQuery.Answer>(query);

            if (answer.Comment is null)
                return NotFound();

            var response = new GetCommentsResponse(answer.Comment);

            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetCommentsResponse(ContentDTO Comment);
}