using Persistence.UserPersistence.Firebase;
using QueryContracts.Contracts;
using W3TL.Core.Domain.Common.Values;

namespace QueryContracts.Queries.Users;

public class GetRelationsQuery {
    public record Query(UserID UserID) : IQuery<Answer>;

    public record Answer(InteractionsDTO Interactions);
}