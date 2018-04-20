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

        public override IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Includ与NoInclud的区别只在于首次加载速度上，之后效率基本持平，Includ比NoInclud首次加载速度快10%左右。
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllIncluding()
        {
            return GetAll();
        }

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

        public override List<TEntityDto> GetPaged(out int totalCount, string sql, string orderBy, int pageIndex, int pageSize, params object[] parameters)
        {
            var totalSql = string.Format("select count(1) from ({0}) total", sql);
            totalCount = ExecuteSql(totalSql, parameters);

            string sqlBuilder = @"select top " + pageSize + " *   "
                                + "from "
                                + "("
                                + "select row_number() over (order by " + orderBy + " ) as RowNumber,* from"
                                + "("
                                + sql
                                + " ) T1 "
                                + ") T2 "
                                + "where RowNumber > " + pageSize + "*(" + (pageIndex - 1) + ") ";
            return GetAll().FromSql(sqlBuilder, parameters).ProjectTo<TEntityDto>().ToList();
        }

        public override async Task<Tuple<int, List<TEntityDto>>> GetPagedAsync(string sql, string orderBy, int pageIndex,
            int pageSize, params object[] parameters)
        {
            var totalSql = string.Format("select count(1) from ({0}) total", sql);
            int totalCount = await ExecuteSqlAsync(totalSql, parameters);

            string sqlBuilder = @"select top " + pageSize + " *   "
                                + "from "
                                + "("
                                + "select row_number() over (order by " + orderBy + " ) as RowNumber,* from"
                                + "("
                                + sql
                                + " ) T1 "
                                + ") T2 "
                                + "where RowNumber > " + pageSize + "*(" + (pageIndex - 1) + ") ";
            var data = await GetAll().FromSql(sqlBuilder, parameters).ProjectTo<TEntityDto>().ToListAsync();
            return new Tuple<int, List<TEntityDto>>(totalCount, data);
        }

        public override int ExecuteSql(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public override Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommandAsync(sql, parameters);
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

        public override TEntityDto Load(TPrimaryKey id)
        {
            var entity = _dbContext.Find<TEntity>(id);
            return Mapper.Map<TEntityDto>(entity);
        }

        public override async Task<TEntityDto> LoadAsync(TPrimaryKey id)
        {
            var entity = await _dbContext.FindAsync<TEntity>(id);
            return Mapper.Map<TEntityDto>(entity);
        }

        public override TEntityDto Insert(TEntityDto entityDto)
        {
            if (entityDto != null)
            {
                _dbContext.Add<TEntity>(Mapper.Map<TEntity>(entityDto));
                _dbContext.SaveChanges();
                return entityDto;
            }
            return null;
        }

        public override async Task<TEntityDto> InsertAsync(TEntityDto entityDto)
        {
            if (entityDto != null)
            {
                await _dbContext.AddAsync<TEntity>(Mapper.Map<TEntity>(entityDto));
                await _dbContext.SaveChangesAsync();
                return entityDto;
            }
            return null;
        }

        public override TEntityDto Update(TEntityDto entityDto)
        {
            if (entityDto != null)
            {
                var dto = Load(entityDto.Id);
                if (dto != null)
                {
                    var entity = Mapper.Map<TEntity>(dto);
                    _dbContext.SaveChanges();
                    return dto;
                }
            }
            return null;
        }

        public override async Task<TEntityDto> UpdateAsync(TEntityDto entityDto)
        {
            if (entityDto != null)
            {
                var dto = await LoadAsync(entityDto.Id);
                if (dto != null)
                {
                    var entity = Mapper.Map<TEntity>(dto);
                    await _dbContext.SaveChangesAsync();
                    return dto;
                }
            }
            return null;
        }

        public override void Delete(TEntityDto entityDto)
        {
            var dto = Load(entityDto.Id);
            if (dto != null)
            {
                var entity = Mapper.Map<TEntity>(dto);
                entity.IsDeleted = true;
                var entry = _dbContext.Entry<TEntity>(entity);
                entry.State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TEntityDto entityDto)
        {
            var dto = await LoadAsync(entityDto.Id);
            if (dto != null)
            {
                var entity = Mapper.Map<TEntity>(dto);
                entity.IsDeleted = true;
                var entry = _dbContext.Entry<TEntity>(entity);
                entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public override void Delete(TPrimaryKey id)
        {
            var dto = Load(id);
            if (dto != null)
            {
                var entity = Mapper.Map<TEntity>(dto);
                entity.IsDeleted = true;
                var entry = _dbContext.Entry<TEntity>(entity);
                entry.State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TPrimaryKey id)
        {
            var dto = await LoadAsync(id);
            if (dto != null)
            {
                var entity = Mapper.Map<TEntity>(dto);
                entity.IsDeleted = true;
                var entry = _dbContext.Entry<TEntity>(entity);
                entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _dbContext.Set<TEntity>().Where(predicate);
            if (entities.Any())
            {
                entities.ToList().ForEach(i =>
                {
                    i.IsDeleted = true;
                    var entry = _dbContext.Entry<TEntity>(i);
                    entry.State = EntityState.Modified;
                });
                _dbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _dbContext.Set<TEntity>().Where(predicate);
            if (await entities.AnyAsync())
            {
                await entities.ForEachAsync(i =>
                {
                    i.IsDeleted = true;
                    var entry = _dbContext.Entry<TEntity>(i);
                    entry.State = EntityState.Modified;
                });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
