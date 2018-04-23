using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class UserDto : EntityBaseFullDto
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNum { get; set; }
        public bool IsPhoneNumConfirmed { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int? CreatorUserId { get; set; }
        public UserDto CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public UserDto DeleterUser { get; set; }
        public bool IsLocked { get; set; } = false;
        public IList<UserRoleDto> UserRoles { get; set; }
    }
}
