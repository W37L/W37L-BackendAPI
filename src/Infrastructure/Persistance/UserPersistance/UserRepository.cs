using Google.Cloud.Firestore;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Repository;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

namespace Persistence.UserPersistence;

public class UserRepository : IUserRepository {
    private readonly FirestoreDb db = FirebaseInitializer.FirestoreDb;

    public async Task<Result> AddAsync(User aggregate) {
        var docRef = db.Collection("users").Document(aggregate.Id.Value);
        var user = new Dictionary<string, object> {
            { "Name", aggregate.FirstName.Value + " " + aggregate.LastName.Value },
            { "Email", aggregate.Email.Value }
            // Add other user properties here
        };
        await docRef.SetAsync(user);
        return Result.Success();
    }

    public Task<Result> UpdateAsync(User aggregate) {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(UserID id) {
        throw new NotImplementedException();
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

    public async Task<Result<User>> GetByIdAsync(UserID id) {
        // create mock with this values
        // public static Result<User> Create(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub
        var docRef = db.Collection("users").Document(id.Value);
        var snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists) {
            Dictionary<string, object> user = snapshot.ToDictionary();
            Console.WriteLine(user);
            var userId = id;
            var userName = UserNameType.Create(user["username"].ToString()).Payload;
            var firstName = NameType.Create(user["name"].ToString()).Payload;
            var lastName = LastNameType.Create(user["lastname"].ToString()).Payload;
            var email = EmailType.Create(user["email"].ToString()).Payload;
            var pub = PubType.Create(user["pub"].ToString()).Payload;
            return User.Create(userId, userName, firstName, lastName, email, pub).Payload;
        }

        return Error.ExampleError;
    }


    public Task<Result<List<User>>> GetAllAsync() {
        throw new NotImplementedException();
    }
}