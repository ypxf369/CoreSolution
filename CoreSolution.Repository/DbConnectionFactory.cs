using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CoreSolution.Domain.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CoreSolution.Repository
{
    public class DbConnectionFactory
    {
        public static string GetSqlConnectionString()
        {
            var cfg = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "configuration.json", ReloadOnChange = true }).Build();
            string sqlConnStr = cfg.GetSection("DbConfig:ConnStr").Value;
            return sqlConnStr;
        }
        public static DbConnection CreateDbConnection(DatabaseType dbType)
        {
            DbConnection connection = null;
            string sqlConnStr = GetSqlConnectionString();
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(sqlConnStr);
                    break;
               // case DatabaseType.MySql:
                    //connection = new MySql.Data.MySqlClient.MySqlConnection(strConn); //安装MySql驱动
                    //break;
                //case DatabaseType.Oracle:
                    //connection = new System.Data.OracleClient.OracleConnection(strConn); //安装Oracle驱动
                    //break;
            }
            return connection;
        }
    }
}
