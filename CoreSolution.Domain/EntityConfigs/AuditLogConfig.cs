using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSolution.Domain.EntityConfigs
{
    public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("T_AuditLogs").HasQueryFilter(i => i.IsDeleted);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.UserId);
            builder.Property(i => i.BrowserInfo).HasMaxLength(200);
            builder.Property(i => i.ServiceName).HasMaxLength(200);
            builder.Property(i => i.MethodName).HasMaxLength(100);
            builder.Property(i => i.Parameters).HasMaxLength(1024);
            builder.Property(i => i.ExecutionTime);
            builder.Property(i => i.ClientIpAddress).HasMaxLength(50);
        }
    }
}
