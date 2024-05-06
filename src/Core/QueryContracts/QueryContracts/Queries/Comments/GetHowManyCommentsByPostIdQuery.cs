using QueryContracts.Contracts;

namespace QueryContracts.Queries.Comments;

public class GetHowManyCommentsByPostIdQuery {
    public record Query(PostId postId) : IQuery<Answer>;

    public record Answer(int Count);
}