using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Dto.Base;

namespace CoreSolution.WebApi.Models.Role
{
    public class OutputRoleModel : EntityBaseFullDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
