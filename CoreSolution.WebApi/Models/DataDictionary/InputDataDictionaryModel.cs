using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSolution.WebApi.Models.DataDictionary
{
    public class InputDataDictionaryModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public string Tips { get; set; }
        public int? CustomType { get; set; }
        public string CustomAttributes { get; set; }
        public int? ParentId { get; set; }

    }
}
