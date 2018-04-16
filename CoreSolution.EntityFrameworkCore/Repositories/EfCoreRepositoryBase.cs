using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreSolution.Domain.Entities.Base;
using CoreSolution.Dto.Base;
using CoreSolution.Repository;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.EntityFrameworkCore.Repositories
{
    public abstract class EfCoreRepositoryBase<TEntity, TEntityDto, TPrimaryKey> : RepositoryBase<TEntity, TEntityDto, TPrimaryKey>,
        IEfCoreRepository<TEntity, TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {

        public readonly EfCoreDbContext _dbContext;

        protected EfCoreRepositoryBase()
        {
            DbContext = new EfCoreDbContext();
            _dbContext = (EfCoreDbContext)DbContext;
        }

        /// <summary>
        /// Includ与NoInclud的区别只在于首次加载速度上，之后效率基本持平，Includ比NoInclud首次加载速度快10%左右。
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<TEntity> GetAllIncluding();

        public virtual TEntityDto GetIncluding(TPrimaryKey id)
        {
            var entity = FirstOrDefaultIncluding(id);
            if (entity == null)
            {
                throw new Exception($"未找到id={id}的Entity!");
            }

            return entity;
        }

        public virtual async Task<TEntityDto> GetIncludingAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultIncludingAsync(id);
            if (entity == null)
            {
                throw new Exception($"未找到id={id}的Entity!");
            }

            return entity;
        }

        public virtual TEntityDto SingleIncluding(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().Single();
        }

        public virtual TEntityDto SingleOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().SingleOrDefault();
        }

        public virtual async Task<TEntityDto> SingleIncludingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().SingleAsync();
        }

        public virtual async Task<TEntityDto> SingleOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().SingleOrDefaultAsync();
        }

        public virtual TEntityDto FirstOrDefaultIncluding(TPrimaryKey id)
        {
            return GetAllIncluding().Where(CreateEqualityExpressionForId(id)).ProjectTo<TEntityDto>().FirstOrDefault();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultIncludingAsync(TPrimaryKey id)
        {
            return await GetAllIncluding().Where(CreateEqualityExpressionForId(id)).ProjectTo<TEntityDto>().FirstOrDefaultAsync();
        }

        public virtual TEntityDto FirstOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().FirstOrDefault();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultIncludingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAllIncluding().Where(predicate).ProjectTo<TEntityDto>().FirstOrDefaultAsync();
        }

        public virtual TEntityDto LoadIncluding(TPrimaryKey id)
        {
            return GetIncluding(id);
        }
    }
}
