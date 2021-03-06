﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Entities.Base;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto.Base;
using CoreSolution.Repository;
using CoreSolution.Tools.Extensions;
using Dapper;

namespace CoreSolution.Dapper.Repositories
{
    public abstract class DapperRepositoryBase<TEntity, TEntityDto, TPrimaryKey> : RepositoryBase<TEntity, TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        protected readonly IDbConnection _dbConnection;

        private readonly string _tableName = $"{typeof(TEntity).Name.ToPluralize()}";

        protected DapperRepositoryBase()
        {
            _dbConnection = DbConnectionFactory.CreateDbConnection(DatabaseType.SqlServer);
        }
        [Obsolete]
        public override IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public override List<TEntityDto> GetAllList()
        {
            string sql = $"SELECT * FROM {_dbConnection.Database}..{_tableName} WITH (NOLOCK)";
            var entity = _dbConnection.Query<TEntity>(sql);
            return Mapper.Map<List<TEntityDto>>(entity);
        }
    }
}
