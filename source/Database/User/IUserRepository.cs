using Architecture.Domain;
using Architecture.Model;
using Architecture.Objects;

namespace Architecture.Database;

public interface IUserRepository : IBaseRepository<User>
{
    Task<long> GetAuthIdByUserIdAsync(long id);

    Task<UserModel> GetModelAsync(long id);

    Task<Grid<UserModel>> GridAsync(GridParameters parameters);

    Task<IEnumerable<UserModel>> ListModelAsync();

    Task<bool> UpdateStatusAsync(User user);
}
