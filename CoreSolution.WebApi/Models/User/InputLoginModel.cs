namespace CoreSolution.WebApi.Models.User
{
    /// <summary>
    /// 用户登录参数model
    /// </summary>
    public class InputLoginModel
    {
        /// <summary>
        /// 邮箱或手机
        /// </summary>
        public string UserNameOrEmailOrPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
