using System.Text;
using Newtonsoft.Json;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;
using W3TL.Core.Domain.Common.Values;

namespace Persistance.PostPersistance;

public class PostRepository : IContentRepository {
    private const string BaseUrl = "http://localhost:3000/api"; // Adjust this URL to your actual backend server URL

    private readonly HttpClient _httpClient;

    public PostRepository(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<Result> AddAsync(Content aggregate) {
        var contentDto = Mapper.MapToDto(aggregate);
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
        var contentDto = Mapper.MapToDto(aggregate);
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
            var domainPost = Mapper.MapToDomain(post);
            return domainPost;
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<Content>.Fail(Error.FromString(error));
    }

    public async Task<Result<List<Content>>> GetAllAsync() {
        var response = await _httpClient.GetAsync($"{BaseUrl}/s");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Content>>(content);
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<List<Content>>.Fail(Error.FromString(error));
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
        return content.IsFailure ? content : Mapper.ConcatenateAggreates(user, content.Payload);
    }
}