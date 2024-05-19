using System.Reflection;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace ObjectMapper.Mappers;

public class PostDomainMapper : IMappingConfig<ContentDTO, Post> {
    /// <summary>
    ///     Maps a ContentDTO object to a Post object.
    /// </summary>
    /// <param name="contentDto">The ContentDTO object to be mapped.</param>
    /// <returns>The mapped Post object.</returns>
    public Post Map(ContentDTO contentDto) {
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

        Reflexion.SetProperty(post, "Id", postId.Payload);
        Reflexion.SetProperty(post, "CreatedAt", createdAt.Payload);
        Reflexion.SetProperty(post, "ContentTweet", contentTweet.Payload);
        Reflexion.SetProperty(post, "Likes", likes.Payload);
        // Reflexion.SetProperty(post, "Creator", new User(userId.Payload, PubType.Create(contentDto.UserPub).Payload));
        Reflexion.SetProperty(post, "Signature", signature.Payload);
        Reflexion.SetProperty(post, "PostType", postType);
        Reflexion.SetProperty(post, "MediaType", mediaType);

        if (!string.IsNullOrWhiteSpace(contentDto.ParentPostId)) {
            var parentPostId = PostId.Create(contentDto.ParentPostId)
                .OnFailure(error => throw new ArgumentException(error.Message));
            if (parentPostId.IsSuccess) Reflexion.SetProperty(post, "ParentPost", parentPostId.Payload);
        }

        if (!string.IsNullOrWhiteSpace(contentDto.MediaUrl)) {
            var mediaUrl = MediaUrl.Create(contentDto.MediaUrl)
                .OnFailure(error => throw new ArgumentException(error.Message));
            if (mediaUrl.IsSuccess) Reflexion.SetProperty(post, "MediaUrl", mediaUrl.Payload);
        }

        return post;
    }
}