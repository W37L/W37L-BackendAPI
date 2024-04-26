using Google.Apis.Util;
using Google.Cloud.Firestore;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Persistence.UserPersistence;

public class UserRepository : IUserRepository {
    private readonly FirestoreDb db = FirebaseInitializer.FirestoreDb;

    public async Task<Result> AddAsync(User aggregate) {
        // Use the Mapper to convert domain User to FirebaseUser
        var firebaseUserResult = Mapper.MapDomainUserToFirebaseUser(aggregate);

        // Check for mapping success
        if (firebaseUserResult.IsFailure) return firebaseUserResult.Error;

        var firebaseUser = firebaseUserResult.Payload;

        // Convert FirebaseUser to a dictionary for Firestore upload
        var userDict = ConvertToDictionary(firebaseUser);

        var docRef = db.Collection("users").Document(aggregate.Id.Value);
        await docRef.SetAsync(userDict);
        return Result.Success();
    }


    public async Task<Result> UpdateAsync(User user) {
        var firebaseUser = Mapper.MapDomainUserToFirebaseUser(user);
        if (firebaseUser.IsFailure) return firebaseUser.Error;

        var userDict = ConvertToDictionary(firebaseUser.Payload);

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

    public Task<Result> DeleteAsync(UserID id) {
        throw new NotImplementedException();
    }

    public async Task<Result<User>> GetByIdAsync(UserID id) {
        var docRef = db.Collection("users").Document(id.Value);
        var snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists) {
            var firebaseUser = snapshot.ConvertTo<FirebaseUser>();
            return Mapper.MapFirebaseUserToDomainUser(firebaseUser);
        }

        return Error.UserNotFound;
    }

    // public async Task<Result<User>> GetByIdAsync(UserID id) {
    //     DocumentReference docRef = db.Collection("users").Document(id.Value);
    //     DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
    //     if (snapshot.Exists) {
    //         Dictionary<string, object> user = snapshot.ToDictionary();
    //         Console.WriteLine(user);
    //         return Result.Success(User.Create(id, user["Name"].ToString(), user["Email"].ToString()));
    //         ));
    //     }
    //
    // }

    // public async Task<Result<User>> GetByIdAsync(UserID id) {
    //     // create mock with this values
    //     // public static Result<User> Create(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub
    //     var docRef = db.Collection("users").Document(id.Value);
    //     var snapshot = await docRef.GetSnapshotAsync();
    //     if (snapshot.Exists) {
    //         Dictionary<string, object> user = snapshot.ToDictionary();
    //         Console.WriteLine(user);
    //         var userId = id;
    //         var userName = UserNameType.Create(user["username"].ToString()).Payload;
    //         var firstName = NameType.Create(user["name"].ToString()).Payload;
    //         var lastName = LastNameType.Create(user["lastname"].ToString()).Payload;
    //         var email = EmailType.Create(user["email"].ToString()).Payload;
    //         var pub = PubType.Create(user["pub"].ToString()).Payload;
    //         return User.Create(userId, userName, firstName, lastName, email, pub).Payload;
    //     }
    //
    //     return Error.ExampleError;
    // }


    public Task<Result<List<User>>> GetAllAsync() {
        throw new NotImplementedException();
    }

// Helper method to convert an object to a dictionary using reflection and Firestore attributes
    private static IDictionary<string, object> ConvertToDictionary(FirebaseUser firebaseUser) {
        var dict = new Dictionary<string, object>();
        foreach (var prop in typeof(FirebaseUser).GetProperties()) {
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