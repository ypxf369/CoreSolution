using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Dto.Base;

namespace CoreSolution.WebApi.Models.Menu
{
    public class OutputMenuModel : EntityBaseFullDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string CustomData { get; set; }
        public string Icon { get; set; }
        public string ClassName { get; set; }
        public int OrderIn { get; set; }
        public IList<OutputMenuModel> MenuItems { get; set; }
    }
}
