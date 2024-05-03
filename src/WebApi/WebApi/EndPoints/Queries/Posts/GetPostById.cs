using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
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

    [HttpGet("posts/user/id/{PostId}")]
    public override async Task<ActionResult<GetPostByIdResponse>> HandleAsync() {
        var fromRoute = RouteData.Values["PostId"];

        var uId = UserID.Create(fromRoute.ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostsByUserIdQuery.Query(uId.Payload);

        var answer = await _dispatcher.DispatchAsync<GetPostsByUserIdQuery.Answer>(query);

        if (answer.Posts is null)
            return NotFound();

        return new GetPostByIdResponse(answer.Posts);
    }

    public record GetPostByIdResponse(List<ContentDTO> Post);
}