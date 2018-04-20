using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Dto.Base;

namespace CoreSolution.WebApi.Models.User
{
    public class OutputUserModel : EntityBaseFullDto
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
    }
}
