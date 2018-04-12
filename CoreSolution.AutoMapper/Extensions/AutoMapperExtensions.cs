using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AutoMapper;
using CoreSolution.AutoMapper.Configuration;

namespace CoreSolution.AutoMapper.Extensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            //Mapper.Initialize(i => i.CreateMap(obj.GetType(), typeof(T)));
            return AutoMapperConfiguration.Mapper.Map<T>(obj);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            if (source == null) return new List<TDestination>();
            //Mapper.Initialize(i => i.CreateMap(source.GetType(), typeof(TDestination)));
            return AutoMapperConfiguration.Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            if (source == null) return new List<TDestination>();
            //Mapper.Initialize(i => i.CreateMap(source.GetType(), typeof(TDestination)));
            return AutoMapperConfiguration.Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;
            //Mapper.Initialize(i => i.CreateMap(source.GetType(), typeof(TDestination)));
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            if (reader == null) return new List<T>();
            //Mapper.Initialize(i => i.CreateMap(reader.GetType(), typeof(T)));
            Mapper.Reset();
            return AutoMapperConfiguration.Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }
    }
}
