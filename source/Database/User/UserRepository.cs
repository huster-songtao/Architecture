using Architecture.Domain;
using Architecture.Model;
using Architecture.Objects;
using SqlSugar;

namespace Architecture.Database;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public Task<long> GetAuthIdByUserIdAsync(long id)
    {
        return Client.Queryable<User>()
            .Where(UserExpression.Id(id))
            .Select(UserExpression.AuthId)
            .SingleAsync();
    }

    public Task<UserModel> GetModelAsync(long id)
    {
        return Client.Queryable<User>()
            .Where(UserExpression.Id(id))
            .Select(UserExpression.Model)
            .SingleAsync();
    }

    public async Task<Grid<UserModel>> GridAsync(GridParameters parameters)
    {
        //RefAsync<int> total = 0;
        //var entities = await Client.Queryable<User>()
        //    .Select(UserExpression.Model)
        //    //.WhereIF(!string.IsNullOrEmpty(where), where)
        //    //.OrderByIF(!string.IsNullOrEmpty(order), order)
        //    .ToPageListAsync(pageIndex, pageSize, total);
        //return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };

        return await Client.Queryable<User>()
            .Select(UserExpression.Model)
            .GridAsync(parameters);
    }

    public async Task<IEnumerable<UserModel>> ListModelAsync()
    {
        return await Client.Queryable<User>()
            .Select(UserExpression.Model)
            .ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(User user)
    {
        return await Client.Updateable(user).UpdateColumns(u => new { user.Id, user.Status }).ExecuteCommandHasChangeAsync();
    }
}
