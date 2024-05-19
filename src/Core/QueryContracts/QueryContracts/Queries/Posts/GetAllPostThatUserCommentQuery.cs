using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve all posts that a user has commented on.
/// </summary>
/// Usage:
/// var query = new GetAllPostThatUserCommentQuery.Query(userId);
/// var answer = await queryBus.DispatchAsync(query);
/// /
public class GetAllPostThatUserCommentQuery {
    
    /// <summary>
    ///  Represents a query to get all posts that a user has commented on.
    /// </summary>
    /// <param name="UserId"></param>
    public record Query(UserID UserId) : IQuery<Answer>;

    /// <summary>
    ///  Represents the answer returned by the GetAllPostThatUserCommentQuery.Query handler.
    /// </summary>
    /// <param name="Posts"></param>
    public record Answer(List<ContentDTO> Posts);
}