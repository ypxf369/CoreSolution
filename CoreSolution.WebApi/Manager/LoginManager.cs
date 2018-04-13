using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Redis.Helper;

namespace CoreSolution.WebApi.Manager
{
    public class LoginManager
    {
        private static string TOKEN_PREFIX = "Api.User.Token.";
        private static string USERID_PREFIX = "Api.User.UserId.";
        private static string USERPERMISSIONS_PREFIX = "Api.User.Permissions.";

        public static async Task LoginAsync(string token, int userId)
        {
            await RedisHelper.StringSetAsync(TOKEN_PREFIX + token, userId, TimeSpan.FromHours(3));
            await RedisHelper.StringSetAsync(USERID_PREFIX + userId, token, TimeSpan.FromHours(3));//正向、反向关系都保存，这样保证一个token只能登陆一次
        }

        /// <summary>
        /// 缓存当前用户所具有的权限
        /// </summary>
        /// <param name="permissions">权限数组</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public static async Task SaveCurrentUserPermissionsAsync(string[] permissions, int userId)
        {
            await RedisHelper.StringSetAsync(USERPERMISSIONS_PREFIX + userId, permissions, TimeSpan.FromHours(3));
        }

        public static async Task<string[]> GetCurrentUserPermissionsAsync(int userId)
        {
            return await RedisHelper.StringGetAsync<string[]>(USERPERMISSIONS_PREFIX + userId);
        }

        public static async Task<int?> GetUserIdAsync(string token)
        {
            int? userId = await RedisHelper.StringGetAsync<int?>(TOKEN_PREFIX + token);
            if (userId == null)
            {
                return null;
            }
            string revertToken = await RedisHelper.StringGetAsync(USERID_PREFIX + userId);
            if (revertToken != token)//如果反向查的token不一样，说明这个token已经过期了
            {
                return null;
            }
            return userId;
        }

        /// <summary>
        /// 得到错误登录次数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static async Task<int> GetErrorLoginTimesAsync(string redisKey, string email)
        {
            string key = redisKey + email;
            int? count = await RedisHelper.StringGetAsync<int?>(key);
            if (count == null)
            {
                count = 0;
            }
            return (int)count;
        }

        /// <summary>
        /// 重置登录错误次数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="email"></param>
        public static void ResetErrorLogin(string redisKey, string email)
        {
            string key = redisKey + email;
            RedisHelper.KeyRemove(key);
        }

        /// <summary>
        /// 递增登录错误次数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="email"></param>
        public static async Task IncreaseErrorLoginAsync(string redisKey, string email)
        {
            string key = redisKey + email;
            int? count = await RedisHelper.StringGetAsync<int?>(key) ?? 0;
            count++;
            await RedisHelper.StringSetAsync(key, count, TimeSpan.FromMinutes(15));//超时时间15分钟
        }
    }
}
