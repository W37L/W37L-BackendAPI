using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

class InMemUserRepoStub : IUserRepository {
    // set up the in-memory database
    public readonly List<User> _users = new();

    public Task<Result> AddAsync(User aggregate) {
        _users.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(User aggregate) {
        // find the user in the list
        var existingUser = _users.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingUser == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        // update the user
        existingUser = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(UserID id) {
        // find the user in the list
        var existingUser = _users.FirstOrDefault(e => e.Id == id);
        if (existingUser == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        // delete the user
        _users.Remove(existingUser);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<User>> GetByIdAsync(UserID id) {
        // find the user in the list
        var existingUser = _users.FirstOrDefault(e => e.Id == id);
        if (existingUser == null) return Task.FromResult(Result<User>.Fail(Error.UserNotFound));

        return Task.FromResult(Result<User>.Success(existingUser));
    }

    public Task<Result<List<User>>> GetAllAsync() {
        return Task.FromResult(Result<List<User>>.Success(_users));
    }

    public Task<Result> UpdateFieldAsync(string userId, string fieldName, string fieldValue) {
        throw new NotImplementedException();
    }

    public Task<Result> IncrementFollowersAsync(string userId) {
        throw new NotImplementedException();
    }

    public Task<Result> DecrementFollowersAsync(string userId) {
        throw new NotImplementedException();
    }

    public Task<Result> IncrementFollowingAsync(string userId) {
        throw new NotImplementedException();
    }

    public Task<Result> DecrementFollowingAsync(string userId) {
        throw new NotImplementedException();
    }

    public Task<Result> ExistsAsync(UserID id) {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetIdByUsernameAsync(string username) {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetByEmailAsync(string email) {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetByUserNameAsync(string userName) {
        throw new NotImplementedException();
    }
}