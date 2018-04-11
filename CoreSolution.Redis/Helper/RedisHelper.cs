using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoreSolution.Redis.Helper
{
    public partial class RedisHelper
    {
        private static int DbNum { get; set; }
        private static string ReadWriteHosts { get; set; }
        private static readonly ConnectionMultiplexer _conn;

        static RedisHelper()
        {
            DbNum = 0;
            ReadWriteHosts = null;
            _conn = string.IsNullOrWhiteSpace(ReadWriteHosts)
                ? RedisConnectionHelpr.Instance
                : RedisConnectionHelpr.GetConnectionMultiplexer(ReadWriteHosts);
        }

        #region 辅助方法

        private static T Do<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(DbNum);
            return func(database);
        }

        private static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            });
            return result;
        }

        private static T ConvertObj<T>(RedisValue value)
        {
            //return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static List<T> ConvertList<T>(RedisValue[] values)
        {
            return values.Select(ConvertObj<T>).ToList();

        }

        /// <summary>
        /// hash使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        private static List<T> ConvertHashList<T>(RedisValue[] values)
        {

            return values.Select(ConvertHashObj<T>).ToList();
        }
        private static T ConvertHashObj<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        #endregion
    }
}
