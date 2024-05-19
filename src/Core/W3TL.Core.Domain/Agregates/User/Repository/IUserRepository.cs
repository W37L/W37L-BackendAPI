using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Repository;

/// <summary>
/// Represents a repository for users.
/// </summary>
public interface IUserRepository : IRepository<global::User, UserID> {
    /// <summary>
    /// Updates a specific field of a user identified by their ID.
    /// </summary>
    Task<Result> UpdateFieldAsync(string userId, string fieldName, string fieldValue);

    /// <summary>
    /// Increments the follower count of the user identified by their ID.
    /// </summary>
    Task<Result> IncrementFollowersAsync(string userId);

    /// <summary>
    /// Decrements the follower count of the user identified by their ID.
    /// </summary>
    Task<Result> DecrementFollowersAsync(string userId);

    /// <summary>
    /// Increments the following count of the user identified by their ID.
    /// </summary>
    Task<Result> IncrementFollowingAsync(string userId);

    /// <summary>
    /// Decrements the following count of the user identified by their ID.
    /// </summary>
    Task<Result> DecrementFollowingAsync(string userId);

    /// <summary>
    /// Checks if a user with the given ID exists.
    /// </summary>
    Task<Result> ExistsAsync(UserID id);

    /// <summary>
    /// Retrieves a user's ID by their username.
    /// </summary>
    Task<Result<global::User>> GetIdByUsernameAsync(string username);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    Task<Result<global::User>> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    Task<Result<global::User>> GetByUserNameAsync(string userName);
}