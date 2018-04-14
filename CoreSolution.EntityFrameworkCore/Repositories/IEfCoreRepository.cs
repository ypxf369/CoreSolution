using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities.Base;
using CoreSolution.Repository;

namespace CoreSolution.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity, TEntityDto> : IEfCoreRepository<TEntity, TEntityDto, int> where TEntity : class, IEntity<int>
    {

    }

    public interface IEfCoreRepository<TEntity, TEntityDto, TPrimaryKey> : IRepository<TEntity, TEntityDto, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> GetAllIncluding();

        TEntityDto GetIncluding(TPrimaryKey id);

        Task<TEntityDto> GetIncludingAsync(TPrimaryKey id);

        TEntityDto SingleIncluding(Expression<Func<TEntity, bool>> predicate);

        TEntityDto SingleOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate);

        Task<TEntityDto> SingleIncludingAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntityDto> SingleOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate);

        TEntityDto FirstOrDefaultIncluding(TPrimaryKey id);

        Task<TEntityDto> FirstOrDefaultIncludingAsync(TPrimaryKey id);

        TEntityDto FirstOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate);

        Task<TEntityDto> FirstOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate);

        TEntityDto LoadIncluding(TPrimaryKey id);
    }
}
