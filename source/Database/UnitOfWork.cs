using SqlSugar;
using Microsoft.Extensions.Logging;

namespace Architecture.Database;

public interface IUnitOfWork
{
    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    SqlSugarScope GetDbScope();

    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    SqlSugarClient GetDbClient();

    /// <summary>
    /// 开始事务
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// 提交事务
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTransaction();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
    {
        _sqlSugarClient = sqlSugarClient;
        _logger = logger;
    }

    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarScope GetDbScope()
    {
        // 必须要as，后边会用到切换数据库操作
        // SqlSugarScope对SqlSugarClient的封装让他支持线程安全，并且在不同上下文自动new 出 SqlSugarClient，在编写代码的时候不需要考虑他线程是否安全
        // 异步情况： 在同一串await 中是一个上下文
        // 同步情况： 在同一个线程是同一个上下文
        // 同一个SqlSugarScope做到了在同一个上下文共享一个对象，不同上下文自动去NEW
        // 官方文档：https://www.donet5.com/Home/Doc?typeId=2362
        return _sqlSugarClient as SqlSugarScope;
    }

    /// <summary>
    /// 获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarClient GetDbClient()
    {
        return _sqlSugarClient as SqlSugarClient;
    }

    /// <summary>
    /// 开始事务
    /// </summary>
    public void BeginTransaction()
    {
        GetDbScope().BeginTran();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTransaction()
    {
        try
        {
            GetDbScope().CommitTran(); //
        }
        catch (Exception ex)
        {
            GetDbScope().RollbackTran();
            _logger.LogError($"{ex.Message}\r\n{ex.InnerException}");
        }
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        GetDbScope().RollbackTran();
    }
}
