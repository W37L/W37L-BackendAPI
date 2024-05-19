using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Agregates.Post.Values;
using W3TL.Core.Domain.Common.Values;

namespace Persistance.PostPersistance;

/// <summary>
/// Represents a repository for managing posts.
/// </summary>
public class PostRepository : IContentRepository {
    private readonly string _baseUrl;

    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the PostRepository class with the specified dependencies.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for making requests to the backend API.</param>
    /// <param name="mapper">The mapper used for mapping between DTOs and domain entities.</param>
    /// <param name="configuration">The configuration used for accessing configuration settings.</param>
    public PostRepository(HttpClient httpClient, IMapper mapper, IConfiguration configuration) {
        _httpClient = httpClient;
        _mapper = mapper;
        _baseUrl = configuration["BackendConfig:BaseUrl"];
    }

    ///<summary>
    /// Adds content asynchronously to the backend API.
    ///</summary>
    ///<param name="aggregate">The content to add.</param>
    ///<returns>A task representing the asynchronous operation. The task result indicates success or failure.</returns>
    public async Task<Result> AddAsync(Content aggregate) {
        var contentDto = _mapper.Map<ContentDTO>(aggregate);
        var settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore,
        };
        var json = JsonConvert.SerializeObject(contentDto, settings);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/post", stringContent);
        if (response.IsSuccessStatusCode) return Result.Success();

