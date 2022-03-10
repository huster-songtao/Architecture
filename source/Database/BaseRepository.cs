using Architecture.Domain;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture.Database;

/// <summary>
/// 数据库操作
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// 
    /// </summary>
    private readonly SqlSugarScope _client;

    public SqlSugarScope Client
    {
        get { return _client; }
    }

    public BaseRepository(IUnitOfWork unitOfWork)
    {
        _client = unitOfWork.GetDbScope();
        //_client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符</param>
    /// <returns>返回实体信息</returns>
    public TEntity Get(object id)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
        //return _client.Queryable<TEntity>().In(id).Single();
        return _client.Queryable<TEntity>().InSingle(id);
    }

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符</param>
    /// <returns>返回实体信息</returns>
    public async Task<TEntity> GetAsync(object id)
    {
        return await _client.Queryable<TEntity>().In(id).SingleAsync();
    }

    /// <summary>
    /// 根据实体信息唯一标识符查询一条实体信息
    /// </summary>
    /// <param name="id">实体信息唯一标识符（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
    /// <param name="isCache">是否使用缓存</param>
    /// <param name="cacheDurationInSeconds">缓存时间</param>
    /// <returns>数据实体</returns>
    public async Task<TEntity> GetAsync(object id, bool isCache = false, int cacheDurationInSeconds = int.MaxValue)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
        return await _client.Queryable<TEntity>().WithCacheIF(isCache, cacheDurationInSeconds).In(id).SingleAsync();
    }

    /// <summary>
    /// 根据实体信息唯一标识符集合查询实体信息
    /// </summary>
    /// <param name="ids">实体信息唯一标识符集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
    /// <returns>返回实体信息集合</returns>
    public List<TEntity> Get(object[] ids)
    {
        return _client.Queryable<TEntity>().In(ids).ToList();
    }

    /// <summary>
    /// 根据实体信息唯一标识符集合查询实体信息
    /// </summary>
    /// <param name="ids">实体信息唯一标识符集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> GetAsync(object[] ids)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
        return await _client.Queryable<TEntity>().In(ids).ToListAsync();
    }

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回实体信息</returns>
    public TEntity Get(Expression<Func<TEntity, bool>> where)
    {
        return _client.Queryable<TEntity>().Where(where).First();
    }

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回实体信息</returns>
    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _client.Queryable<TEntity>().Where(where).FirstAsync();
    }

    /// <summary>
    /// 根据查询表达式判断实体信息是否存在
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>存在实体信息则返回True，否则返回False</returns>
    public bool Any(Expression<Func<TEntity, bool>> where)
    {
        return _client.Queryable<TEntity>().Where(where).Any();
    }

    /// <summary>
    /// 根据查询表达式判断实体信息是否存在
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>存在实体信息则返回True，否则返回False</returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _client.Queryable<TEntity>().Where(where).AnyAsync();
    }

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>返回数据库受影响的行数</returns>
    public int Add(TEntity entity)
    {
        return _client.Insertable(entity).ExecuteCommand();
    }

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="model">实体信息</param>
    /// <returns>返回数据库受影响的行数</returns>
    public async Task<int> AddAsync(TEntity entity)
    {
        return await _client.Insertable(entity).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>返回实体信息的唯一标识符</returns>
    public TEntity Insert(TEntity entity)
    {
        return _client.Insertable(entity).ExecuteReturnEntity();
    }

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="model">实体信息</param>
    /// <returns>返回数据库受影响的行数</returns>
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        // 返回TEntity，这样的话就可以获取id值，无论主键是什么类型
        return await _client.Insertable(entity).ExecuteReturnEntityAsync();
    }

    /// <summary>
    /// 新增实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="columns">指定只插入的字段</param>
    /// <returns>返回实体信息的唯一标识符</returns>
    public async Task<TEntity> InsertAsync(TEntity entity, Expression<Func<TEntity, object>> columns = null)
    {
        if (columns == null)
        {
            return await _client.Insertable(entity).ExecuteReturnEntityAsync();
        }
        else
        {
            return await _client.Insertable(entity).InsertColumns(columns).ExecuteReturnEntityAsync();
        }
    }
    
    /// <summary>
    /// 批量插入实体信息(速度快)
    /// </summary>
    /// <param name="entities">实体信息集合</param>
    /// <returns>影响行数</returns>
    public int Insert(List<TEntity> entities)
    {
        return _client.Insertable(entities.ToArray()).ExecuteCommand();
    }

    /// <summary>
    /// 批量插入实体信息(速度快)
    /// </summary>
    /// <param name="entities">实体信息集合</param>
    /// <returns>影响行数</returns>
    public async Task<int> InsertAsync(List<TEntity> entities)
    {
        return await _client.Insertable(entities.ToArray()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public bool Update(TEntity entity)
    {
        //var rows = _client.Updateable(entity).ExecuteCommand();
        //return rows > 0 ? true : false;
        return _client.Updateable(entity).ExecuteCommandHasChange();
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> UpdateAsync(TEntity entity)
    {
        ////这种方式会以主键为条件
        //var i = await Task.Run(() => _db.Updateable(entity).ExecuteCommand());
        //return i > 0;
        //这种方式会以主键为条件
        return await _client.Updateable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public bool Update(TEntity entity, string where, object parameters = null)
    {
        return _client.Updateable(entity).Where(where, parameters).ExecuteCommandHasChange();
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> UpdateAsync(TEntity entity, string where, object parameters = null)
    {
        //return await Task.Run(() => _db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
        return await _client.Updateable(entity).Where(where, parameters).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public bool Update(string sql, List<SugarParameter> parameters)
    {
        return _client.Ado.ExecuteCommand(sql, parameters) > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public bool Update(string sql, params SugarParameter[] parameters)
    {
        return _client.Ado.ExecuteCommand(sql, parameters) > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public bool Update(string sql, object parameters)
    {
        return _client.Ado.ExecuteCommand(sql, parameters) > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> UpdateAsync(string sql, params SugarParameter[] parameters)
    {
        return await _client.Ado.ExecuteCommandAsync(sql, parameters) > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> UpdateAsync(string sql, List<SugarParameter> parameters)
    {
        return await _client.Ado.ExecuteCommandAsync(sql, parameters) > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="parameters">参数信息</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> UpdateAsync(string sql, object parameters)
    {
        return await _client.Ado.ExecuteCommandAsync(sql, parameters) > 0;
    }

    public async Task<bool> Update(object operateAnonymousObjects)
    {
        return await _client.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <param name="columns">更新的字段</param>
    /// <param name="ignoreColumns">忽略的字段</param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>实体信息有更新则返回True，否则返回False</returns>
    public async Task<bool> Update(TEntity entity, List<string> columns = null, List<string> ignoreColumns = null, string where = "")
    {
        //IUpdateable<TEntity> up = await Task.Run(() => _db.Updateable(entity));
        //if (ignoreColumns != null && ignoreColumns.Count > 0)
        //{
        //    up = await Task.Run(() => up.IgnoreColumns(it => ignoreColumns.Contains(it)));
        //}
        //if (lstColumns != null && lstColumns.Count > 0)
        //{
        //    up = await Task.Run(() => up.UpdateColumns(it => lstColumns.Contains(it)));
        //}
        //if (!string.IsNullOrEmpty(strWhere))
        //{
        //    up = await Task.Run(() => up.Where(strWhere));
        //}
        //return await Task.Run(() => up.ExecuteCommand()) > 0;

        IUpdateable<TEntity> up = _client.Updateable(entity);
        if (ignoreColumns != null && ignoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(ignoreColumns.ToArray());
        }
        if (columns != null && columns.Count > 0)
        {
            up = up.UpdateColumns(columns.ToArray());
        }
        if (!string.IsNullOrEmpty(where))
        {
            up = up.Where(where);
        }
        return await up.ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 删除实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    public bool Delete(TEntity entity)
    {
        return _client.Deleteable(entity).ExecuteCommandHasChange();
    }

    /// <summary>
    /// 删除实体信息
    /// </summary>
    /// <param name="entity">实体信息</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    public async Task<bool> DeleteAsync(TEntity entity)
    {
        //var i = await Task.Run(() => _db.Deleteable(entity).ExecuteCommand());
        //return i > 0;
        return await _client.Deleteable(entity).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 根据实体信息的唯一标识符 id 删除实体信息
    /// </summary>
    /// <param name="id">实体信息的唯一标识符</param>
    /// <returns>实体信息删除成功返回True，否则返回False</returns>
    public async Task<bool> DeleteAsync(object id)
    {
        //var i = await Task.Run(() => _db.Deleteable<TEntity>(id).ExecuteCommand());
        //return i > 0;
        return await _client.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 批量删除指定唯一标识符集合的实体信息
    /// </summary>
    /// <param name="ids">实体信息的唯一标识符集合</param>
    /// <returns>实体信息批量删除成功返回True，否则返回False</returns>
    public async Task<bool> DeleteByIds(object[] ids)
    {
        //var i = await Task.Run(() => _db.Deleteable<TEntity>().In(ids).ExecuteCommand());
        //return i > 0;
        return await _client.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
    }

    /// <summary>
    /// 查询所有实体信息
    /// </summary>
    /// <returns>返回所有实体信息</returns>
    public async Task<List<TEntity>> Query()
    {
        return await _client.Queryable<TEntity>().ToListAsync();
    }

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(string where)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        return await _client.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
    }

    /// <summary>
    /// 根据查询表达式查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如.Where(it=>it.id>0)</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where)
    {
        return await _client.Queryable<TEntity>().WhereIF(where != null, where).ToListAsync();
    }

    /// <summary>
    /// 根据查询表达式查询一条实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回一条实体信息</returns>
    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _client.Queryable<TEntity>().WhereIF(where != null, where).SingleAsync();
    }

    /// <summary>
    /// 根据查询表达式查询一条实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <returns>返回一条实体信息</returns>
    public async Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _client.Queryable<TEntity>().WhereIF(where != null, where).SingleAsync();
    }

    /// <summary>
    /// 查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, OrderByType type = OrderByType.Desc)
    {
        return await _client.Queryable<TEntity>().WhereIF(where != null, where).OrderByIF(order != null, order, type).ToListAsync();
    }

    /// <summary>
    /// 查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序</param>
    /// <param name="isAsc">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 isAsc = false。</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, bool isAsc = true)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(order != null, order, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(where != null, where).ToList());
        return await _client.Queryable<TEntity>().OrderByIF(order != null, order, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(where != null, where).ToListAsync();
    }

    /// <summary>
    /// 按照特定列查询数据集
    /// </summary>
    /// <typeparam name="TResult">从实体信息表中选取指定的数据</typeparam>
    /// <param name="select">选取表达式，从实体信息表中选取数据，例如(Entity, Result) => new Result{ Id = Entity.UserNo, Id1 = Entity.UserNo}</param>
    /// <returns>返回结果集</returns>
    public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> select)
    {
        return await _client.Queryable<TEntity>().Select(select).ToListAsync();
    }

    /// <summary>
    /// 按照特定列查询数据集，带查询表达式和排序
    /// </summary>
    /// <typeparam name="TResult">从实体信息表中选取指定的数据</typeparam>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="select">选取表达式，从实体信息表中选取数据，例如(Entity, Result) => new Result{ Id = Entity.UserNo, Id1 = Entity.UserNo}</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns></returns>
    public async Task<List<TResult>> Query<TResult>(
        Expression<Func<TEntity, TResult>> select, 
        Expression<Func<TEntity, bool>> where, 
        string order)
    {
        return await _client.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(where != null, where).Select(select).ToListAsync();
    }

    /// <summary>
    /// 查询一个列表
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where, string order)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(where != null, where).ToList());
        return await _client.Queryable<TEntity>().WhereIF(where != null, where).OrderByIF(order != null, order).ToListAsync();
    }

    /// <summary>
    /// 查询一个列表
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(string where, string order)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        return await _client.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
    }

    /// <summary>
    /// 查询前N条数据
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="top">前N条</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where,
        int top,
        string order)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(where != null, where).Take(intTop).ToList());
        return await _client.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(where != null, where).Take(top).ToListAsync(); // .SingleAsync();
    }

    /// <summary>
    /// 查询前N条数据
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="top">前N条</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(string where, int top, string order)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(!string.IsNullOrEmpty(where), where).Take(intTop).ToList());
        return await _client.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(!string.IsNullOrEmpty(where), where).Take(top).ToListAsync();
    }

    /// <summary>
    /// 根据sql语句查询
    /// </summary>
    /// <param name="sql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
    {
        return await _client.Ado.SqlQueryAsync<TEntity>(sql, parameters);
    }

    /// <summary>
    /// 根据sql语句查询
    /// </summary>
    /// <param name="sql">完整的sql语句</param>
    /// <param name="parameters">参数</param>
    /// <returns>DataTable</returns>
    public async Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null)
    {
        return await _client.Ado.GetDataTableAsync(sql, parameters);
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(
        Expression<Func<TEntity, bool>> where, 
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20, 
        OrderByType type = OrderByType.Desc)
    {
        //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(order), order).WhereIF(where != null, where).ToPageList(intPageIndex, intPageSize));
        return await _client.Queryable<TEntity>()
            .WhereIF(where != null, where)
            .OrderByIF(order != null, order, type)
            .ToPageListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(
        Expression<Func<TEntity, bool>> where,
        string order,
        int pageIndex = 1,
        int pageSize = 20)
    {
        return await _client.Queryable<TEntity>()
            .WhereIF(where != null, where)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .ToPageListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<List<TEntity>> Query(
        string where,
        string order,
        int pageIndex = 1,
        int pageSize = 20
    )
    {
        return await _client.Queryable<TEntity>()
            .WhereIF(!string.IsNullOrEmpty(where), where)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .ToPageListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    public Pagination<TEntity> QueryPage(
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20,
        OrderByType type = OrderByType.Desc)
    {
        int total = 0;
        var entities = _client.Queryable<TEntity>()
            .WhereIF(where != null, where)
            .OrderByIF(order != null, order, type)
            .ToPageList(pageIndex, pageSize, ref total);
        return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="type">默认按照升序对实体信息结果集进行排序。如果您希望按照降序对实体信息结果集进行排序，可以使用 OrderByType.Desc。</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<Pagination<TEntity>> QueryPage(
        List<IConditionalModel> where,
        Expression<Func<TEntity, object>> order,
        int pageIndex = 1,
        int pageSize = 20,
        OrderByType type = OrderByType.Desc)
    {
        // and id=100 and (id=1 or id=2 and id=1) 
        //List<IConditionalModel> conModels;
        //conModels.Add(new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "100" });
        //conModels.Add(new ConditionalCollections()
        //{
        //    ConditionalList =
        //new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
        //{
        //    new  KeyValuePair<WhereType, ConditionalModel>
        //    ( WhereType.And ,
        //    new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "1" }),
        //    new  KeyValuePair<WhereType, ConditionalModel>
        //    (WhereType.Or,
        //    new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "2" }),
        //    new  KeyValuePair<WhereType, ConditionalModel>
        //    ( WhereType.And,
        //    new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "2" })
        //}
        //});
        //var student = _client.Queryable<Profile>().Where(conModels).ToList();

        RefAsync<int> total = 0;
        var entities = await _client.Queryable<TEntity>()
            .Where(where)
            .OrderByIF(order != null, order, type)
            .ToPageListAsync(pageIndex, pageSize, total);
        return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<Pagination<TEntity>> QueryPage(Expression<Func<TEntity, bool>> where, string order, int pageIndex = 1, int pageSize = 20)
    {
        RefAsync<int> total = 0;
        var entities = await _client.Queryable<TEntity>()
            .WhereIF(where != null, where)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .ToPageListAsync(pageIndex, pageSize, total);
        return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<Pagination<TEntity>> QueryPage(string where, string order, int pageIndex = 1, int pageSize = 20)
    {
        RefAsync<int> total = 0;
        var entities = await _client.Queryable<TEntity>()
            .WhereIF(!string.IsNullOrEmpty(where), where)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .ToPageListAsync(pageIndex, pageSize, total);
        return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary>
    /// 分页查询实体信息
    /// </summary>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据
    /// 例如：.WhereIF(string.IsNullOrEmpty(input), p=>p.Age>10)
    /// </param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <returns>返回实体信息集合</returns>
    public async Task<Pagination<TEntity>> QueryPage(
        Expression<Func<TEntity, bool>> where, 
        int pageIndex = 1, 
        int pageSize = 20, 
        string order = null
    )
    {
        RefAsync<int> total = 0;
        var entities = await _client.Queryable<TEntity>()
            .WhereIF(where != null, where)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .ToPageListAsync(pageIndex, pageSize, total);

        //int pageCount = (int)Math.Ceiling(total.ToDouble() / pageSize);
        return new Pagination<TEntity>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary> 
    /// 多表查询
    /// </summary> 
    /// <typeparam name="T">实体1</typeparam> 
    /// <typeparam name="T2">实体2</typeparam> 
    /// <typeparam name="T3">实体3</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="join">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
    /// <param name="select">选取表达式，从实体信息表中选取数据，例如(t1, t2) => new { Id = t1.UserNo, Id1 = t2.UserNo}</param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如(t1, t2) => t1.UserNo == ""</param>
    /// <returns>值</returns>
    public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
        Expression<Func<T, T2, T3, object[]>> join,
        Expression<Func<T, T2, T3, TResult>> select,
        Expression<Func<T, T2, T3, bool>> where = null) where T : class, new()
    {
        if (where == null)
        {
            return await _client.Queryable(join).Select(select).ToListAsync();
        }
        return await _client.Queryable(join).Where(where).Select(select).ToListAsync();
    }


    /// <summary>
    /// 两表联合查询-分页
    /// </summary>
    /// <typeparam name="T">实体1</typeparam>
    /// <typeparam name="T2">实体1</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="join">关联表达式</param>
    /// <param name="select">选取表达式，从实体信息表中选取数据，例如(t1, t2) => new { Id = t1.UserNo, Id1 = t2.UserNo}</param>
    /// <param name="where">查询表达式，有条件地从实体信息表中选取数据，例如(TResult, bool) => TResult.UserNo == ""</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns></returns>
    public async Task<Pagination<TResult>> QueryTabsPage<T, T2, TResult>(
        Expression<Func<T, T2, object[]>> join,
        Expression<Func<T, T2, TResult>> select,
        Expression<Func<TResult, bool>> where,
        int pageIndex = 1,
        int pageSize = 20,
        string order = null)
    {
        RefAsync<int> total = 0;
        var entities = await _client.Queryable<T, T2>(join)
         .Select(select)
         .OrderByIF(!string.IsNullOrEmpty(order), order)
         .WhereIF(where != null, where)
         .ToPageListAsync(pageIndex, pageSize, total);

        //int pageCount = (int)Math.Ceiling(total.ToDouble() / pageSize);
        return new Pagination<TResult>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    /// <summary>
    /// 两表联合查询-分页-分组
    /// </summary>
    /// <typeparam name="T">实体1</typeparam>
    /// <typeparam name="T2">实体1</typeparam>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <param name="join">关联表达式</param>
    /// <param name="select">选取表达式，从实体信息表中选取数据，例如(t1, t2) => new { Id = t1.UserNo, Id1 = t2.UserNo}</param>
    /// <param name="where">查询表达式</param>
    /// <param name="pageIndex">页码，显示第几页（下标0）</param>
    /// <param name="pageSize">页大小，每页显示多少条记录</param>
    /// <param name="order">排序字段，对实体信息结果集进行排序，例如：DateCreated desc，例如：name asc,age desc</param>
    /// <returns></returns>
    public async Task<Pagination<TResult>> QueryTabsPage<T, T2, TResult>(
        Expression<Func<T, T2, object[]>> join,
        Expression<Func<T, T2, TResult>> select,
        Expression<Func<TResult, bool>> where,
        Expression<Func<T, object>> group,
        int pageIndex = 1,
        int pageSize = 20,
        string order = null)
    {

        RefAsync<int> total = 0;
        var entities = await _client.Queryable<T, T2>(join)
            .GroupBy(group)
            .Select(select)
            .OrderByIF(!string.IsNullOrEmpty(order), order)
            .WhereIF(where != null, where)
            .ToPageListAsync(pageIndex, pageSize, total);
        return new Pagination<TResult>() { Total = total, PageIndex = pageIndex, PageSize = pageSize, Entities = entities };
    }

    //var exp = Expressionable.Create<ProjectToUser>()
    //        .And(s => s.tdIsDelete != true)
    //        .And(p => p.IsDeleted != true)
    //        .And(p => p.pmId != null)
    //        .AndIF(!string.IsNullOrEmpty(model.paramCode1), (s) => s.uID == model.paramCode1.ObjToInt())
    //                .AndIF(!string.IsNullOrEmpty(model.searchText), (s) => (s.groupName != null && s.groupName.Contains(model.searchText))
    //                        || (s.jobName != null && s.jobName.Contains(model.searchText))
    //                        || (s.uRealName != null && s.uRealName.Contains(model.searchText)))
    //                .ToExpression();//拼接表达式
    //var data = await _projectMemberServices.QueryTabsPage<sysUserInfo, ProjectMember, ProjectToUser>(
    //    (s, p) => new object[] { JoinType.Left, s.uID == p.uId },
    //    (s, p) => new ProjectToUser
    //    {
    //        uID = s.uID,
    //        uRealName = s.uRealName,
    //        groupName = s.groupName,
    //        jobName = s.jobName
    //    }, exp, s => new { s.uID, s.uRealName, s.groupName, s.jobName }, model.currentPage, model.pageSize, model.orderField + " " + model.orderType);
}
