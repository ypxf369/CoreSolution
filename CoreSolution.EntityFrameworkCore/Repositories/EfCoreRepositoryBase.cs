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

        public override List<TEntityDto> GetFromSql(string sql, params object[] parameters)
        {
            return GetAll().FromSql(sql, parameters).ProjectTo<TEntityDto>().ToList();
        }

        public override async Task<List<TEntityDto>> GetFromSqlAsync(string sql, params object[] parameters)
        {
            return await GetAll().FromSql(sql, parameters).ProjectTo<TEntityDto>().ToListAsync();
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
                var entity = _dbContext.Find<TEntity>(entityDto.Id);
                if (entity != null)
                {
                    var inputEntity = Mapper.Map<TEntity>(entityDto);//
                    var entityType = entity.GetType();
                    //var dtoType = entityDto.GetType();
                    var inputType = inputEntity.GetType();//
                    var entityProperties = entityType.GetProperties();
                    var inputProperties = inputType.GetProperties().ToList();
                    foreach (var ePro in entityProperties)
                    {
                        string eName = ePro.Name;
                        switch (eName)
                        {
                            case nameof(Entity<TPrimaryKey>.Id):
                            case nameof(Entity<TPrimaryKey>.IsDeleted):
                            case nameof(ICreationTime.CreationTime):
                                continue;
                            case nameof(IModificationTime.LastModificationTime):
                                ePro.SetValue(entity, DateTime.Now);
                                continue;
                        }
                        foreach (var iPro in inputProperties)
                        {
                            if (eName == iPro.Name)
                            {
                                //var value = dPro.GetValue(entityDto);
                                var ivalue = iPro.GetValue(inputEntity);//
                                var iType = iPro.PropertyType;
                                if (iType == typeof(int) || iType == typeof(long) || iType == typeof(Guid))
                                {
                                    object[] obj = { 0, new Guid() };
                                    if (obj.Contains(ivalue)) break;
                                }
                                if (ivalue != null && ePro.GetValue(entity) != ivalue)
                                {
                                    ePro.SetValue(entity, ivalue);
                                    inputProperties.Remove(iPro);
                                    break;
                                }
                            }
                        }
                    }
                    _dbContext.SaveChanges();
                    return entityDto;
                }
            }
            return null;
        }

        public override async Task<TEntityDto> UpdateAsync(TEntityDto entityDto)
        {
            if (entityDto != null)
            {
                var entity = await _dbContext.FindAsync<TEntity>(entityDto.Id);
                if (entity != null)
                {
                    var inputEntity = Mapper.Map<TEntity>(entityDto);//
                    var entityType = entity.GetType();
                    //var dtoType = entityDto.GetType();
                    var inputType = inputEntity.GetType();//
                    var entityProperties = entityType.GetProperties();
                    var inputProperties = inputType.GetProperties().ToList();
                    foreach (var ePro in entityProperties)
                    {
                        string eName = ePro.Name;
                        switch (eName)
                        {
                            case nameof(Entity<TPrimaryKey>.Id):
                            case nameof(Entity<TPrimaryKey>.IsDeleted):
                            case nameof(ICreationTime.CreationTime):
                                continue;
                            case nameof(IModificationTime.LastModificationTime):
                                ePro.SetValue(entity, DateTime.Now);
                                continue;
                        }
                        foreach (var iPro in inputProperties)
                        {
                            if (eName == iPro.Name)
                            {
                                //var value = dPro.GetValue(entityDto);
                                var ivalue = iPro.GetValue(inputEntity);//
                                var iType = iPro.PropertyType;
                                if (iType == typeof(int) || iType == typeof(long) || iType == typeof(Guid))
                                {
                                    object[] obj = { 0, new Guid() };
                                    if (obj.Contains(ivalue)) break;
                                }
                                if (ivalue != null && ePro.GetValue(entity) != ivalue)
                                {
                                    ePro.SetValue(entity, ivalue);
                                    inputProperties.Remove(iPro);
                                    break;
                                }
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return entityDto;
                }
            }
            return null;
        }

        public override void Delete(TEntityDto entityDto)
        {
            var entity = _dbContext.Find<TEntity>(entityDto.Id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                var entityType = entity.GetType();
                var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                if (deletionTime != null)
                {
                    deletionTime.SetValue(entity, DateTime.Now);
                }
                _dbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TEntityDto entityDto)
        {
            var entity = await _dbContext.FindAsync<TEntity>(entityDto.Id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                var entityType = entity.GetType();
                var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                if (deletionTime != null)
                {
                    deletionTime.SetValue(entity, DateTime.Now);
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = _dbContext.Find<TEntity>(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                var entityType = entity.GetType();
                var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                if (deletionTime != null)
                {
                    deletionTime.SetValue(entity, DateTime.Now);
                }
                _dbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await _dbContext.FindAsync<TEntity>(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                var entityType = entity.GetType();
                var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                if (deletionTime != null)
                {
                    deletionTime.SetValue(entity, DateTime.Now);
                }
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
                    var entityType = i.GetType();
                    var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                    if (deletionTime != null)
                    {
                        deletionTime.SetValue(i, DateTime.Now);
                    }
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
                    var entityType = i.GetType();
                    var deletionTime = entityType.GetProperty(nameof(IDeletionTime.DeletionTime));
                    if (deletionTime != null)
                    {
                        deletionTime.SetValue(i, DateTime.Now);
                    }
                });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
