using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using CoreSolution.AutoMapper.MapProfile;

namespace CoreSolution.AutoMapper.Configuration
{
    public class AutoMapperConfiguration
    {
        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }

        public static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {

                //获取所有IProfile实现类
                var allType = Assembly
                        .GetEntryAssembly()//获取默认程序集
                        .GetReferencedAssemblies()//获取所有引用程序集
                        .Select(Assembly.Load)
                        .SelectMany(y => y.DefinedTypes)
                        .Where(type => typeof(IProfile).GetTypeInfo().IsAssignableFrom(type.AsType()));
                var types = allType.Select(i => i.AsType()).ToArray();

                //注册映射
                global::AutoMapper.Mapper.Initialize(i => i.AddProfiles(types));
            });

            Mapper = MapperConfiguration.CreateMapper();
            MapperConfiguration.AssertConfigurationIsValid();
        }
    }

}
