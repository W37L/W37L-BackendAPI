using System.Reflection;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace ObjectMapper.Mappers;

/// <summary>
///     Class responsible for mapping a <see cref="ContentDTO" /> object to a <see cref="Comment" /> object.
/// </summary>
public class CommentDomainMapper : IMappingConfig<ContentDTO, Comment> {
    /// <summary>
    ///     Maps a ContentDTO object to a Comment object.
    /// </summary>
    /// <param name="contentDto">The ContentDTO object to be mapped.</param>
    /// <returns>The mapped Comment object.</returns>
    public Comment Map(ContentDTO contentDto) {
        var privateConstructor = typeof(Comment).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { }, // Match the parameters of the private constructor
            null);

        var comment = (Comment)privateConstructor.Invoke(new object[] { });

        var postId = CommentId.Create(contentDto.PostId)
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


        Reflexion.SetProperty(comment, "Id", postId.Payload);
        Reflexion.SetProperty(comment, "CreatedAt", createdAt.Payload);
        Reflexion.SetProperty(comment, "ContentTweet", contentTweet.Payload);
        Reflexion.SetProperty(comment, "Likes", likes.Payload);
        Reflexion.SetProperty(comment, "Signature", signature.Payload);
        // Reflexion.SetProperty(comment, "ParentPost", parentPostId.Payload);
        Reflexion.SetProperty(comment, "PostType", postType);

        return comment;
    }
}