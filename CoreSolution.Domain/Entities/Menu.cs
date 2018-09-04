using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class Menu : EntityBaseFull<int>
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string CustomData { get; set; }
        public string Icon { get; set; }
        public string ClassName { get; set; }
        public int OrderIn { get; set; }
        public int? CreatorUserId { get; set; }
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public virtual User DeleterUser { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
