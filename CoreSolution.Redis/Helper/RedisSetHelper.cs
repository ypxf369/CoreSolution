using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CoreSolution.Redis.Helper
{
    public partial class RedisHelper
    {
        #region Sorted Sets 同步方法

        public static bool SortedSetAdd<T>(string key, T value, double score)
        {
            return Do(redis => redis.SortedSetAdd(key, ConvertJson<T>(value), score));
        }

        public static bool SortedSetRemove<T>(string key, T value)
        {
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        public static List<T> SortedSetRangeByRank<T>(string key)
        {
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key);
                return ConvertList<T>(values);
            });
        }

        public static long SortedSetLength(string key)
        {
            return Do(redis => redis.SortedSetLength(key));
        }

        public static double SortedSetIncrement<T>(string key, T value, double score)
        {
            return Do(redis => redis.SortedSetIncrement(key, ConvertJson(value), score));
        }

        public static double SortedSetDecrement<T>(string key, T value, double score)
        {
            return Do(redis => redis.SortedSetDecrement(key, ConvertJson(value), score));
        }

        #endregion

        #region Sets 同步方法

        public static bool SetAdd<T>(string key, T value, double score)
        {
            return Do(redis => redis.SetAdd(key, ConvertJson<T>(value)));
        }

        public static bool SetRemove<T>(string key, T value)
        {
            return Do(redis => redis.SetRemove(key, ConvertJson(value)));
        }

        public static bool SetContains<T>(string key, T value)
        {
            return Do(redis => redis.SetContains(key, ConvertJson(value)));
        }

        public static long SetLength(string key)
        {
            return Do(redis => redis.SetLength(key));
        }

        public static List<T> SetMembers<T>(string key)
        {
            return Do(redis =>
            {
                var value = redis.SetMembers(key);
                return ConvertList<T>(value);
            });
        }

        public static List<RedisValue> SetMembers(string key)
        {
            return Do(redis =>
            {
                var value = redis.SetMembers(key);
                return value.ToList();
            });
        }

        #endregion

        #region Sets 异步方法

        public static async Task<bool> SetAddAsync<T>(string key, T value)
        {
            return await Do(redis => redis.SetAddAsync(key, ConvertJson<T>(value)));
        }

        public static async Task<bool> SetRemoveAsync<T>(string key, T value)
        {
            return await Do(redis => redis.SetRemoveAsync(key, ConvertJson(value)));
        }

        public static async Task<bool> SetContainsAsync<T>(string key, T value)
        {
            return await Do(redis => redis.SetContainsAsync(key, ConvertJson(value)));
        }

        public static async Task<long> SetLengthAsync(string key)
        {
            return await Do(redis => redis.SetLengthAsync(key));
        }

        public static async Task<List<T>> SetMembersAsync<T>(string key)
        {
            var values = await Do(redis => redis.SetMembersAsync(key));
            return ConvertList<T>(values);
        }

        public static async Task<List<RedisValue>> SetMembersAsync(string key)
        {
            var values = await Do(redis => redis.SetMembersAsync(key));
            return values.ToList();
        }

        #endregion

        #region Sorted Sets 异步方法

        public static async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson<T>(value), score));
        }

        public static async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvertList<T>(values);
        }

        public static async Task<List<SortedSetEntry>> SortedSetRangeByRankWithScoresAsync(string key, Order order, long start = 0, long stop = -1)
        {
            var values = await Do(redis => redis.SortedSetRangeByRankWithScoresAsync(key, start, stop, order));
            return values.ToList();
        }

        public static async Task<long> SortedSetLengthAsync(string key)
        {
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        public static async Task<double> SortedSetIncrementAsync<T>(string key, T value, double score)
        {
            return await Do(redis => redis.SortedSetIncrementAsync(key, ConvertJson(value), score));
        }

        public static async Task<double> SortedSetDecrementAsync<T>(string key, T value, double score)
        {
            return await Do(redis => redis.SortedSetDecrementAsync(key, ConvertJson(value), score));
        }

        #endregion
    }
}
