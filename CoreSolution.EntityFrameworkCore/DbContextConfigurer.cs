using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CoreSolution.EntityFrameworkCore
{
    public static class DbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder builder)
        {
            var cfg = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "configuration.json", ReloadOnChange = true }).Build();
            var connStr = cfg.GetSection("connStr");
            builder.UseSqlServer(connStr.Value);
            //builder.UseLazyLoadingProxies();//启用EFCore的延迟加载(建议不启用，显式Includ)
        }

        public static void Configure(DbContextOptionsBuilder builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
