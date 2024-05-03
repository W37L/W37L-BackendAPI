using Google.Cloud.Firestore;

namespace Persistence.UserPersistence.Firebase;

[FirestoreData]
public class InteractionsDTO {
    [FirestoreProperty("blockedUsers")] public List<string> blockedUsers { get; set; }

    [FirestoreProperty("followers")] public List<string> followers { get; set; }

    [FirestoreProperty("following")] public List<string> following { get; set; }

    [FirestoreProperty("highlightedTweetIds")]
    public List<string> highlightedTweetIds { get; set; }

    [FirestoreProperty("likedTweetIds")] public List<string> likedTweetIds { get; set; }

    [FirestoreProperty("mutedUsers")] public List<string> mutedUsers { get; set; }

    [FirestoreProperty("reportedUsers")] public List<string> reportedUsers { get; set; }

    [FirestoreProperty("retweetedTweetIds")]
    public List<string> retweetedTweetIds { get; set; }
}