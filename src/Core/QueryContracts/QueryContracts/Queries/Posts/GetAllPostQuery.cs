using ObjectMapper.DTO;
using QueryContracts.Contracts;

namespace QueryContracts.Queries;

public class GetAllPostQuery {
    public record Query : IQuery<Answer>;

    public record Answer(List<ContentDTO> Posts);
}