        var error = await response.Content.ReadAsStringAsync();
        return Result.Fail(Error.FromString(error));
    }
    
    ///<summary>
    /// Updates content asynchronously in the backend API.
    ///</summary>
    ///<param name="aggregate">The content to update.</param>
    ///<returns>A task representing the asynchronous operation. The task result indicates success or failure.</returns>
    public async Task<Result> UpdateAsync(Content aggregate) {
        var contentDto = _mapper.Map<ContentDTO>(aggregate);
        var settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
        };
        var json = JsonConvert.SerializeObject(contentDto, settings);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var reposons = await _httpClient.PutAsync($"{_baseUrl}/post", stringContent);
        if (reposons.IsSuccessStatusCode) return Result.Success();

        var error = await reposons.Content.ReadAsStringAsync();
        return Result.Fail(Error.FromString(error));
    }

    ///<summary>
    /// Deletes content asynchronously from the backend API.
    ///</summary>
    ///<param name="id">The ID of the content to delete.</param>
    ///<returns>A task representing the asynchronous operation. The task result indicates success or failure.</returns>
    public Task<Result> DeleteAsync(ContentIDBase id) {
        throw new NotImplementedException();
    }

    ///<summary>
    /// Retrieves content by its ID asynchronously from the backend API.
    ///</summary>
    ///<param name="id">The ID of the content to retrieve.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains the retrieved content or an error.</returns>
    public async Task<Result<Content>> GetByIdAsync(ContentIDBase id) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getPostById/{id.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var domainPost = _mapper.Map<Content>(post);
            return domainPost;
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<Content>.Fail(Error.FromString(error));
    }

    ///<summary>
    /// Retrieves all content asynchronously from the backend API.
    ///</summary>
    ///<returns>A task representing the asynchronous operation. The task result contains a list of all retrieved content or an error.</returns>
    public async Task<Result<List<Content>>> GetAllAsync() {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getPosts");
        return await ProcessContentResponse(response);
    }

    ///<summary>
    /// Retrieves the author ID of a content item asynchronously from the backend API.
    ///</summary>
    ///<param name="id">The ID of the content item to retrieve the author ID for.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains the author ID or an error.</returns>
    public async Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id) {
        // if conteent ID start with PID, it is a post, if start with CID, it is a comment
        var response = new HttpResponseMessage();
        if (id.Value.StartsWith("PID")) {
            response = await _httpClient.GetAsync($"{_baseUrl}/getPostById/{id.Value}");
        }
        else {
            response = await _httpClient.GetAsync($"{_baseUrl}/getCommentById/{id.Value}");
        }

        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var userId = UserID.Create(post.UserId)
                .OnFailure(error => throw new ArgumentException(error.Message));
            return userId;
        }

        return Result<UserID>.Fail(Error.UserNotFound);
    }

    ///<summary>
    /// Retrieves content by its full ID, including user information, asynchronously from the backend API.
    ///</summary>
    ///<param name="id">The ID of the content to retrieve.</param>
    ///<param name="user">The user requesting the content.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains the retrieved content or an error.</returns>
    public async Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, User user) {
        var content = await GetByIdAsync(id);
        return content.IsFailure ? content : Concatenate.Append(user, content.Payload);
    }

    ///<summary>
    /// Retrieves all posts by a user asynchronously from the backend API.
    ///</summary>
    ///<param name="userId">The ID of the user whose posts are to be retrieved.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains a list of posts or an error.</returns>
    public async Task<Result<List<Content>>> GetPostsByUserIdAsync(UserID userId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getPostsByUserId/{userId.Value}");
        return await ProcessContentResponse(response);
    }

    ///<summary>
    /// Retrieves the parent post ID of a comment asynchronously from the backend API.
    ///</summary>
    ///<param name="commentId">The ID of the comment to retrieve the parent post ID for.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains the parent post ID or an error.</returns>
    public async Task<Result<PostId>> GetParentPostIdAsync(CommentId commentId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getCommentById/{commentId.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var postId = PostId.Create(post.ParentPostId)
                .OnFailure(error => throw new ArgumentException(error.Message));
            return postId;
        }

        return Error.PostNotFound;
    }

    ///<summary>
    /// Retrieves comments by a user asynchronously from the backend API.
    ///</summary>
    ///<param name="queryUserId">The ID of the user whose comments are to be retrieved.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains a list of comments or an error.</returns>
    public async Task<Result<List<Content>>> GetCommentsByUserIdAsync(UserID queryUserId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getCommentsByUserId/{queryUserId.Value}");
        return await ProcessContentResponse(response);
    }

    ///<summary>
    /// Retrieves comments by a post asynchronously from the backend API.
    ///</summary>
    ///<param name="postId">The ID of the post whose comments are to be retrieved.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains a list of comments or an error.</returns>
    public async Task<Result<List<Content>>> GetCommentsByPostIdAsync(ContentIDBase postId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getCommentsByPostId/{postId.Value}");
        return await ProcessContentResponse(response);
    }

    /// <inheritdoc />
    public async Task<Result<Content>> GetCommentByIdAsync(CommentId queryCommentId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getCommentById/{queryCommentId.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var domainPost = _mapper.Map<Content>(post);
            return domainPost;
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<Content>.Fail(Error.FromString(error));
    }

    ///<summary>
    /// Retrieves a comment by its ID asynchronously from the backend API.
    ///</summary>
    ///<param name="queryCommentId">The ID of the comment to retrieve.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains the retrieved comment or an error.</returns>
    public async Task<Result<Content>> GetCommentByIdWithAuthorAsync(CommentId queryCommentId, User user) {
        var content = await GetCommentByIdAsync(queryCommentId);
        return content.IsFailure ? content : Concatenate.Append(user, content.Payload);
    }

    ///<summary>
    /// Retrieves all posts commented by a user asynchronously from the backend API.
    ///</summary>
    ///<param name="userId">The ID of the user whose commented posts are to be retrieved.</param>
    ///<returns>A task representing the asynchronous operation. The task result contains a list of posts or an error.</returns>
    public async Task<Result<List<Content>>> GetAllPostThatUserCommentAsync(UserID userId) {
        var response = await _httpClient.GetAsync($"{_baseUrl}/getPostsCommentedByUser/{userId.Value}");
        return await ProcessContentResponse(response);
    }
    
    /// <summary>
    /// Processes the HTTP response message from the backend API to handle content retrieval.
    /// </summary>
    /// <param name="response">The HTTP response message received from the backend API.</param>
    /// <returns>A result containing a list of domain content objects if successful, otherwise an error result.</returns>
    private async Task<Result<List<Content>>> ProcessContentResponse(HttpResponseMessage response) {
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            return Result<List<Content>>.Fail(Error.FromString(error));
        }

        var content = await response.Content.ReadAsStringAsync();
        var postsDto = JsonConvert.DeserializeObject<List<ContentDTO>>(content);
        var uniquePostsDto = postsDto
            .GroupBy(p => p.PostId)
            .Select(g => g.First())
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
        var domainPosts = uniquePostsDto.Select(dto => _mapper.Map<Content>(dto)).ToList();
        return Result<List<Content>>.Success(domainPosts);
    }
}