using Google.Apis.Util;
using Google.Cloud.Firestore;
using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Persistence.UserPersistence;

public class UserRepository(IMapper mapper) : IUserRepository {
    private readonly IMapper _mapper = mapper;


    private readonly FirestoreDb db = FirebaseInitializer.FirestoreDb;

    public async Task<Result> AddAsync(User aggregate) {
        UserDTO firebaseUser;

        try {
            firebaseUser = _mapper.Map<UserDTO>(aggregate);
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }

        // Convert FirebaseUser to a dictionary for Firestore upload
        var userDict = ConvertToDictionary(firebaseUser);

        var docRef = db.Collection("users").Document(aggregate.Id.Value);
        await docRef.SetAsync(userDict);
        return Result.Success();
    }


    public async Task<Result> UpdateAsync(User user) {
        UserDTO firebaseUser;
        try {
            firebaseUser = _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }

        var userDict = ConvertToDictionary(firebaseUser);

        // Get Firestore document reference
        var docRef = db.Collection("users").Document(user.Id.Value);

        try {
            // Set the document with the new user data
            await docRef.SetAsync(userDict, SetOptions.Overwrite);
            return Result.Success();
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }

    public async Task<Result> UpdateFieldAsync(string userId, string fieldName, string fieldValue) {
        var docRef = db.Collection("users").Document(userId);

        try {
            // Create a dictionary to specify which field to update
            var updates = new Dictionary<string, object> {
                { fieldName, fieldValue }
            };

            // Update the specific field in the Firestore document
            await docRef.UpdateAsync(updates);
            return Result.Success();
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }

    public Task<Result> IncrementFollowersAsync(string userId) {
        var docRef = db.Collection("users").Document(userId);
        var updates = new Dictionary<string, object> {
            { "followersCount", FieldValue.Increment(1) }
        };
        return docRef.UpdateAsync(updates).ContinueWith(t => {
            if (t.IsFaulted) return Error.FromException(t.Exception);
            return Result.Success();
        });
    }

    public Task<Result> DecrementFollowersAsync(string userId) {
        var docRef = db.Collection("users").Document(userId);
        var updates = new Dictionary<string, object> {
            { "followersCount", FieldValue.Increment(-1) }
        };
        return docRef.UpdateAsync(updates).ContinueWith(t => {
            if (t.IsFaulted) return Error.FromException(t.Exception);
            return Result.Success();
        });
    }

    public Task<Result> IncrementFollowingAsync(string userId) {
        var docRef = db.Collection("users").Document(userId);
        var updates = new Dictionary<string, object> {
            { "followingCount", FieldValue.Increment(1) }
        };
        return docRef.UpdateAsync(updates).ContinueWith(t => {
            if (t.IsFaulted) return Error.FromException(t.Exception);
            return Result.Success();
        });
    }

    public Task<Result> DecrementFollowingAsync(string userId) {
        var docRef = db.Collection("users").Document(userId);
        var updates = new Dictionary<string, object> {
            { "followingCount", FieldValue.Increment(-1) }
        };
        return docRef.UpdateAsync(updates).ContinueWith(t => {
            if (t.IsFaulted) return Error.FromException(t.Exception);
            return Result.Success();
        });
    }


    public Task<Result> DeleteAsync(UserID id) {
        throw new NotImplementedException();
    }

    public async Task<Result> ExistsAsync(UserID id) {
        var docRef = db.Collection("users").Document(id.Value);
        var snapshot = await docRef.GetSnapshotAsync();
        return snapshot.Exists ? Result.Success() : Error.UserNotFound;
    }

    public async Task<Result<User>> GetIdByUsernameAsync(string username) {
        var query = db.Collection("users").WhereEqualTo("username", username);
        var querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count == 0)
            return Error.UserNotFound;


        var userSnapshot = querySnapshot.Documents.First();
        var userDto = userSnapshot.ConvertTo<UserDTO>();
        var user = _mapper.Map<User>(userDto);
        return user;
    }

    public async Task<Result<User>> GetByEmailAsync(string email) {
        var query = db.Collection("users").WhereEqualTo("email", email);
        var querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count == 0)
            return Error.UserNotFound;

        var userSnapshot = querySnapshot.Documents.First();
        var userDto = userSnapshot.ConvertTo<UserDTO>();
        var user = _mapper.Map<User>(userDto);
        return user;
    }

    public async Task<Result<User>> GetByUserNameAsync(string userName) {
        var query = db.Collection("users").WhereEqualTo("username", userName);
        var querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count == 0)
            return Error.UserNotFound;

        var userSnapshot = querySnapshot.Documents.First();

        var userDto = userSnapshot.ConvertTo<UserDTO>();
        var user = _mapper.Map<User>(userDto);
        return user;
    }


    public async Task<Result<User>> GetByIdAsync(UserID id) {
        var userDocRef = db.Collection("users").Document(id.Value);
        var userSnapshot = await userDocRef.GetSnapshotAsync();
        if (!userSnapshot.Exists) return Error.UserNotFound;

        var firebaseUser = userSnapshot.ConvertTo<UserDTO>();
        var interactionsDocRef = userDocRef.Collection("interactions").Document("data");
        var interactionsSnapshot = await interactionsDocRef.GetSnapshotAsync();
        if (interactionsSnapshot.Exists)
            firebaseUser.Interactions = interactionsSnapshot.ConvertTo<InteractionsDTO>();
        else
            // Handle the case where no interactions are found, could initialize to default or handle as an error
            firebaseUser.Interactions = new InteractionsDTO();

        return _mapper.Map<User>(firebaseUser);
    }


    public async Task<Result<List<User>>> GetAllAsync() {
        var querySnapshot = await db.Collection("users").GetSnapshotAsync();
        var users = querySnapshot.Documents.Select(doc => _mapper.Map<User>(doc.ConvertTo<UserDTO>())).ToList();
        return users;
    }

// Helper method to convert an object to a dictionary using reflection and Firestore attributes
    private static IDictionary<string, object> ConvertToDictionary(UserDTO firebaseUser) {
        var dict = new Dictionary<string, object>();
        foreach (var prop in typeof(UserDTO).GetProperties()) {
            var attr = prop.GetCustomAttribute<FirestorePropertyAttribute>();
            if (attr != null) {
                var value = prop.GetValue(firebaseUser);
                if (value != null)
                    // Add the FirestoreProperty name as the key
                    dict[attr.Name] = value;
            }
        }

        return dict;
    }
}