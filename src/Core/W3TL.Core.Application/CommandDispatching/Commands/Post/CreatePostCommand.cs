using W3TL.Core.Application.CommandDispatching.Commands;
using W3TL.Core.Domain.Agregates.Post.Enum;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

public class CreatePostCommand : Command<PostId>, ICommand<CreatePostCommand> {
    
    /// <summary>
    ///     Initializes a new instance of the CreatePostCommand class with the specified properties.
    /// </summary>
    /// <param name="postId">The unique identifier for the post (might be generated or provided).</param>
    /// <param name="contentTweet">The content of the post (tweet-like).</param>
    /// <param name="creatorId">The ID of the user creating the post.</param>
    /// <param name="signature">The signature associated with the post.</param>
    /// <param name="postType">The type of the post (e.g., Text, Image, Video).</param>
    /// <param name="mediaUrl">The optional URL of any media attached to the post.</param>
    /// <param name="mediaType">The type of media attached to the post (defaults to Text if not provided).</param>
    /// <param name="parentPostId">The optional ID of the parent post (for replies or nested comments).</param>
    private CreatePostCommand(PostId postId, TheString contentTweet, UserID creatorId, Signature signature,
        PostType postType, MediaUrl? mediaUrl, MediaType mediaType, PostId? parentPostId) : base(postId) {
        ContentTweet = contentTweet;
        CreatorId = creatorId;
        Signature = signature;
        PostType = postType;
        MediaUrl = mediaUrl;
        MediaType = mediaType;
        ParentPostId = parentPostId;
    }

    public TheString ContentTweet { get; }
    public UserID CreatorId { get; }
    public Signature Signature { get; }
    public PostType PostType { get; }
    public MediaUrl? MediaUrl { get; }
    public MediaType MediaType { get; } = MediaType.Text;
    public PostId? ParentPostId { get; }

    /// <summary>
    ///   The number of parameters required to create a new CreatePostCommand instance.
    /// </summary>
    public static int ParametersCount { get; } = 5;

    /// <summary>
    ///     Attempts to create a new CreatePostCommand instance from the provided arguments.
    /// </summary>
    /// <param name="args">An array of objects representing the command parameters.</param>
    /// <returns>A Result object containing either a new CreatePostCommand instance or an error message if creation fails.</returns>
    public static Result<CreatePostCommand> Create(params object[] args) {
        if (args.Length < ParametersCount) {
            return Error.WrongNumberOfParameters; // Not enough parameters
        }

        var errors = new HashSet<Error>();

        var postIdResult = (args[0] == null) || string.IsNullOrWhiteSpace(args[0]?.ToString())
            ? PostId.Generate()
            : PostId.Create(args[0].ToString());

        if (postIdResult.IsFailure) errors.Add(postIdResult.Error);

        var contentTweetResult = TheString.Create(args[1].ToString());
        if (contentTweetResult.IsFailure) errors.Add(contentTweetResult.Error);

        var creatorIdResult = UserID.Create(args[2].ToString());

        if (creatorIdResult.IsFailure) errors.Add(creatorIdResult.Error);
        //find the creator in the repository
        User creator; //= creatorRepository.Find(creatorIdResult.Payload);

        var signatureResult = Signature.Create(args[3].ToString());
        if (signatureResult.IsFailure) errors.Add(signatureResult.Error);

        // Mandatory PostType parsing
        if (!Enum.TryParse<PostType>(args[4].ToString(), true, out var postType)) {
            errors.Add(Error.InvalidPostType);
        }

        // Optional MediaUrl parsing
        MediaUrl? mediaUrl = null;
        if (args.Length > 5 && !string.IsNullOrWhiteSpace(args[5]?.ToString())) {
            var mediaUrlResult = MediaUrl.Create(args[5].ToString());
            if (mediaUrlResult.IsSuccess) {
                mediaUrl = mediaUrlResult.Payload;
            }
            else {
                errors.Add(mediaUrlResult.Error);
            }
        }

        // Optional MediaType parsing
        MediaType mediaType = MediaType.Text;
        if (args.Length > 6 && !string.IsNullOrWhiteSpace(args[6]?.ToString())) {
            if (!Enum.TryParse<MediaType>(args[6].ToString(), true, out var parsedMediaType)) {
                errors.Add(Error.InvalidMediaType);
            }
            else {
                mediaType = parsedMediaType;
            }
        }

        // Optional ParentPostId parsing
        PostId? parentPostId = null;
        if (args.Length > 7 && !string.IsNullOrWhiteSpace(args[7]?.ToString())) {
            var parentPostIdResult = PostId.Create(args[7].ToString());
            if (parentPostIdResult.IsSuccess) {
                parentPostId = parentPostIdResult.Payload;
                //find the parent post in the repository
            }
            else {
                errors.Add(parentPostIdResult.Error);
            }
        }

        if (args.Length > 8) errors.Add(Error.WrongNumberOfParameters);

        if (errors.Any()) {
            return Error.CompileErrors(errors);
        }

        // Assuming creator creation/fetching is handled correctly and you pass it to the constructor.
        return new CreatePostCommand(postIdResult.Payload, contentTweetResult.Payload, creatorIdResult.Payload,
            signatureResult.Payload, postType, mediaUrl, mediaType, parentPostId);
    }
}