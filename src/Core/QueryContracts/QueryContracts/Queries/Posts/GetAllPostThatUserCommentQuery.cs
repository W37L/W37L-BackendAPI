using ObjectMapper.DTO;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries;

/// <summary>
///     Represents a query to retrieve all posts that a user has commented on.
/// </summary>
public class GetAllPostThatUserCommentQuery {
    public record Query(UserID UserId) : IQuery<Answer>;

    public record Answer(List<ContentDTO> Posts);
}