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
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }

    /// <summary>
    /// 泛型Repository
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// IQueryable-GetAll
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// IQueryable-GetAll
        /// </summary>
        /// <param name="propertySelectors">表达式列表</param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <returns>List Entity</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <returns>List Entity</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>List Entity</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// List-GetAll
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>List Entity</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

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
        /// <returns>Entity</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// 根据主键获得一个实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// 根据条件获得Single Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获得Single Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获得FirstOrDefault Entity
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity or null</returns>
        TEntity FirstOrDefault(TPrimaryKey id);

        /// <summary>
        /// 根据主键获得FirstOrDefault Entity
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// 根据条件获得FirstOrDefault Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获得FirstOrDefault Entity
        /// </summary>
        /// <param name="predicate">lambda</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 在没有数据库访问的情况下创建具有给定主键的实体。
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>Entity</returns>
        TEntity Load(TPrimaryKey id);

        #endregion

        #region Insert

        /// <summary>
        /// 插入一个Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 插入一个Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 插入一个Entity并获得Id
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id</returns>
        TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// 插入一个Entity并获得Id
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// 插入一个Entity或者根据Id更新
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// 插入一个Entity或者根据Id更新
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// 插入一个Entity或者根据Id更新并返回Id
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id</returns>
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// 插入一个Entity或者根据Id更新并返回Id
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id</returns>
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion

        #region Update

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="updateAction">change entity</param>
        /// <returns></returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        /// 更新现有的Entity
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        #endregion

        #region Delete

        /// <summary>
        /// 删除一个Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除一个Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

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

        #endregion
    }
}
