using Architecture.Domain;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture.Database;

/// <summary>
/// SQL 语法参考：https://www.w3school.com.cn/sql/sql_syntax.asp
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// SqlsugarClient实体
    /// </summary>
    SqlSugarScope Client { get; }

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符</param>
    /// <returns>返回实体信息</returns>
    TEntity Get(object id);

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符</param>
    /// <returns>返回实体信息</returns>
    Task<TEntity> GetAsync(object id);

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
    /// <param name="isCache">是否使用缓存</param>
    /// <param name="cacheDurationInSeconds">缓存时间</param>
    /// <returns>数据实体</returns>
    Task<TEntity> GetAsync(object id, bool isCache = false, int cacheDurationInSeconds = int.MaxValue);

    /// <summary>
    /// 根据实体信息唯一标识符集合查询实体信息
    /// </summary>
    /// <param name="ids">实体信息唯一标识符集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
    /// <returns>返回实体信息集合</returns>
    List<TEntity> Get(object[] ids);

    /// <summary>
    /// 根据实体信息唯一标识符集合查询实体信息
    /// </summary>
    /// <param name="ids">实体信息唯一标识符集合</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> GetAsync(object[] ids);

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式</param>
    /// <returns>返回实体信息</returns>
    TEntity Get(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式</param>
    /// <returns>返回实体信息</returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 根据查询表达式判断实体信息是否存在
    /// </summary>
    /// <param name="where">查询表达式</param>
    /// <returns>存在实体信息则返回True，否则返回False</returns>
    bool Any(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 根据查询表达式判断实体信息是否存在
    /// </summary>
    /// <param name="where">查询表达式</param>
    /// <returns>存在实体信息则返回True，否则返回False</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>返回数据库受影响的行数</returns>
    int Add(TEntity entity);

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="model">实体信息</param>
    /// <returns>返回数据库受影响的行数</returns>
    Task<int> AddAsync(TEntity entity);

    /// <summary>
    /// 写入实体数据
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>返回实体数据的唯一标识符</returns>
    TEntity Insert(TEntity entity);

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="model">实体信息</param>
    /// <returns>新增实体信息成功返回唯一标识符，否则返回0</returns>
    Task<TEntity> InsertAsync(TEntity model);

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="columns">指定只插入的字段</param>
    /// <returns>返回实体信息的唯一标识符</returns>
    Task<TEntity> InsertAsync(TEntity entity, Expression<Func<TEntity, object>> columns = null);

    /// <summary>
    /// 批量插入实体信息(速度快)
    /// </summary>
    /// <param name="entities">实体信息集合</param>
    /// <returns>返回数据库受影响的行数</returns>
    int Insert(List<TEntity> entities);

    /// <summary>
    /// 批量插入实体信息(速度快)
    /// </summary>
    /// <param name="entities">实体信息集合</param>
    /// <returns>返回数据库受影响的行数</returns>
    Task<int> InsertAsync(List<TEntity> entities);

    /// <summary>
    /// 根据实体信息的唯一标识符 id 删除实体信息
    /// </summary>
    /// <param name="id">实体信息的唯一标识符</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    Task<bool> DeleteAsync(object id);

    /// <summary>
    /// 删除实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    bool Delete(TEntity entity);

    /// <summary>
    /// 删除实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// 批量删除指定唯一标识符集合的实体信息
    /// </summary>
    /// <param name="ids">实体信息的唯一标识符集合</param>
    /// <returns>实体信息批量删除成功返回True，否则返回False</returns>
    Task<bool> DeleteByIds(object[] ids);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> UpdateAsync(TEntity model);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="where">查询表达式</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    bool Update(TEntity entity, string where, object parameters = null);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="where">查询表达式</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> UpdateAsync(TEntity entity, string where, object parameters = null);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    bool Update(string sql, List<SugarParameter> parameters);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    bool Update(string sql, params SugarParameter[] parameters);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    bool Update(string sql, object parameters);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> UpdateAsync(string sql, params SugarParameter[] parameters);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> UpdateAsync(string sql, List<SugarParameter> parameters);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> UpdateAsync(string sql, object parameters);

    Task<bool> Update(object operateAnonymousObjects);

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="columns">更新的字段</param>
    /// <param name="ignoreColumns">忽略的字段</param>
    /// <param name="where">查询表达式</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    Task<bool> Update(TEntity entity, List<string> columns = null, List<string> ignoreColumns = null, string where = "");

    /// <summary>
    /// 查询所有实体信息
    /// </summary>
    /// <returns>返回所有实体信息</returns>
    Task<List<TEntity>> Query();

    /// <summary>
    /// 带sql where查询
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(string where);

    /// <summary>
    /// 根据表达式查询
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

    /// <summary>
    /// 根据查询表达式查询一条实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回一条实体信息</returns>
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 根据查询表达式查询一条实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回一条实体信息</returns>
    Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, OrderByType type = OrderByType.Desc);

    /// <summary>
    /// 查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序</param>
    /// <param name="isAsc">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 isAsc = false。</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

    /// <summary>
    /// 根据表达式，指定返回对象模型，查询
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);

    /// <summary>
    /// 根据表达式，指定返回对象模型，排序，查询
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="select"></param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where, string order);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, string order);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(string where, string order);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="top"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, int top, string order);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="top"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<List<TEntity>> Query(string where, int top, string order);

    Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null);
    Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> Query(
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20,
        OrderByType type = OrderByType.Desc);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> Query(
        Expression<Func<TEntity, bool>> where,
        string order,
        int pageIndex = 1,
        int pageSize = 20);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    Task<List<TEntity>> Query(string where, string order, int pageIndex = 1, int pageSize = 20);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    Task<Pagination<TEntity>> QueryPage(string where, string order, int pageIndex = 1, int pageSize = 20);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    Pagination<TEntity> QueryPage(
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20,
        OrderByType type = OrderByType.Desc);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    Task<Pagination<TEntity>> QueryPage(
        List<IConditionalModel> where,
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20,
        OrderByType type = OrderByType.Desc);

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    Task<Pagination<TEntity>> QueryPage(Expression<Func<TEntity, bool>> where, string order, int pageIndex = 1, int pageSize = 20);

    /// <summary>
    /// 三表联查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="join"></param>
    /// <param name="select"></param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如(T, T2, T3, bool) => T.UserNo == T2.UserNo AND T2.UserNo == T3.UserNo</param>
    /// <returns></returns>
    Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
        Expression<Func<T, T2, T3, object[]>> join,
        Expression<Func<T, T2, T3, TResult>> select,
        Expression<Func<T, T2, T3, bool>> where = null) where T : class, new();

    /// <summary>
    /// 两表联查-分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="join">
    /// 
    /// (T, T2)=>new object[]
    /// {
    ///     JoinType.Left,
    ///     T2.Id == T.Id
    /// }
    /// </param>
    /// <param name="select"></param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如(TResult, bool) => TResult.UserNo == ""</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <returns></returns>
    Task<Pagination<TResult>> QueryTabsPage<T, T2, TResult>(
        Expression<Func<T, T2, object[]>> join,
        Expression<Func<T, T2, TResult>> select,
        Expression<Func<TResult, bool>> where,
        int pageIndex = 1,
        int pageSize = 20,
        string order = null);

    /// <summary>
    /// 两表联合查询-分页-分组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="join"></param>
    /// <param name="select"></param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如(TResult, bool) => TResult.UserNo == ""</param>
    /// <param name="group"></param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <returns></returns>
    Task<Pagination<TResult>> QueryTabsPage<T, T2, TResult>(
        Expression<Func<T, T2, object[]>> join,
        Expression<Func<T, T2, TResult>> select,
        Expression<Func<TResult, bool>> where,
        Expression<Func<T, object>> group,
        int pageIndex = 1,
        int pageSize = 20,
        string order = null);
}
