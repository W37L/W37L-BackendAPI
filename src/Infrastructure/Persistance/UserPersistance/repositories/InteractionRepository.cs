using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using ObjectMapper;
using Persistence.UserPersistence.Firebase;
using W3TL.Core.Domain.Agregates.User.Repository;

namespace Persistance.UserPersistance;

public class InteractionRepository : IInteractionRepository {
    private readonly IMapper _mapper;
    private readonly FirestoreDb db;
    private readonly ILogger<InteractionRepository> logger;

    public InteractionRepository(IMapper mapper, ILogger<InteractionRepository> logger) {
        _mapper = mapper;
        db = FirebaseInitializer.FirestoreDb;
        this.logger = logger;
    }


    public Task<Result> BlockUserAsync(string userId, string blockedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.blockedUsers == null) interactions.blockedUsers = new List<string>();

                if (!interactions.blockedUsers.Contains(blockedUserId)) {
                    interactions.blockedUsers.Add(blockedUserId);
                    transaction.Update(interactionRef, "blockedUsers", interactions.blockedUsers);
                    return Result.Ok;
                }

                return Error.UserAlreadyBlocked;
            }

            // Here the document does not exist, so we create it with the blockedUserId in the list
            interactions = new InteractionsDTO {
                blockedUsers = new List<string> { blockedUserId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }

    public Task<Result> UnblockUserAsync(string userId, string blockedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.blockedUsers.Contains(blockedUserId)) {
                    interactions.blockedUsers.Remove(blockedUserId);
                    transaction.Update(interactionRef, "blockedUsers", interactions.blockedUsers);
                    return Result.Ok;
                }

                return Error.UserNotBlocked;
            }

            return Error.UserNotFound;
        });
    }

    public async Task<Result> FollowUserAsync(string userId, string followedUserId) {
        logger.LogDebug("Starting to process following user: User {UserId} follows {FollowedUserId}", userId,
            followedUserId);

        var userAInteractionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");
        var userBInteractionRef =
            db.Collection("users").Document(followedUserId).Collection("interactions").Document("data");

        try {
            return await db.RunTransactionAsync<Result>(async transaction => {
                var userASnapshot = await transaction.GetSnapshotAsync(userAInteractionRef);
                var userBSnapshot = await transaction.GetSnapshotAsync(userBInteractionRef);

                InteractionsDTO userAInteractions = userASnapshot.Exists
                    ? userASnapshot.ConvertTo<InteractionsDTO>()
                    : new InteractionsDTO();
                InteractionsDTO userBInteractions = userBSnapshot.Exists
                    ? userBSnapshot.ConvertTo<InteractionsDTO>()
                    : new InteractionsDTO();

                if (userAInteractions.following == null) {
                    userAInteractions.following = new List<string>();
                    logger.LogDebug("Initialized following list for User A {UserId}", userId);
                }

                if (userBInteractions.followers == null) {
                    userBInteractions.followers = new List<string>();
                    logger.LogDebug("Initialized followers list for User B {FollowedUserId}", followedUserId);
                }

                if (!userAInteractions.following.Contains(followedUserId)) {
                    userAInteractions.following.Add(followedUserId);
                    userBInteractions.followers.Add(userId);

                    if (!userASnapshot.Exists)
                        transaction.Set(userAInteractionRef, userAInteractions);
                    else
                        transaction.Update(userAInteractionRef, "following", userAInteractions.following);

                    if (!userBSnapshot.Exists)
                        transaction.Set(userBInteractionRef, userBInteractions);
                    else
                        transaction.Update(userBInteractionRef, "followers", userBInteractions.followers);

                    logger.LogInformation("User {UserId} now follows User {FollowedUserId}", userId, followedUserId);
                    return Result.Ok;
                }

                logger.LogWarning("User {UserId} already follows User {FollowedUserId}", userId, followedUserId);
                return Error.UserAlreadyFollowed;
            });
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error occurred while trying to follow User {FollowedUserId} by User {UserId}",
                followedUserId, userId);
            throw;
        }
    }


    public async Task<Result> UnfollowUserAsync(string userId, string followedUserId) {
        logger.LogDebug("Starting to process unfollowing: User {UserId} unfollows {FollowedUserId}", userId,
            followedUserId);

        var userAInteractionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");
        var userBInteractionRef =
            db.Collection("users").Document(followedUserId).Collection("interactions").Document("data");

        try {
            return await db.RunTransactionAsync<Result>(async transaction => {
                var userASnapshot = await transaction.GetSnapshotAsync(userAInteractionRef);
                var userBSnapshot = await transaction.GetSnapshotAsync(userBInteractionRef);

                InteractionsDTO userAInteractions = userASnapshot.Exists
                    ? userASnapshot.ConvertTo<InteractionsDTO>()
                    : new InteractionsDTO();
                InteractionsDTO userBInteractions = userBSnapshot.Exists
                    ? userBSnapshot.ConvertTo<InteractionsDTO>()
                    : new InteractionsDTO();

                if (userAInteractions.following == null) {
                    userAInteractions.following = new List<string>();
                    logger.LogDebug("Initialized following list for User A {UserId}", userId);
                }

                if (userBInteractions.followers == null) {
                    userBInteractions.followers = new List<string>();
                    logger.LogDebug("Initialized followers list for User B {FollowedUserId}", followedUserId);
                }

                if (userAInteractions.following.Contains(followedUserId)) {
                    userAInteractions.following.Remove(followedUserId);
                    userBInteractions.followers.Remove(userId);

                    if (!userASnapshot.Exists)
                        transaction.Set(userAInteractionRef, userAInteractions);
                    else
                        transaction.Update(userAInteractionRef, "following", userAInteractions.following);

                    if (!userBSnapshot.Exists)
                        transaction.Set(userBInteractionRef, userBInteractions);
                    else
                        transaction.Update(userBInteractionRef, "followers", userBInteractions.followers);

                    logger.LogInformation("User {UserId} has unfollowed User {FollowedUserId}", userId, followedUserId);
                    return Result.Ok;
                }

                logger.LogWarning("User {UserId} does not follow User {FollowedUserId}, so cannot unfollow", userId,
                    followedUserId);
                return Error.UserNotFollowed;
            });
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error occurred while trying to unfollow User {FollowedUserId} by User {UserId}",
                followedUserId, userId);
            throw;
        }
    }


    public Task<Result> MuteUserAsync(string userId, string mutedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.mutedUsers == null) interactions.mutedUsers = new List<string>();

                if (!interactions.mutedUsers.Contains(mutedUserId)) {
                    interactions.mutedUsers.Add(mutedUserId);
                    transaction.Update(interactionRef, "mutedUsers", interactions.mutedUsers);
                    return Result.Ok;
                }

                return Error.UserAlreadyMuted;
            }

            // Here the document does not exist, so we create it with the mutedUserId in the list
            interactions = new InteractionsDTO {
                mutedUsers = new List<string> { mutedUserId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }

    public Task<Result> UnmuteUserAsync(string userId, string mutedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.mutedUsers.Contains(mutedUserId)) {
                    interactions.mutedUsers.Remove(mutedUserId);
                    transaction.Update(interactionRef, "mutedUsers", interactions.mutedUsers);
                    return Result.Ok;
                }

                return Error.UserNotMuted;
            }

            return Error.UserNotFound;
        });
    }

    public Task<Result> ReportUserAsync(string userId, string reportedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.reportedUsers == null) interactions.reportedUsers = new List<string>();

                if (!interactions.reportedUsers.Contains(reportedUserId)) {
                    interactions.reportedUsers.Add(reportedUserId);
                    transaction.Update(interactionRef, "reportedUsers", interactions.reportedUsers);
                    return Result.Ok;
                }

                return Error.UserAlreadyReported;
            }

            // Here the document does not exist, so we create it with the reportedUserId in the list
            interactions = new InteractionsDTO {
                reportedUsers = new List<string> { reportedUserId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }

    public Task<Result> UnreportUserAsync(string userId, string reportedUserId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.reportedUsers.Contains(reportedUserId)) {
                    interactions.reportedUsers.Remove(reportedUserId);
                    transaction.Update(interactionRef, "reportedUsers", interactions.reportedUsers);
                    return Result.Ok;
                }

                return Error.UserNotReported;
            }

            return Error.UserNotFound;
        });
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
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.retweetedTweetIds == null) interactions.retweetedTweetIds = new List<string>();

                if (!interactions.retweetedTweetIds.Contains(tweetId)) {
                    interactions.retweetedTweetIds.Add(tweetId);
                    transaction.Update(interactionRef, "retweetedTweetIds", interactions.retweetedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetAlreadyRetweeted;
            }

            // Here the document does not exist, so we create it with the tweetId in the list
            interactions = new InteractionsDTO {
                retweetedTweetIds = new List<string> { tweetId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }

    public Task<Result> UnretweetAsync(string userId, string tweetId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.retweetedTweetIds.Contains(tweetId)) {
                    interactions.retweetedTweetIds.Remove(tweetId);
                    transaction.Update(interactionRef, "retweetedTweetIds", interactions.retweetedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetNotRetweeted;
            }

            return Error.UserNotFound;
        });
    }

    public Task<Result> HighlightTweetAsync(string userId, string tweetId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            InteractionsDTO interactions;
            if (snapshot.Exists) {
                interactions = snapshot.ConvertTo<InteractionsDTO>();

                // Initialize the list if it's null (e.g., due to data corruption or partial updates)
                if (interactions.highlightedTweetIds == null) interactions.highlightedTweetIds = new List<string>();

                if (!interactions.highlightedTweetIds.Contains(tweetId)) {
                    interactions.highlightedTweetIds.Add(tweetId);
                    transaction.Update(interactionRef, "highlightedTweetIds", interactions.highlightedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetAlreadyHighlighted;
            }

            // Here the document does not exist, so we create it with the tweetId in the list
            interactions = new InteractionsDTO {
                highlightedTweetIds = new List<string> { tweetId }
            };
            transaction.Set(interactionRef, interactions);
            return Result.Ok;
        });
    }

    public Task<Result> UnhighlightTweetAsync(string userId, string tweetId) {
        var interactionRef = db.Collection("users").Document(userId).Collection("interactions").Document("data");

        return db.RunTransactionAsync<Result>(async transaction => {
            var snapshot = await transaction.GetSnapshotAsync(interactionRef);

            if (snapshot.Exists) {
                var interactions = snapshot.ConvertTo<InteractionsDTO>();
                if (interactions.highlightedTweetIds.Contains(tweetId)) {
                    interactions.highlightedTweetIds.Remove(tweetId);
                    transaction.Update(interactionRef, "highlightedTweetIds", interactions.highlightedTweetIds);
                    return Result.Ok;
                }

                return Error.TweetNotHighlighted;
            }

            return Error.UserNotFound;
        });
    }
}