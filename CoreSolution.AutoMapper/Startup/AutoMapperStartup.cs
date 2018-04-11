using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.AutoMapper.Configuration;

namespace CoreSolution.AutoMapper.Startup
{
    public class AutoMapperStartup
    {
        public static void Register()
        {
            AutoMapperConfiguration.Init();
        }
    }
}
