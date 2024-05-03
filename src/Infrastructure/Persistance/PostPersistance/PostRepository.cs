using System.Text;
using Newtonsoft.Json;
using ObjectMapper;
using ObjectMapper.DTO;
using ObjectMapper.Tools;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Persistance.PostPersistance;

public class PostRepository : IContentRepository {
    private const string BaseUrl = "http://localhost:3000/api"; // Adjust this URL to your actual backend server URL

    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public PostRepository(HttpClient httpClient, IMapper mapper) {
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<Result> AddAsync(Content aggregate) {
        var contentDto = _mapper.Map<ContentDTO>(aggregate);
        var settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore,
        };
        var json = JsonConvert.SerializeObject(contentDto, settings);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BaseUrl}/post", stringContent);
        if (response.IsSuccessStatusCode) return Result.Success();

        var error = await response.Content.ReadAsStringAsync();
        return Result.Fail(Error.FromString(error));
    }

    public async Task<Result> UpdateAsync(Content aggregate) {
        var contentDto = _mapper.Map<ContentDTO>(aggregate);
        var settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
        };
        var json = JsonConvert.SerializeObject(contentDto, settings);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var reposons = await _httpClient.PutAsync($"{BaseUrl}/post", stringContent);
        if (reposons.IsSuccessStatusCode) return Result.Success();

        var error = await reposons.Content.ReadAsStringAsync();
        return Result.Fail(Error.FromString(error));
    }

    public Task<Result> DeleteAsync(ContentIDBase id) {
        throw new NotImplementedException();
    }

    public async Task<Result<Content>> GetByIdAsync(ContentIDBase id) {
        var response = await _httpClient.GetAsync($"{BaseUrl}/getPostById/{id.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var domainPost = _mapper.Map<Content>(post);
            return domainPost;
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<Content>.Fail(Error.FromString(error));
    }

    public async Task<Result<List<Content>>> GetAllAsync() {
        var response = await _httpClient.GetAsync($"{BaseUrl}/getPosts");
        return await ProcessContentResponse(response);
    }


    public async Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id) {
        var response = await _httpClient.GetAsync($"{BaseUrl}/getPostById/{id.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<ContentDTO>(content);
            var userId = UserID.Create(post.UserId)
                .OnFailure(error => throw new ArgumentException(error.Message));
            return userId;
        }

        return Result<UserID>.Fail(Error.UserNotFound);
    }

    public async Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, User user) {
        var content = await GetByIdAsync(id);
        return content.IsFailure ? content : Concatenate.Append(user, content.Payload);
    }

    public async Task<Result<List<Content>>> GetPostsByUserIdAsync(UserID userId) {
        var response = await _httpClient.GetAsync($"{BaseUrl}/getPostsByUserId/{userId.Value}");
        return await ProcessContentResponse(response);
    }

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