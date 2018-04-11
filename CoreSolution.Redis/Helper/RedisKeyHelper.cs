using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CoreSolution.Redis.Helper
{
    public partial class RedisHelper
    {
        #region 同步方法
        public static bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return Do(db => db.KeyExpire(key, expiry));
        }

        public static bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            var bResult = Do(db => db.StringSet(key, value, expiry));
            return bResult;
        }

        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues = keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray()));
        }

        public static bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        public static string StringGet(string key)
        {
            return Do(db => db.StringGet(key));
        }

        public static RedisValue[] StringGet(List<string> listKey)
        {
            return Do(db => db.StringGet(ConvertRedisKeys(listKey)));
        }

        public static T StringGet<T>(string key)
        {
            return Do(db => ConvertObj<T>(db.StringGet(key)));
        }

        public static double StringIncrement(string key, double val = 1)
        {
            return Do(db => db.StringIncrement(key, val));
        }

        public static double StringDecrement(string key, double val = 1)
        {
            return Do(db => db.StringDecrement(key, val));
        }

        public static bool KeyRemove(string key)
        {
            return Do(db => db.KeyDelete(key));
        }

        public static long KeyDelete(List<string> keys)
        {
            return Do(db => db.KeyDelete(ConvertRedisKeys(keys)));
        }

        public static bool KeyRename(string oldKey, string newKey)
        {
            return Do(db => db.KeyRename(oldKey, newKey));
        }

        public static long StringAppend(string key, string value)
        {
            return Do(db => db.StringAppend(key, value));
        }

        public static bool KeyExists(string key)
        {
            return Do(db => db.KeyExists(key));
        }

        #endregion

        #region 异步方法
        public static async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return await Do(db => db.KeyExpireAsync(key, expiry));
        }
        public static async Task<bool> KeyExpireAsync(string key, DateTime? expiry = default(DateTime?))
        {
            return await Do(db => db.KeyExpireAsync(key,expiry));
        }

        public static async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return await Do(db => db.StringSetAsync(key, value, expiry));
        }

        public static async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues = keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray()));
        }

        public static async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = ConvertJson(obj);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }

        public static async Task<string> StringGetAsync(string key)
        {
            return await Do(db => db.StringGetAsync(key));
        }

        public static async Task<RedisValue[]> StringGetAsync(List<string> listKey)
        {
            return await Do(db => db.StringGetAsync(ConvertRedisKeys(listKey)));
        }

        public static async Task<T> StringGetAsync<T>(string key)
        {
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);
        }

        public static async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        public static async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        public static async Task<long> StringAppendAsync(string key, string value)
        {
            return await Do(db => db.StringAppendAsync(key, value));
        }

        #endregion
    }
}
