using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CoreSolution.Redis.Helper
{
    public partial class RedisHelper
    {
        #region 同步方法

        public static bool HashExists(string key, string dataKey)
        {
            return Do(db => db.HashExists(key, dataKey));
        }

        public static bool HashSet<T>(string key, string dataKey, T t)
        {
            return Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        public static bool HashDelete(string key, string dataKey)
        {
            return Do(db => db.HashDelete(key, dataKey));
        }

        public static long HashDelete(string key, List<RedisValue> dataKeys)
        {
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return Do(db => db.HashDelete(key, dataKeys.ToArray()));
        }

        public static T HashGet<T>(string key, string dataKey)
        {
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }

        public static double HashIncrement(string key, string dataKey, double val = 1)
        {
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        public static double HashDecrement(string key, string dataKey, double val = 1)
        {
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        public static List<T> HashKeys<T>(string key)
        {
            return Do(db =>
            {
                RedisValue[] values = db.HashKeys(key);
                return ConvertHashList<T>(values);
            });
        }
        //public static List<T> HashGetAll<T>(string key)
        //{
        //    return Do(db =>
        //    {
        //        var result = new List<T>();
        //        var list = db.HashGetAll(key);
        //        if (list != null)
        //        {
        //            list.ForEach(x =>
        //            {
        //                var value = JsonSerializer.DeserializeFromString<T>(x);
        //                result.Add(value);
        //            });
        //        }
        //    });
        //}

        #endregion

        #region 异步方法

        public static async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        public static async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            return await Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        public static async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        public static async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys)
        {
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        public static async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);
        }

        public static async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        public static async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        public static async Task<List<T>> HashKeysAsync<T>(string key)
        {
            RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            return ConvertList<T>(values);
        }

        #endregion
    }
}
