using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

public class GetRelations
    : ApiEndpoint.WithoutRequest.WithResponse<GetRelations.GetRelationsResponse> {
    private readonly IQueryDispatcher _dispatcher;

    public GetRelations(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }

    [HttpGet("user/{userId}/relations")]
    public override async Task<ActionResult<GetRelationsResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;


        var query = new GetRelationsQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetRelationsResponse(answer.Interactions);
    }

    public record GetRelationsResponse(InteractionsDTO Interactions);
}