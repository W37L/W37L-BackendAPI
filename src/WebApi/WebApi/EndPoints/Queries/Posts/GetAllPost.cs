using Microsoft.AspNetCore.Mvc;
using ObjectMapper.DTO;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebApi.EndPoints.Common;

public class GetAllPost :
    ApiEndpoint
    .WithoutRequest
    .WithResponse<GetAllPost.GetAllPostResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetAllPost(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("posts")]
    public override async Task<ActionResult<GetAllPostResponse>> HandleAsync() {
        var query = new GetAllPostQuery.Query();

        var answer = await _dispatcher.DispatchAsync<GetAllPostQuery.Answer>(query);

        var response = new GetAllPostResponse(answer.Posts);

        foreach (var VARIABLE in answer.Posts) {
            Console.WriteLine(VARIABLE.UserId);
        }

        return Ok(response);
    }

    public record GetAllPostResponse(List<ContentDTO> Posts);
}