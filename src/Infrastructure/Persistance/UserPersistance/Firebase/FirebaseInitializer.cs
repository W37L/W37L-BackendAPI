using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Initializes Firebase services and provides access to the Firestore database.
/// </summary>
public static class FirebaseInitializer {
    private static FirestoreDb _firestoreDb;

    /// <summary>
    /// Gets the Firestore database instance.
    /// </summary>    private static FirestoreDb _firestoreDb;
    public static FirestoreDb FirestoreDb {
        get {
            if (_firestoreDb == null) {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
                var projectId = configuration["FirebaseConfig:ProjectId"];
                _firestoreDb = FirestoreDb.Create(projectId);
            }

            return _firestoreDb;
        }
    }
}