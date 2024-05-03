namespace ObjectMapper.DTO;

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
    List<ContentDTO> Comms { get; set; }
}