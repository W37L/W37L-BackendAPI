using System.Reflection;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

class InMemUserRepoStub : IUserRepository {
    private readonly List<User> _users = new();

    public Task<Result> AddAsync(User aggregate) {
        _users.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(User aggregate) {
        var existingUser = _users.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingUser == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        var index = _users.IndexOf(existingUser);
        _users[index] = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(UserID id) {
        var userToRemove = _users.FirstOrDefault(u => u.Id == id);
        if (userToRemove == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        _users.Remove(userToRemove);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<User>> GetByIdAsync(UserID id) {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user != null ? Result<User>.Success(user) : Result<User>.Fail(Error.UserNotFound));
    }

    public Task<Result<List<User>>> GetAllAsync() {
        return Task.FromResult(Result<List<User>>.Success(_users));
    }

    public Task<Result> UpdateFieldAsync(string userId, string fieldName, string fieldValue) {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        // Get the property info from Profile type, considering case-insensitivity
        var userProperty = user.Profile.GetType()
            .GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (userProperty == null || !userProperty.CanWrite) return Task.FromResult(Result.Fail(Error.InvalidField));

        try {
            // Determine the type based on fieldName and create the corresponding type
            object valueToSet = null;
            switch (fieldName.ToLower()) {
                case "banner":
                    var bannerResult = BannerType.Create(fieldValue);
                    if (bannerResult.IsFailure) return Task.FromResult(Result.Fail(bannerResult.Error));
                    valueToSet = bannerResult.Payload;
                    break;
                case "avatar":
                    var avatarResult = AvatarType.Create(fieldValue);
                    if (avatarResult.IsFailure) return Task.FromResult(Result.Fail(avatarResult.Error));
                    valueToSet = avatarResult.Payload;
                    break;
                default:
                    return Task.FromResult(Result.Fail(Error.InvalidField));
            }

            // Set the value to the property
            userProperty.SetValue(user.Profile, valueToSet);
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex) {
            return Task.FromResult(Result.Fail(Error.InvalidField));
        }
    }

    public Task<Result> IncrementFollowersAsync(string userId) {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        user.Profile.Followers.Increment();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DecrementFollowersAsync(string userId) {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        user.Profile.Followers.Decrement();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> IncrementFollowingAsync(string userId) {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        user.Profile.Following.Increment();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DecrementFollowingAsync(string userId) {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return Task.FromResult(Result.Fail(Error.UserNotFound));

        user.Profile.Following.Decrement();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ExistsAsync(UserID id) {
        var exists = _users.Any(u => u.Id == id);
        return Task.FromResult(exists ? Result.Success() : Result.Fail(Error.UserNotFound));
    }

    public Task<Result<User>> GetIdByUsernameAsync(string username) {
        var user = _users.FirstOrDefault(u => u.UserName.Value == username);
        return Task.FromResult(user != null ? Result<User>.Success(user) : Result<User>.Fail(Error.UserNotFound));
    }

    public Task<Result<User>> GetByEmailAsync(string email) {
        var user = _users.FirstOrDefault(u => u.Email.Value == email);
        return Task.FromResult(user != null ? Result<User>.Success(user) : Result<User>.Fail(Error.UserNotFound));
    }

    public Task<Result<User>> GetByUserNameAsync(string userName) {
        var user = _users.FirstOrDefault(u => u.UserName.Value == userName);
        return Task.FromResult(user != null ? Result<User>.Success(user) : Result<User>.Fail(Error.UserNotFound));
    }
}