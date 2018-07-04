using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using CoreSolution.Tools.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreSolution.Domain.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User).ToPluralize()).HasQueryFilter(i => !i.IsDeleted);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.UserName).HasMaxLength(50).IsRequired();
            builder.Property(i => i.RealName).HasMaxLength(50);
            builder.Property(i => i.Email).HasMaxLength(50);
            builder.Property(i => i.IsEmailConfirmed);
            builder.Property(i => i.PhoneNum).HasMaxLength(20);
            builder.Property(i => i.IsPhoneNumConfirmed);
            builder.Property(i => i.Password).HasMaxLength(100).IsRequired();
            builder.Property(i => i.Salt).HasMaxLength(50).IsRequired();
            builder.Property(i => i.IsDeleted).IsRequired();
            builder.Property(i => i.IsLocked).IsRequired();
            builder.Property(i => i.CreationTime);
            builder.Property(i => i.LastModificationTime);
            builder.Property(i => i.DeletionTime);
            builder.HasOne(i => i.CreatorUser).WithMany().HasForeignKey(i => i.CreatorUserId);
            builder.HasOne(i => i.DeleterUser).WithMany().HasForeignKey(i => i.DeleterUserId);
        }
    }
}
