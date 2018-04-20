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
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public bool IsLocked { get; set; }
        public IDictionary<int, string> Roles { get; set; }
    }
}
