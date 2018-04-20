using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class AuditLogProfile : Profile, IProfile
    {
        public AuditLogProfile()
        {
            CreateMap<AuditLog, AuditLogDto>();

            CreateMap<AuditLogDto, AuditLog>()
                .ForMember(i => i.IsDeleted, i => i.Ignore());
        }
    }
}
