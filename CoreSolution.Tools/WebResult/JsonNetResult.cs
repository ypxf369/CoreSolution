using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreSolution.Tools.WebResult
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult() : base(null)
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//忽略循环引用，如果设置为Error，则遇到循环引用的时候报错（建议设置为Error，这样更规范）
                DateFormatString = "yyyy-MM-dd HH:mm:ss",//日期格式化，默认的格式也不好看
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()//json中属性开头字母小写的驼峰命名
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.Value == null)
            {
                return;
            }
            var scriptSerializer = JsonSerializer.Create(this.Settings);
            //TextWriter writer = new StreamWriter(response.Body);
            scriptSerializer.Serialize(new StreamWriter(response.Body), this.Value);
        }
    }
}
