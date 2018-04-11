using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreSolution.Domain.Enum
{
    public enum LoginResults
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        [Description("登录成功")]
        Success = 0,
        /// <summary>
        /// 登录名不存在
        /// </summary>
        [Description("登录名不存在")]
        NotExist = -1,
        /// <summary>
        /// 登录名或是密码错误
        /// </summary>
        [Description("登录名或是密码错误")]
        PassWordError = -2,
        /// <summary>
        /// 验证码错误
        /// </summary>
        [Description("验证码输入不正确")]
        CodeError = -3,
        /// <summary>
        /// 邮箱未激活
        /// </summary>
        [Description("您的邮箱还没有激活")]
        NoActived = -4,
        /// <summary>
        /// 登录参数录入不完整
        /// </summary>
        [Description("登录参数输入不完整")]
        Incomplete = -5,
        /// <summary>
        /// 如果是用手机号登陆的，必须检查手机号是否已经认证
        /// </summary>
        [Description("您的手机号还没有认证")]
        NoPhoneActived = -6,
        /// <summary>
        /// 如果是用手机号登陆的，必须检查手机号是否已经认证
        /// </summary>
        [Description("您的账户已经被锁定，请15分钟后重试，如有问题，请联系客服")]
        IsLocked = -7,

        [Description("用户注册失败")]
        RegError = -8
    }
}
