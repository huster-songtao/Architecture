using Architecture.Domain;
//using DotNetCore.Repositories;

namespace Architecture.Database;

public interface IAuthRepository : IBaseRepository<Auth>
{
    Task<bool> AnyByLoginAsync(string login);

    Task<Auth> GetByLoginAsync(string login);
}
