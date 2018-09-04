using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Dto.Base;

namespace CoreSolution.WebApi.Models.Permission
{
    public class OutputPermissionModel : EntityBaseFullDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
    }
}
