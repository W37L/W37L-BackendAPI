using System.Reflection;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace Persistance.PostPersistance;

public class Mapper {
    public static ContentDTO MapToDto(Content content) {
        if (content == null) return null;
        ContentDTO c = new();
        c.PostId = content.Id.Value;
        c.UserId = content.Creator.Id.Value;
        c.ParentPostId = content.ParentPost?.Id.Value;
        c.Type = content.PostType.ToString();
        c.Content = content.ContentTweet.Value;
        c.Likes = content.Likes.Value;
        c.CreatedAt = content.CreatedAt.Value.ToString();
        c.UserPub = content.Creator.Pub.Value;
        c.Signature = content.Signature.Value;
        c.IsDeleted = false; // Assuming a default or no corresponding domain model property
        if (content is not Post post) return c;
        c.MediaUrl = post?.MediaUrl?.Url;
        c.MediaType = post?.MediaType.ToString();
        c.Thumbnail = null;
        c.Retweets = 0;
        c.Comments = 0; //TODO: Implement this
        return c;
    }

    public static Content MapToDomain(ContentDTO contentDto) {
        if (contentDto.Type.Equals("comment", StringComparison.OrdinalIgnoreCase))
            return MapToDomainComment(contentDto);

        return MapToDomainPost(contentDto);
    }

    private static Comment MapToDomainComment(ContentDTO contentDto) {
        var privateConstructor = typeof(Comment).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var comment = (Comment)privateConstructor.Invoke(new object[] { });

        var postId = PostId.Create(contentDto.PostId)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var userId = UserID.Create(contentDto.UserId)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var parentPostId = PostId.Create(contentDto.ParentPostId)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var createdAt = CreatedAtType.Create(contentDto.CreatedAt)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var contentTweet = TheString.Create(contentDto.Content)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var likes = Count.Create(contentDto.Likes)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var signature = Signature.Create(contentDto.Signature)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var postType = Enum.TryParse<PostType>(contentDto.Type, out var postTypeResult)
            ? postTypeResult
            : PostType.Comment;


        SetProperty(comment, "Id", postId.Payload);
        SetProperty(comment, "CreatedAt", createdAt.Payload);
        SetProperty(comment, "ContentTweet", contentTweet.Payload);
        SetProperty(comment, "Likes", likes.Payload);
        SetProperty(comment, "Signature", signature.Payload);
        SetProperty(comment, "ParentPost", parentPostId.Payload);
        SetProperty(comment, "PostType", postType);

        return comment;
    }

    private static Post MapToDomainPost(ContentDTO contentDto) {
        var privateConstructor = typeof(Post).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var post = (Post)privateConstructor.Invoke(new object[] { });

        var postId = PostId.Create(contentDto.PostId)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var userId = UserID.Create(contentDto.UserId)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var createdAt = CreatedAtType.Create(contentDto.CreatedAt)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var contentTweet = TheString.Create(contentDto.Content)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var likes = Count.Create(contentDto.Likes)
            .OnFailure(error => throw new ArgumentException(error.Message));
        var signature = Signature.Create(contentDto.Signature)
            .OnFailure(error => throw new ArgumentException(error.Message));

        var postType = Enum.TryParse<PostType>(contentDto.Type, out var postTypeResult)
            ? postTypeResult
            : PostType.Original;

        var mediaType = Enum.TryParse<MediaType>(contentDto.MediaType, out var mediaTypeResult)
            ? mediaTypeResult
            : MediaType.Text;

        SetProperty(post, "Id", postId.Payload);
        SetProperty(post, "CreatedAt", createdAt.Payload);
        SetProperty(post, "ContentTweet", contentTweet.Payload);
        SetProperty(post, "Likes", likes.Payload);
        // SetProperty(post, "Creator", new User(userId.Payload, PubType.Create(contentDto.UserPub).Payload));
        SetProperty(post, "Signature", signature.Payload);
        SetProperty(post, "PostType", postType);
        SetProperty(post, "MediaType", mediaType);

        if (!string.IsNullOrWhiteSpace(contentDto.ParentPostId)) {
            var parentPostId = PostId.Create(contentDto.ParentPostId)
                .OnFailure(error => throw new ArgumentException(error.Message));
            if (parentPostId.IsSuccess) SetProperty(post, "ParentPost", parentPostId.Payload);
        }

        if (!string.IsNullOrWhiteSpace(contentDto.MediaUrl)) {
            var mediaUrl = MediaUrl.Create(contentDto.MediaUrl)
                .OnFailure(error => throw new ArgumentException(error.Message));
            if (mediaUrl.IsSuccess) SetProperty(post, "MediaUrl", mediaUrl.Payload);
        }

        return post;
    }

    public static Content ConcatenateAggreates(User user, Content content) {
        SetProperty(content, "Creator", user);
        return content;
    }

    private static void SetProperty(object obj, string propertyName, object value) {
        var propertyInfo = obj.GetType().GetProperty(
            propertyName,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
        );

        // Ensure the property exists and has a setter
        if (propertyInfo != null && propertyInfo.CanWrite) {
            // If the property has a private setter, it still needs to be accessed explicitly
            var setMethod = propertyInfo.GetSetMethod(true);
            if (setMethod != null)
                setMethod.Invoke(obj, new[] { value });
            else
                throw new InvalidOperationException($"Property '{propertyName}' does not have a setter.");
        }
        else {
            throw new InvalidOperationException($"Property '{propertyName}' not found on type '{obj.GetType()}'.");
        }
    }
}