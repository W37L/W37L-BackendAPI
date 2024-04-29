using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.Post.Repository;

public interface IContentRepository : IRepository<Content, ContentIDBase> {
    Task<Result<UserID>> GetAuthorIdAsync(ContentIDBase id);
    Task<Result<Content>> GetByFullIdAsync(ContentIDBase id, global::User user);
}