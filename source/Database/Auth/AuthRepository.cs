using Architecture.Domain;

namespace Architecture.Database;

public sealed class AuthRepository : BaseRepository<Auth>, IAuthRepository
{
    public AuthRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public Task<bool> AnyByLoginAsync(string login)
    {
        return AnyAsync(AuthExpression.Login(login));
    }

    public Task<Auth> GetByLoginAsync(string login)
    {
        return QuerySingleAsync(AuthExpression.Login(login));
    }
}
