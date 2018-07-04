using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using CoreSolution.Tools.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSolution.Domain.EntityConfigs
{
    public class MenuItemConfig : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable(nameof(MenuItem).ToPluralize()).HasQueryFilter(i => !i.IsDeleted);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.CustomData);
            builder.Property(i => i.Name).HasMaxLength(50).IsRequired();
            builder.Property(i => i.Url).HasMaxLength(1024);
            builder.Property(i => i.Icon).HasMaxLength(100);
            builder.Property(i => i.ClassName).HasMaxLength(100);
            builder.Property(i => i.OrderIn);
            builder.Property(i => i.RequiredPermissionName).HasMaxLength(1024);
            builder.Property(i => i.RequiresAuthentication);
            builder.HasOne(i => i.CreatorUser).WithMany().HasForeignKey(i => i.CreatorUserId);
            builder.HasOne(i => i.DeleterUser).WithMany().HasForeignKey(i => i.DeleterUserId);
            builder.HasOne(i => i.Menu).WithMany(i => i.MenuItems).HasForeignKey(i => i.MenuId);
            builder.HasMany(i => i.MenuItems).WithOne().HasForeignKey(i => i.MenuItemId);
        }
    }
}
