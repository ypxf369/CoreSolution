using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using CoreSolution.Tools.Extensions;

namespace CoreSolution.EntityFrameworkCore
{
    /// <summary>
    /// 种子数据
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(EfCoreDbContext context)
        {
            var user = new User
            {
                UserName = "admin",
                RealName = "admin",
                Salt = "123456",
                Password = ("123456".ToMd5() + "123456").ToMd5()
            };
            var role = new Role
            {
                Name = "Admin",
                Description = "管理员"
            };
            var userRole = new UserRole
            {
                User = user,
                Role = role
            };
            var permission = new Permission
            {
                Name = "All",
                Description = "All",
                Role = role
            };
            context.Users.Add(user);
            context.Roles.Add(role);
            context.UserRoles.Add(userRole);
            context.Permissions.Add(permission);
            context.SaveChanges();
        }
    }
}
