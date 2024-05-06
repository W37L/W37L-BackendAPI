using ObjectMapper.DTO;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.Post.Enum;

namespace ObjectMapper.Mappers;

public class ContentDomainMapper : IMappingConfig<ContentDTO, Content> {
    CommentDomainMapper _commentDomainMapper;
    PostDomainMapper _postDomainMapper;

    public ContentDomainMapper() {
        _commentDomainMapper = new CommentDomainMapper();
        _postDomainMapper = new PostDomainMapper();
    }

    public Content Map(ContentDTO input) {
        if (input.Type == PostType.Comment.ToString()) {
            return _commentDomainMapper.Map(input);
        }

        return _postDomainMapper.Map(input);
    }
}