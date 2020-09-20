using InstaOperatorOauthAPIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ISTAOnlineWebAPI.DAL
{
    public class DALExceptionManagment : SqlHelper
    {
        
        public DALExceptionManagment()
        { }
        public string InsertException(string ApplicationType, string ExceptionMessage, string Module, string Procedure, string Method)
        {
            int strResult;
            string OperResult = string.Empty;
            try
            {
                if (ExceptionMessage != "")
                {
                    using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                    {
                        using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_InsertAPPExceptions", sqlconn_obj))
                        {
                            sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                            sqlcmd_obj.Parameters.AddWithValue("@ApplicationType", ApplicationType);
                            sqlcmd_obj.Parameters.AddWithValue("@ExceptionMessage ", ExceptionMessage);
                            sqlcmd_obj.Parameters.AddWithValue("@Model", Module);
                            sqlcmd_obj.Parameters.AddWithValue("@Procedure", Procedure);
                            sqlcmd_obj.Parameters.AddWithValue("@ApplicationMethod", Method);
                            sqlconn_obj.Open();
                            sqlcmd_obj.CommandTimeout = 0;
                            strResult = sqlcmd_obj.ExecuteNonQuery();
                            sqlconn_obj.Close();
                            if (strResult > 0)
                            {
                                OperResult = "Success";
                            }
                            else
                            {
                                OperResult = "Failed to load error";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return OperResult;

        }

    }
}