using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries;

public class GetPostsByUserId :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<GetPostsByUserId.GetPostsByUserIdResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetPostsByUserId(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("posts/{UserId}")]
    public override async Task<ActionResult<GetPostsByUserIdResponse>> HandleAsync() {
        var uId = UserID.Create(RouteData.Values["UserId"].ToString())
            .OnFailure(error => BadRequest(error));

        var query = new GetPostsByUserIdQuery.Query(uId.Payload);

        var answer = await _dispatcher.DispatchAsync<GetPostsByUserIdQuery.Answer>(query);

        if (answer.Posts is null)
            return NotFound();


        return new GetPostsByUserIdResponse(answer.Posts);
    }

    public record GetPostsByUserIdResponse(List<ContentDTO> Posts);
}