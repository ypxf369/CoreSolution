using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using CoreSolution.AutoMapper.Extensions;
using CoreSolution.Domain.Entities.Base;
using CoreSolution.Dto.Base;
using CoreSolution.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Repository
{
    public abstract class RepositoryBase<TEntity, TEntityDto, TPrimaryKey> : IRepository<TEntity, TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        public virtual CoreDbContext CoreDbContext { get; set; }
        public abstract IQueryable<TEntity> GetAll();

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            //return GetAll().Include(i => propertySelectors);
            return GetAll();
        }

        public virtual List<TEntityDto> GetAllList()
        {
            return GetAll().ProjectTo<TEntityDto>().ToList();
        }

        public virtual Task<List<TEntityDto>> GetAllListAsync()
        {
            return GetAll().ProjectTo<TEntityDto>().ToListAsync();
            //return Task.FromResult(GetAllList());
        }

        public virtual List<TEntityDto> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().ToList();
        }

        public virtual Task<List<TEntityDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().ToListAsync();
            //return Task.FromResult(GetAllList(predicate));
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual TEntityDto Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new Exception($"未找到id={id}的Entity!");
            }

            return entity;
        }

        public virtual async Task<TEntityDto> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new Exception($"未找到id={id}的Entity!");
            }

            return entity;
        }

        public virtual TEntityDto Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate).MapTo<TEntityDto>();
        }

        public virtual TEntityDto SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefault(predicate).MapTo<TEntityDto>();
        }

        public virtual async Task<TEntityDto> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAll().SingleAsync(predicate)).MapTo<TEntityDto>();
            //return Task.FromResult(Single(predicate));
        }

        public virtual async Task<TEntityDto> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAll().SingleOrDefaultAsync(predicate)).MapTo<TEntityDto>();
        }

        public virtual TEntityDto FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id)).MapTo<TEntityDto>();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return (await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id))).MapTo<TEntityDto>();
            //return Task.FromResult(FirstOrDefault(id));
        }

        public virtual TEntityDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate).MapTo<TEntityDto>();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAll().FirstOrDefaultAsync(predicate)).MapTo<TEntityDto>();
            //return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual TEntityDto Load(TPrimaryKey id)
        {
            return Get(id);
        }

        public abstract TEntityDto Insert(TEntityDto entityDto);

        public abstract Task<TEntityDto> InsertAsync(TEntityDto entityDto);

        //public virtual Task<TEntityDto> InsertAsync(TEntityDto entityDto)
        //{
        //    return Task.FromResult(Insert(entityDto));
        //}

        public virtual TPrimaryKey InsertAndGetId(TEntityDto entityDto)
        {
            return Insert(entityDto).Id;
        }

        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntityDto entityDto)
        {
            return (await InsertAsync(entityDto)).Id;
        }

        public virtual TEntityDto InsertOrUpdate(TEntityDto entityDto)
        {
            return FirstOrDefault(entityDto.Id) == null ? Insert(entityDto).MapTo<TEntityDto>() : Update(entityDto).MapTo<TEntityDto>();
        }

        public virtual async Task<TEntityDto> InsertOrUpdateAsync(TEntityDto entityDto)
        {
            return await FirstOrDefaultAsync(entityDto.Id) == null ? (await InsertAsync(entityDto)).MapTo<TEntityDto>() : (await UpdateAsync(entityDto)).MapTo<TEntityDto>();
        }

        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntityDto entityDto)
        {
            return InsertOrUpdate(entityDto).Id;
        }

        public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntityDto entityDto)
        {
            return (await InsertOrUpdateAsync(entityDto)).Id;
            //return Task.FromResult(InsertOrUpdateAndGetId(entityDto));
        }

        public abstract TEntityDto Update(TEntityDto entityDto);

        public abstract Task<TEntityDto> UpdateAsync(TEntityDto entityDto);
        //public virtual Task<TEntityDto> UpdateAsync(TEntityDto entityDto)
        //{
        //    return Task.FromResult(Update(entityDto));
        //}

        public virtual TEntityDto Update(TPrimaryKey id, Action<TEntityDto> updateAction)
        {
            var entityDto = Get(id);
            updateAction(entityDto);
            return entityDto;
        }

        public virtual async Task<TEntityDto> UpdateAsync(TPrimaryKey id, Func<TEntityDto, Task> updateAction)
        {
            var entityDto = await GetAsync(id);
            await updateAction(entityDto);
            return entityDto;
        }

        public abstract void Delete(TEntityDto entityDto);

        public abstract Task DeleteAsync(TEntityDto entityDto);

        //public virtual Task DeleteAsync(TEntityDto entityDto)
        //{
        //    Delete(entityDto);
        //    return Task.FromResult(0);
        //}

        public abstract void Delete(TPrimaryKey id);

        public abstract Task DeleteAsync(TPrimaryKey id);

        //public virtual Task DeleteAsync(TPrimaryKey id)
        //{
        //    Delete(id);
        //    return Task.FromResult(0);
        //}

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity.MapTo<TEntityDto>());
            }
        }

        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        //public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        //{
        //    Delete(predicate);
        //    return Task.FromResult(0);
        //}

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return GetAll().CountAsync();
            //return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().CountAsync(predicate);
            //return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return GetAll().LongCountAsync();
            //return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCountAsync(predicate);
            //return Task.FromResult(LongCount(predicate));
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
