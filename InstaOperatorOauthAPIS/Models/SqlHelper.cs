using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models
{
    public abstract class SqlHelper
    {
        public static string SQLDBCon { get; set; }

        public SqlHelper()
        {
            
        }
        public static string GetDBConnectionString()
        {
            try
            {
                SQLDBCon = ConfigurationManager.ConnectionStrings["InstaOperatorConnString"].ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            return SQLDBCon;
        }
    }
}