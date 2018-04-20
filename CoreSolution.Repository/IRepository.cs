using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Repository
{
    public interface IRepository
    {
    }
    public interface IRepository<TEntity, TEntityDto> : IRepository<TEntity, TEntityDto, int> where TEntity : class, IEntity<int>
    {

    }

    /// <summary>
    /// 泛型Repository
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TEntityDto">EntityDto</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IRepository<TEntity, TEntityDto, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// IQueryable-GetAll
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <returns>List EntityDto</returns>
        List<TEntityDto> GetAllList();

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <returns>List EntityDto</returns>
        Task<List<TEntityDto>> GetAllListAsync();

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>List EntityDto</returns>
        List<TEntityDto> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>List EntityDto</returns>
        Task<List<TEntityDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="totalCount">总数据条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        List<TEntityDto> GetPaged<TProperty>(out int totalCount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> orderBy, int pageIndex, int pageSize, bool isDesc = false);

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        Task<Tuple<int, List<TEntityDto>>> GetPagedAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> orderBy, int pageIndex, int pageSize, bool isDesc = false);

        /// <summary>
        /// Sql分页方法
        /// </summary>
        /// <param name="totalCount">总数据条数</param>
        /// <param name="sql">分页Sql</param>
        /// <param name="orderBy">排序规则</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        List<TEntityDto> GetPaged(out int totalCount, string sql, string orderBy, int pageIndex, int pageSize, params object[] parameters);

        /// <summary>
        /// Sql分页方法
        /// </summary>
        /// <param name="sql">分页Sql</param>
        /// <param name="orderBy">排序规则</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        Task<Tuple<int, List<TEntityDto>>> GetPagedAsync(string sql, string orderBy, int pageIndex, int pageSize, params object[] parameters);

        /// <summary>
        /// 执行原始Sql查询
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响的行数</returns>
        int ExecuteSql(string sql, params object[] parameters);

        /// <summary>
        /// 执行原始Sql查询
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响的行数</returns>
        Task<int> ExecuteSqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="queryMethod">lambda</param>
        /// <returns></returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// 根据主键获得一个实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>EntityDto</returns>
        TEntityDto Get(TPrimaryKey id);

        /// <summary>
        /// 根据主键获得一个实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>EntityDto</returns>
        Task<TEntityDto> GetAsync(TPrimaryKey id);

        /// <summary>
        /// 根据条件获得Single EntityDto
        /// </summary>
        /// <param name="predicate">lambda</param>
        TEntityDto Single(Expression<Func<TEntity, bool>> predicate);

        TEntityDto SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获得Single EntityDto
        /// </summary>
        /// <param name="predicate">lambda</param>
        Task<TEntityDto> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntityDto> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获得FirstOrDefault EntityDto
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>EntityDto or null</returns>
        TEntityDto FirstOrDefault(TPrimaryKey id);

        /// <summary>
        /// 根据主键获得FirstOrDefault Entity
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>EntityDto or null</returns>
        Task<TEntityDto> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// 根据条件获得FirstOrDefault EntityDto
        /// </summary>
        /// <param name="predicate">lambda</param>
        TEntityDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获得FirstOrDefault EntityDto
        /// </summary>
        /// <param name="predicate">lambda</param>
        Task<TEntityDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 在没有数据库访问的情况下创建具有给定主键的实体。
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>EntityDto</returns>
        TEntityDto Load(TPrimaryKey id);

        Task<TEntityDto> LoadAsync(TPrimaryKey id);

        #endregion

        #region Insert

        /// <summary>
        /// 插入一个Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        TEntityDto Insert(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        Task<TEntityDto> InsertAsync(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity并获得Id
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        /// <returns>Id</returns>
        TPrimaryKey InsertAndGetId(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity并获得Id
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        /// <returns>Id</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity或者根据Id更新
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        TEntityDto InsertOrUpdate(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity或者根据Id更新
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        Task<TEntityDto> InsertOrUpdateAsync(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity或者根据Id更新并返回Id
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        /// <returns>Id</returns>
        TPrimaryKey InsertOrUpdateAndGetId(TEntityDto entityDto);

        /// <summary>
        /// 插入一个Entity或者根据Id更新并返回Id
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        /// <returns>Id</returns>
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntityDto entityDto);

        #endregion

        #region Update

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        TEntityDto Update(TEntityDto entityDto);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        Task<TEntityDto> UpdateAsync(TEntityDto entityDto);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="updateAction">change entity</param>
        /// <returns></returns>
        TEntityDto Update(TPrimaryKey id, Action<TEntityDto> updateAction);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        Task<TEntityDto> UpdateAsync(TPrimaryKey id, Func<TEntityDto, Task> updateAction);

        #endregion

        #region Delete

        /// <summary>
        /// 删除一个Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        void Delete(TEntityDto entityDto);

        /// <summary>
        /// 删除一个Entity
        /// </summary>
        /// <param name="entityDto">EntityDto</param>
        Task DeleteAsync(TEntityDto entityDto);

        /// <summary>
        /// 根据Id删除一个Entity
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// 根据Id删除一个Entity
        /// </summary>
        /// <param name="id">Id</param>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// 根据条件删除一个Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件删除一个Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Aggregates

        /// <summary>
        ///get count
        /// </summary>
        /// <returns>int</returns>
        int Count();

        /// <summary>
        /// get count
        /// </summary>
        /// <returns>int</returns>
        Task<int> CountAsync();

        /// <summary>
        /// 根据条件获取count
        /// </summary>
        /// <param name="predicate">lambda</param>
        /// <returns>int</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取count
        /// </summary>
        /// <param name="predicate">lambda</param>
        /// <returns>int</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get longCount
        /// </summary>
        /// <returns>long</returns>
        long LongCount();

        /// <summary>
        /// get longCount
        /// </summary>
        /// <returns>long</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// 根据条件获取longCount
        /// </summary>
        /// <param name="predicate">lambda</param>
        /// <returns>long</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取longCount
        /// </summary>
        /// <param name="predicate">lambda</param>
        /// <returns>long</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion
    }
}
