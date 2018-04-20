using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSolution.Domain.EntityConfigs
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("T_Menus").HasQueryFilter(i => !i.IsDeleted);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).HasMaxLength(50).IsRequired();
            builder.Property(i => i.Url).HasMaxLength(1024);
            builder.Property(i => i.Icon).HasMaxLength(100);
            builder.Property(i => i.ClassName).HasMaxLength(100);
            builder.Property(i => i.OrderIn);
            builder.Property(i => i.CustomData);
            builder.HasOne(i => i.CreatorUser).WithMany().HasForeignKey(i => i.CreatorUserId);
            builder.HasOne(i => i.DeleterUser).WithMany().HasForeignKey(i => i.DeleterUserId);
        }
    }
}
