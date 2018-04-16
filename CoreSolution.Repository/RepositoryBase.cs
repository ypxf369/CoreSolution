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
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Repository
{
    public abstract class RepositoryBase<TEntity, TEntityDto, TPrimaryKey> : IRepository<TEntity, TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        public object DbContext { get; set; }

        public abstract IQueryable<TEntity> GetAll();

        public virtual List<TEntityDto> GetAllList()
        {
            return GetAll().ProjectTo<TEntityDto>().ToList();
        }

        public virtual Task<List<TEntityDto>> GetAllListAsync()
        {
            return GetAll().ProjectTo<TEntityDto>().ToListAsync();
        }

        public virtual List<TEntityDto> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().ToList();
        }

        public virtual Task<List<TEntityDto>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().ToListAsync();
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
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().Single();
        }

        public virtual TEntityDto SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().SingleOrDefault();
        }

        public virtual async Task<TEntityDto> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ProjectTo<TEntityDto>().SingleAsync();
        }

        public virtual async Task<TEntityDto> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ProjectTo<TEntityDto>().SingleOrDefaultAsync();
        }

        public virtual TEntityDto FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().Where(CreateEqualityExpressionForId(id)).ProjectTo<TEntityDto>().FirstOrDefault();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().Where(CreateEqualityExpressionForId(id)).ProjectTo<TEntityDto>().FirstOrDefaultAsync();
        }

        public virtual TEntityDto FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ProjectTo<TEntityDto>().FirstOrDefault();
        }

        public virtual async Task<TEntityDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ProjectTo<TEntityDto>().FirstOrDefaultAsync();
        }

        public virtual TEntityDto Load(TPrimaryKey id)
        {
            return Get(id);
        }

        public abstract TEntityDto Insert(TEntityDto entityDto);

        public abstract Task<TEntityDto> InsertAsync(TEntityDto entityDto);

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
            var entity = FirstOrDefault(entityDto.Id) == null ? Insert(entityDto) : Update(entityDto);
            return Mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task<TEntityDto> InsertOrUpdateAsync(TEntityDto entityDto)
        {
            var entity = await FirstOrDefaultAsync(entityDto.Id) == null ? await InsertAsync(entityDto) : await UpdateAsync(entityDto);
            return Mapper.Map<TEntityDto>(entity);
        }

        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntityDto entityDto)
        {
            return InsertOrUpdate(entityDto).Id;
        }

        public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntityDto entityDto)
        {
            return (await InsertOrUpdateAsync(entityDto)).Id;
        }

        public abstract TEntityDto Update(TEntityDto entityDto);

        public abstract Task<TEntityDto> UpdateAsync(TEntityDto entityDto);

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

        public abstract void Delete(TPrimaryKey id);

        public abstract Task DeleteAsync(TPrimaryKey id);

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(Mapper.Map<TEntityDto>(entity));
            }
        }

        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return GetAll().CountAsync();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().CountAsync(predicate);
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return GetAll().LongCountAsync();
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCountAsync(predicate);
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
