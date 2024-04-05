using W3TL.Core.Domain.Common.Repository;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Repository;

public interface IUserRepository : IRepository<global::User, UserID> { }