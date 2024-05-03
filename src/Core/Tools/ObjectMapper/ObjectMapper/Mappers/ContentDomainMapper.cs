using ObjectMapper.DTO;
using W3TL.Core.Domain.Agregates.Post;

namespace ObjectMapper.Mappers;

public class ContentDomainMapper : IMappingConfig<ContentDTO, Content> {
    CommentDomainMapper _commentDomainMapper;
    PostDomainMapper _postDomainMapper;

    public ContentDomainMapper() {
        _commentDomainMapper = new CommentDomainMapper();
        _postDomainMapper = new PostDomainMapper();
    }

    public Content Map(ContentDTO input) {
        if (input is Comment) {
            return _commentDomainMapper.Map(input);
        }

        return _postDomainMapper.Map(input);
    }
}