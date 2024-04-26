using W3TL.Core.Domain.Agregates.Post;

namespace Persistance.PostPersistance;

public class ContentDTO {
    public string PostId { get; set; }
    public string UserId { get; set; }
    public string ParentPostId { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public int Likes { get; set; }
    public string CreatedAt { get; set; }
    public string MediaUrl { get; set; }
    public string? MediaType { get; set; }
    public string? Thumbnail { get; set; }
    public int Retweets { get; set; }
    public int Comments { get; set; }
    public string UserPub { get; set; }
    public string Signature { get; set; }
    public bool IsDeleted { get; set; }

    public static ContentDTO MapToDto(Content content) {
        if (content == null) return null;
        if (content is Post post)
            return new ContentDTO {
                PostId = post.Id.Value,
                UserId = post.Creator.Id.Value,
                ParentPostId = post.ParentPost?.Id.Value,
                Type = post.PostType.ToString(),
                Content = post.ContentTweet.Value,
                Likes = post.Likes.Value,
                CreatedAt = post.CreatedAt.Value.ToString(),
                MediaUrl = post?.MediaUrl?.Url,
                MediaType = post?.MediaType.ToString(),
                Thumbnail = null, // TODO: Implement Thumbnail property in Post domain model
                Retweets = 0, // TODO: Implement Retweets property in Post domain model
                Comments = post.Comments.Count,
                UserPub = content.Creator.Pub.Value,
                Signature = content.Signature.Value,
                IsDeleted = false // Assuming a default or no corresponding domain model property
            };

        return null;
    }
}