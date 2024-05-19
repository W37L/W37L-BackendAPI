using Google.Apis.Util;
using Google.Cloud.Firestore;
using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Persistence.UserPersistence;

/// <summary>
///  Implementation of the user repository interface for interacting with user data in Firestore.
/// </summary>
/// <param name="mapper"></param>
public class UserRepository(IMapper mapper) : IUserRepository {
    private readonly IMapper _mapper = mapper;
    private readonly FirestoreDb db = FirebaseInitializer.FirestoreDb;

    ///<summary>
    /// Add a new user asynchronously.
    ///</summary>
    ///<param name="aggregate">User object to be added.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public async Task<Result> AddAsync(User aggregate) {
        UserDTO firebaseUser;

        try {
            firebaseUser = _mapper.Map<UserDTO>(aggregate);
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }

        var userDict = ConvertToDictionary(firebaseUser);
        var docRef = db.Collection("users").Document(aggregate.Id.Value);

        try {
            // Start a Firestore batch to handle multiple write operations atomically.
            var batch = db.StartBatch();

            // Set the user document
            batch.Set(docRef, userDict);

            // Create an interactions document with initial empty lists
            var interactionsDict = new Dictionary<string, object> {
                { "blockedUsers", new List<string>() },
                { "followers", new List<string>() },
                { "following", new List<string>() },
                { "highlightedTweetIds", new List<string>() },
                { "likedTweetIds", new List<string>() },
                { "mutedUsers", new List<string>() },
                { "reportedUsers", new List<string>() },
                { "retweetedTweetIds", new List<string>() }
            };
            // Add the interactions subdocument
            var interactionsRef = docRef.Collection("interactions").Document("data");
            batch.Set(interactionsRef, interactionsDict);

            // Commit the batch
            await batch.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex) {
            return Error.FromException(ex);
        }
    }

    ///<summary>
    /// Update an existing user asynchronously.
    ///</summary>
    ///<param name="user">Updated user object.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Update a specific field of a user asynchronously.
    ///</summary>
    ///<param name="userId">ID of the user whose field is to be updated.</param>
    ///<param name="fieldName">Name of the field to be updated.</param>
    ///<param name="fieldValue">New value for the field.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Increment the followers count of a user asynchronously.
    ///</summary>
    ///<param name="userId">ID of the user whose followers count is to be incremented.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Decrement the followers count of a user asynchronously.
    ///</summary>
    ///<param name="userId">ID of the user whose followers count is to be decremented.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Increment the following count of a user asynchronously.
    ///</summary>
    ///<param name="userId">ID of the user whose following count is to be incremented.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Decrement the following count of a user asynchronously.
    ///</summary>
    ///<param name="userId">ID of the user whose following count is to be decremented.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Delete a user asynchronously (Not Implemented).
    ///</summary>
    ///<param name="id">ID of the user to be deleted.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public Task<Result> DeleteAsync(UserID id) {
        throw new NotImplementedException();
    }

    ///<summary>
    /// Check if a user exists asynchronously.
    ///</summary>
    ///<param name="id">ID of the user to check.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public async Task<Result> ExistsAsync(UserID id) {
        var docRef = db.Collection("users").Document(id.Value);
        var snapshot = await docRef.GetSnapshotAsync();
        return snapshot.Exists ? Result.Success() : Error.UserNotFound;
    }

    ///<summary>
    /// Get user ID by username asynchronously.
    ///</summary>
    ///<param name="username">Username of the user.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Get user by email asynchronously.
    ///</summary>
    ///<param name="email">Email of the user.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public async Task<Result<User>> GetByEmailAsync(string email) {
        var query = db.Collection("users").WhereEqualTo("email", email);
        var querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count == 0)
            return Error.UserNotFound;

        var userSnapshot = querySnapshot.Documents.First();
        var userDto = userSnapshot.ConvertTo<UserDTO>();

        // Retrieve interactions
        var interactionsDocRef = userSnapshot.Reference.Collection("interactions").Document("data");
        var interactionsSnapshot = await interactionsDocRef.GetSnapshotAsync();
        if (interactionsSnapshot.Exists)
            userDto.Interactions = interactionsSnapshot.ConvertTo<InteractionsDTO>();
        else
            userDto.Interactions = new InteractionsDTO();

        var user = _mapper.Map<User>(userDto);
        return user;
    }

    ///<summary>
    /// Get user by username asynchronously.
    ///</summary>
    ///<param name="userName">Username of the user.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public async Task<Result<User>> GetByUserNameAsync(string userName) {
        var query = db.Collection("users").WhereEqualTo("username", userName);
        var querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count == 0)
            return Error.UserNotFound;

        var userSnapshot = querySnapshot.Documents.First();
        var userDto = userSnapshot.ConvertTo<UserDTO>();

        // Retrieve interactions
        var interactionsDocRef = userSnapshot.Reference.Collection("interactions").Document("data");
        var interactionsSnapshot = await interactionsDocRef.GetSnapshotAsync();
        if (interactionsSnapshot.Exists)
            userDto.Interactions = interactionsSnapshot.ConvertTo<InteractionsDTO>();
        else
            userDto.Interactions = new InteractionsDTO(); // Or handle as an error if necessary

        var user = _mapper.Map<User>(userDto);
        return user;
    }

    ///<summary>
    /// Get user by ID asynchronously.
    ///</summary>
    ///<param name="id">ID of the user.</param>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
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

    ///<summary>
    /// Get all users asynchronously.
    ///</summary>
    ///<returns>A task representing the asynchronous operation with the result of the action.</returns>
    public async Task<Result<List<User>>> GetAllAsync() {
        var querySnapshot = await db.Collection("users").GetSnapshotAsync();
        var users = querySnapshot.Documents.Select(doc => _mapper.Map<User>(doc.ConvertTo<UserDTO>())).ToList();
        return users;
    }

    ///<summary>
    /// Helper method to convert a UserDTO object into a dictionary format suitable for Firestore storage.
    ///</summary>
    ///<param name="firebaseUser">The UserDTO object to convert.</param>
    ///<returns>A dictionary containing the converted properties.</returns>
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