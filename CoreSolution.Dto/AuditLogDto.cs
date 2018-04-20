using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class AuditLogDto : EntityDto<int>
    {
        public int? UserId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string ClientIpAddress { get; set; }
        public string BrowserInfo { get; set; }
    }
}
