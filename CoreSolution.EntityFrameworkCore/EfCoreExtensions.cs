using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.EntityFrameworkCore
{
    public static class EfCoreExtensions
    {
        private static bool IsIEntityTypeConfigurationType(Type typeIntf)
        {
            return typeIntf.IsInterface && typeIntf.IsGenericType && typeIntf.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
        }

        public static void ApplyConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            //判断这个类型实现的接口是不是IEntityTypeConfiguration<>类型，因为是泛型的，所以
            //写的就比较麻烦
            var types = assembly.GetTypes().Where(t => !t.IsAbstract && t.GetInterfaces().Any(IsIEntityTypeConfigurationType));
            Type typeModelBuilder = modelBuilder.GetType();
            //MethodInfo methodNonGenericApplyConfiguration = typeModelBuilder.GetMethod(nameof(ModelBuilder.ApplyConfiguration));
            var methodNonGenericApplyConfiguration = typeModelBuilder
                .GetMethods()
                .SingleOrDefault(i => i.Name == nameof(ModelBuilder.ApplyConfiguration) && i.GetParameters()
                .Any(a => a.ParameterType.Name == typeof(IEntityTypeConfiguration<>).Name));

            //var methodNonGenericApplyConfiguration = typeModelBuilder
            //    .GetMethods()
            //    .Where(i => i.Name == nameof(ModelBuilder.ApplyConfiguration));
            //methodNonGenericApplyConfiguration=methodNonGenericApplyConfiguration.Where(a => a.GetParameters().ParameterType == typeof(IEntityTypeConfiguration<>))
            foreach (var type in types)
            {
                var entityTypeConfig = Activator.CreateInstance(type);

                //获取实体的类型
                Type typeEntity = type.GetInterfaces().First(IsIEntityTypeConfigurationType).GenericTypeArguments[0];

                //因为ApplyConfiguration是泛型方法，所以要通过MakeGenericMethod转换为泛型方法才能调用
                MethodInfo methodApplyConfiguration = methodNonGenericApplyConfiguration.MakeGenericMethod(typeEntity);
                methodApplyConfiguration.Invoke(modelBuilder, new[] { entityTypeConfig });
            }
        }
    }
}
