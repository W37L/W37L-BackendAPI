using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries.Comments;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.Comments;

public class
    GetAllCommentsByUserId : ApiEndpoint.WithoutRequest.WithResponse<
    GetAllCommentsByUserId.GetAllCommentsByUserIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetAllCommentsByUserId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("comments/user/{userId}")]
    public override async Task<ActionResult<GetAllCommentsByUserIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["userId"];

        var userId = UserID.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetAllCommentsByUserIdQuery.Query(userId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetAllCommentsByUserIdQuery.Answer>(query);

            var response = new GetAllCommentsByUserIdResponse(answer.Comments);
            return Ok(response);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetAllCommentsByUserIdResponse(List<ContentDTO> Comments);
}