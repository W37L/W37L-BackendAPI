using System.Text;
using Newtonsoft.Json;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Repository;

namespace Persistance.PostPersistance;

public class PostRepository : IContentRepository {
    private const string BaseUrl = "http://localhost:3000/api"; // Adjust this URL to your actual backend server URL

    private readonly HttpClient _httpClient;

    public PostRepository(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<Result> AddAsync(Content aggregate) {
        var contentDto = ContentDTO.MapToDto(aggregate);
        var settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
        };
        var json = JsonConvert.SerializeObject(contentDto, settings);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BaseUrl}/post", stringContent);
        if (response.IsSuccessStatusCode) return Result.Success();

        var error = await response.Content.ReadAsStringAsync();
        return Result.Fail(Error.FromString(error));
    }

    public Task<Result> UpdateAsync(Content aggregate) {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(ContentIDBase id) {
        throw new NotImplementedException();
    }

    public async Task<Result<Content>> GetByIdAsync(ContentIDBase id) {
        var response = await _httpClient.GetAsync($"{BaseUrl}/post/{id.Value}");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Content>(content);
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<Content>.Fail(Error.FromString(error));
    }

    public async Task<Result<List<Content>>> GetAllAsync() {
        var response = await _httpClient.GetAsync($"{BaseUrl}/getPost");
        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Content>>(content);
        }

        var error = await response.Content.ReadAsStringAsync();
        return Result<List<Content>>.Fail(Error.FromString(error));
    }
}