using Google.Cloud.Firestore;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Persistance.UserPersistance;

public class InteractionRepository : IInteractionRepository {
    private readonly FirestoreDb db = FirebaseInitializer.FirestoreDb;

    public Task<Result> BlockUserAsync(string userId, string blockedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnblockUserAsync(string userId, string blockedUserId) {
        throw new NotImplementedException();
    }

    public async Task<Result> FollowUserAsync(string userId, string followedUserId) {
        // Get reference to the initiator's (User A) interactions document
        var userAInteractionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        // Get reference to the followed user's (User B) interactions document
        var userBInteractionRef =
            db.Collection("users").Document(followedUserId).Collection("interactions").Document("data");

        // Execute a transaction to update or create the 'following' and 'followers' lists
        return await db.RunTransactionAsync<Result>(async transaction => {
            // Get snapshots for both user A and user B
            var userASnapshot = await transaction.GetSnapshotAsync(userAInteractionRef);
            var userBSnapshot = await transaction.GetSnapshotAsync(userBInteractionRef);

            InteractionsDTO userAInteractions;
            InteractionsDTO userBInteractions;

            // Initialize or convert user A interactions
            if (userASnapshot.Exists)
                userAInteractions = userASnapshot.ConvertTo<InteractionsDTO>();
            else
                userAInteractions = new InteractionsDTO {
                    following = new List<string>(),
                    followers = new List<string>(),
                    blockedUsers = new List<string>()
                };

            // Initialize or convert user B interactions
            if (userBSnapshot.Exists)
                userBInteractions = userBSnapshot.ConvertTo<InteractionsDTO>();
            else
                userBInteractions = new InteractionsDTO {
                    following = new List<string>(),
                    followers = new List<string>(),
                    blockedUsers = new List<string>()
                };

            // Check if User A already follows User B
            if (!userAInteractions.following.Contains(followedUserId)) {
                // Add User B to User A's following list
                userAInteractions.following.Add(followedUserId);

                // Add User A to User B's followers list
                userBInteractions.followers.Add(userId);

                // Update or create User A's interactions document
                if (!userASnapshot.Exists)
                    transaction.Set(userAInteractionRef, userAInteractions);
                else
                    transaction.Update(userAInteractionRef, "following", userAInteractions.following);

                // Update or create User B's interactions document
                if (!userBSnapshot.Exists)
                    transaction.Set(userBInteractionRef, userBInteractions);
                else
                    transaction.Update(userBInteractionRef, "followers", userBInteractions.followers);

                return Result.Ok;
            }

            return Error.UserAlreadyFollowed;
        });
    }


    public async Task<Result> UnfollowUserAsync(string userId, string followedUserId) {
        // Get reference to the initiator's (User A) interactions document
        var userAInteractionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        // Get reference to the followed user's (User B) interactions document
        var userBInteractionRef =
            db.Collection("users").Document(followedUserId).Collection("interactions").Document("data");

        // Execute a transaction to update or create the 'following' and 'followers' lists
        return await db.RunTransactionAsync<Result>(async transaction => {
            // Get snapshots for both user A and user B
            var userASnapshot = await transaction.GetSnapshotAsync(userAInteractionRef);
            var userBSnapshot = await transaction.GetSnapshotAsync(userBInteractionRef);

            InteractionsDTO userAInteractions;
            InteractionsDTO userBInteractions;

            // Initialize or convert user A interactions
            if (userASnapshot.Exists)
                userAInteractions = userASnapshot.ConvertTo<InteractionsDTO>();
            else
                return Error.UserNotFound;

            // Initialize or convert user B interactions
            if (userBSnapshot.Exists)
                userBInteractions = userBSnapshot.ConvertTo<InteractionsDTO>();
            else
                return Error.UserNotFound;

            // Check if User A actually follows User B
            if (userAInteractions.following.Contains(followedUserId) && userBInteractions.followers.Contains(userId)) {
                // Remove User B from User A's following list
                userAInteractions.following.Remove(followedUserId);

                // Remove User A from User B's followers list
                userBInteractions.followers.Remove(userId);

                // Update User A's interactions document
                transaction.Update(userAInteractionRef, "following", userAInteractions.following);

                // Update User B's interactions document
                transaction.Update(userBInteractionRef, "followers", userBInteractions.followers);

                return Result.Ok;
            }

            return Error.UserNotFollowed; // Or a similar error indicating the user wasn't followed
        });
    }


    public Task<Result> MuteUserAsync(string userId, string mutedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnmuteUserAsync(string userId, string mutedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> ReportUserAsync(string userId, string reportedUserId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnreportUserAsync(string userId, string reportedUserId) {
        throw new NotImplementedException();
    }

    public async Task<Result> LikeTweetAsync(string userId, string tweetId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return await db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.likedTweetIds == null) interactions.likedTweetIds = new List<string>();

                if (!interactions.likedTweetIds.Contains(tweetId)) {
                    interactions.likedTweetIds.Add(tweetId);
                    transaction.Update(interactionRef, "likedTweetIds", interactions.likedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetAlreadyLiked;
            }

            // Here the document does not exist, so we create it with the tweetId in the list
            interactions = new InteractionsDTO {
                likedTweetIds = new List<string> { tweetId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }


    public async Task<Result> UnlikeTweetAsync(string userId, string tweetId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return await db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.likedTweetIds.Contains(tweetId)) {
                    interactions.likedTweetIds.Remove(tweetId);
                    transaction.Update(interactionRef, "likedTweetIds", interactions.likedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetNotLiked;
            }

            return Error.UserNotFound;
        });
    }


    public Task<Result> RetweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnretweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> HighlightTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }

    public Task<Result> UnhighlightTweetAsync(string userId, string tweetId) {
        throw new NotImplementedException();
    }
}