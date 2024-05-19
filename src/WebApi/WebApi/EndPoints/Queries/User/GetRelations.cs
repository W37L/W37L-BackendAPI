using Microsoft.AspNetCore.Mvc;
using Persistence.UserPersistence.Firebase;
using QueryContracts.Queries.Users;
using QueryContracts.QueryDispatching;
using W3TL.Core.Domain.Common.Values;
using WebApi.EndPoints.Common;

namespace WebApi.EndPoints.Queries.User;

/// <summary>
///  API endpoint for getting all relations of a user.
/// </summary>
public class GetRelations : ApiEndpoint.WithoutRequest.WithResponse<GetRelations.GetRelationsResponse> {
    private readonly IQueryDispatcher _dispatcher;

    /// <summary>
    ///  Initializes a new instance of the <see cref="GetRelations"/> class.
    /// </summary>
    /// <param name="dispatcher"></param>
    public GetRelations(IQueryDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }
    
    /// <summary>
    ///  Handles the HTTP GET request to get all relations of a user.
    /// </summary>
    /// <returns>An asynchronous task that represents the operation and contains the action result.</returns>
    [HttpGet("user/{userId}/relations")]
    public override async Task<ActionResult<GetRelationsResponse>> HandleAsync() {
        var userId = UserID.Create(RouteData.Values["userId"].ToString())
            .OnFailure(error => BadRequest(error)).Payload;


        var query = new GetRelationsQuery.Query(userId);
        var answer = await _dispatcher.DispatchAsync(query);

        return new GetRelationsResponse(answer.Interactions);
    }

    /// <summary>
    ///  Represents a response after attempting to get all relations of a user.
    /// </summary>
    /// <param name="Interactions"></param>
    public record GetRelationsResponse(InteractionsDTO Interactions);
}