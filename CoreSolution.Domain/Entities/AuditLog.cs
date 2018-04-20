using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class AuditLog : Entity<int>
    {
        public int? UserId { get; set; }
        /// <summary>
        /// (class/interface) name.
        /// </summary>
        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        /// <summary>
        /// Start time of the method execution.
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        public string ClientIpAddress { get; set; }

        public string BrowserInfo { get; set; }
    }
}
