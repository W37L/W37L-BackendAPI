using W3TL.Core.Domain.Agregates.Post.Values;

namespace UnitTests.Common.Factories;

public static class ValidFields {
    // User
    public static readonly string? VALID_USER_ID = "UID-123456789012345678901234567890123456";
    public static readonly string? VALID_USERNAME = "something_valid";
    public static readonly string? VALID_FIRST_NAME = "John";
    public static readonly string? VALID_LAST_NAME = "Doe";
    public static readonly string? VALID_EMAIL = "john.doe@example.com";
    public static readonly string? VALID_PUB_KEY = "FJbAsEj5pj+xm0lMRwqym72lPWOU0NNeFxwId+bC1iF=";
    public static readonly long VALID_CREATED_AT_UNIX = 1622548800;

    // User Profile
    public static readonly string? VALID_BIO = "This is a valid bio";
    public static readonly string? VALID_LOCATION = "This is a valid location";
    public static readonly string? VALID_WEBSITE = "https://example.com";
    public static readonly string? VALID_AVATAR_URL = "https://example.com/avatar.jpg";
    public static readonly string? VALID_BANNER_URL = "https://example.com/banner.jpg";


    // Post
    public static readonly string VALID_POST_ID = "PID-123456789012345678901234567890123456";
    public static readonly string? VALID_POST_CONTENT = "This is a valid post content";
    public static readonly string VALID_MEDIA_URL = "https://example.com/image.jpg";
    public static readonly MediaType VALID_CONTENT_TYPE = MediaType.Text;
    public static readonly string? VALID_SIGNATURE = "6fCA1f58Ada8fa3e219b87dEc0Cebd98C4B4ac314AD227377Ec6E581F060F46e1f0B58f7e4C23efD6fEa34eCdd6bDf6B74eB233a089AA0a67c412Cc8c7deAc0c";

    // Comment
    public static readonly string VALID_COMMENT_ID = "PID-123456789012345678901234567890123456";
    public static readonly string? VALID_COMMENT_CONTENT = "This is a valid comment content";
    public static readonly string? VALID_COMMENT_SIGNATURE = "6fCA1f58Ada8fa3e219b87dEc0Cebd98C4B4ac314AD227377Ec6E581F060F46e1f0B58f7e4C23efD6fEa34eCdd6bDf6B74eB233a089AA0a67c412Cc8c7deAc0c";
}