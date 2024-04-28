using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Repository;

public interface IUserRepository : IRepository<global::User, UserID> {
    Task<Result> UpdateFieldAsync(string userId, string fieldName, string fieldValue);
    Task<Result> IncrementFollowersAsync(string userId);
    Task<Result> DecrementFollowersAsync(string userId);
    Task<Result> IncrementFollowingAsync(string userId);
    Task<Result> DecrementFollowingAsync(string userId);
    Task<Result> ExistsAsync(UserID id);
}