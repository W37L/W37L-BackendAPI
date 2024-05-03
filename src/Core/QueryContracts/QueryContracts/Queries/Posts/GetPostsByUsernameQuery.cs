using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

public class GetPostsByUsernameQuery {
    public record Query(string Username) : IQuery<Answer>;

    public record Answer(
        List<ContentDTO> Posts);
}