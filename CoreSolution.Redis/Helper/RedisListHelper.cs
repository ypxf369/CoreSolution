using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSolution.Redis.Helper
{
    public partial class RedisHelper
    {
        #region 同步方法

        public static void ListRemove<T>(string key, T value)
        {
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        public static List<T> ListRange<T>(string key)
        {
            return Do(redis =>
            {
                var values = redis.ListRange(key);
                return ConvertList<T>(values);
            });
        }

        public static void ListRightPush<T>(string key, T value)
        {
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        public static T ListRightPop<T>(string key)
        {
            return Do(db =>
            {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            });
        }

        public static void ListLeftPush<T>(string key, T value)
        {
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        public static T ListLeftPop<T>(string key)
        {
            return Do(db =>
            {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            });
        }

        public static long ListLength(string key)
        {
            return Do(redis => redis.ListLength(key));
        }

        #endregion
        #region 异步方法

        public static async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)));
        }

        public static async Task<List<T>> ListRangeAsync<T>(string key)
        {
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvertList<T>(values);
        }

        public static async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)));
        }

        public static async Task<T> ListRightPopAsync<T>(string key)
        {
            var value = await Do(db => db.ListRightPopAsync(key));
            return ConvertObj<T>(value);
        }

        public static async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        public static async Task<T> ListLeftPopAsync<T>(string key)
        {
            var value = await Do(db => db.ListLeftPopAsync(key));
            return ConvertObj<T>(value);
        }

        public static async Task<long> ListLengthAsync(string key)
        {
            return await Do(redis => redis.ListLengthAsync(key));
        }

        #endregion
    }
}
