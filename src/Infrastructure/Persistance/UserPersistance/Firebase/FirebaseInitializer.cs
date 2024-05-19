using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;

public static class FirebaseInitializer {
    private static FirestoreDb _firestoreDb;

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