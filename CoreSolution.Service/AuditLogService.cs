using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class AuditLogService : EfCoreRepositoryBase<AuditLog, AuditLogDto, int>, IAuditLogService
    {
        
    }
}
