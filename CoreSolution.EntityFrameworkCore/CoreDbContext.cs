using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.EntityFrameworkCore
{
    public partial class CoreDbContext : DbContext
    {
        //public CoreDbContext()
        //{
        //    Database.EnsureCreated();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DbContextConfigurer.Configure(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*var typesToRegister = Assembly.Load("CoreSolution.Domain").GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(Entity<>));*/
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("CoreSolution.Domain"));
            //foreach (var type in typesToRegister)
            //{
            //    var instance = Activator.CreateInstance(type);
            //    //获取实体的类型
            //    Type typeEntity = type.GetInterfaces().First(t => IsIEntityTypeConfigurationType(t)).GenericTypeArguments[0];
            //    modelBuilder.ApplyConfiguration((typeEntity)instance);
            //}
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
    }
}
