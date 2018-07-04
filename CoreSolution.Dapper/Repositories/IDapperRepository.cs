using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;
using CoreSolution.Repository;

namespace CoreSolution.Dapper.Repositories
{
    public interface IDapperRepository<TEntity, TEntityDto> : IDapperRepository<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
    {

    }
    public interface IDapperRepository<TEntity, TEntityDto, TPrimaryKey> : IRepository<TEntity, TEntityDto, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}
