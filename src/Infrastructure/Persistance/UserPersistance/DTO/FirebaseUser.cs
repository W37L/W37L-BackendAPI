using Google.Cloud.Firestore;

namespace Persistence.UserPersistence;

[FirestoreData]
public class FirebaseUser {
    [FirestoreProperty("userId")] public string userId { get; set; }

    [FirestoreProperty("username")] public string username { get; set; }

    [FirestoreProperty("name")] public string name { get; set; }

    [FirestoreProperty("lastname")] public string lastname { get; set; }

    [FirestoreProperty("email")] public string email { get; set; }

    [FirestoreProperty("avatar")] public string? avatar { get; set; }

    [FirestoreProperty("bio")] public string? bio { get; set; }

    [FirestoreProperty("location")] public string? location { get; set; }

    [FirestoreProperty("website")] public string? website { get; set; }

    [FirestoreProperty("verified")] public bool verified { get; set; }

    [FirestoreProperty("createdAt")] public string createdAt { get; set; }

    [FirestoreProperty("followersCount")] public int followersCount { get; set; }

    [FirestoreProperty("followingCount")] public int followingCount { get; set; }

    [FirestoreProperty("background")] public string? background { get; set; }

    [FirestoreProperty("pub")] public string? pub { get; set; }
}