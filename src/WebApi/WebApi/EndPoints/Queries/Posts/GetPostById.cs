using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries;

public class GetPostById :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<GetPostById.GetPostByIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetPostById(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("posts/{PostId}")]
    public override async Task<ActionResult<GetPostByIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["PostId"];

        var pId = PostId.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostByIdQuery.Query(pId.Payload);

        try {
            var answer = await _dispatcher.DispatchAsync<GetPostByIdQuery.Answer>(query);

            if (answer.Post is null)
                return NotFound();

            return new GetPostByIdResponse(answer.Post);
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    public record GetPostByIdResponse(ContentDTO Post);
}