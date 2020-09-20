using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALViolation
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public List<ViolationReason> GetViolationReasons(string StatusCode)
        {

            List<ViolationReason> lstViolationReason = new List<ViolationReason>();
            string resultMsg = string.Empty;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetViolationReasons", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@StatusCode", (StatusCode ==""|| StatusCode == string.Empty)?(object)DBNull.Value: StatusCode); 
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                ViolationReason objViolationReason = new ViolationReason();
                                objViolationReason.ViolationReasonID = resultdt.Rows[i]["ViolationReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ViolationReasonID"]);
                                objViolationReason.Reason = Convert.ToString(resultdt.Rows[i]["Reason"]);
                                lstViolationReason.Add(objViolationReason);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALViolation", "Proc: " + "OPAPP_PROC_GetViolationReasons", "GetViolationReasons");
            }
            return lstViolationReason;

        }
        public string SaveVehicleViolationAndClamp(ViolationAndClamp objViolationanClamp)
        {
            string resultmsg = string.Empty;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveVehicleViolationAndClamp", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objViolationanClamp.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objViolationanClamp.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objViolationanClamp.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@BayNumberID", objViolationanClamp.BayNumberID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objViolationanClamp.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objViolationanClamp.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@ViolationImage", objViolationanClamp.ViolationImage);
                        sqlcmd_obj.Parameters.AddWithValue("@ReasonID", objViolationanClamp.ReasonID);
                        sqlcmd_obj.Parameters.AddWithValue("@IsClamp",  objViolationanClamp.IsClamp);
                        sqlcmd_obj.Parameters.AddWithValue("@IsWarning", objViolationanClamp.IsWarning);
                        sqlcmd_obj.Parameters.AddWithValue("@ViolationStartTime", objViolationanClamp.ViolationTime);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleImageLottitude", objViolationanClamp.VehicleImageLottitude == null || objViolationanClamp.VehicleImageLottitude == 0 ? (Object)DBNull.Value : objViolationanClamp.VehicleImageLottitude);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleImageLongitude", objViolationanClamp.VehicleImageLongitude == null || objViolationanClamp.VehicleImageLongitude == 0 ? (Object)DBNull.Value : objViolationanClamp.VehicleImageLongitude);
                        sqlconn_obj.Open();
                        int resultid = sqlcmd_obj.ExecuteNonQuery();
                        if (resultid > 0)
                        {
                            resultmsg = "Success";
                        }
                        else
                        {
                            resultmsg = "Fail";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALViolation", "Proc: " + "OPAPP_PROC_SaveVehicleViolationAndClamp", "SaveVehicleViolationAndClamp");
            }
            return resultmsg;

        }
        public ViolationAndClamp GetVehicleViolationWarningCount(CustomerVehicle objInput)
        {
            ViolationAndClamp objViolationAndClamp = new ViolationAndClamp();
            string resultMsg = string.Empty;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleViolationWarning", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objInput.VehicleTypeID.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objInput.RegistrationNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            int warningCount = resultdt.Rows[0]["WarningCount"]==DBNull.Value?0:Convert.ToInt32(resultdt.Rows[0]["WarningCount"]);
                            if(warningCount>=3)
                            {
                                objViolationAndClamp.IsWarning = true;
                            }
                            else
                            {
                                objViolationAndClamp.IsWarning = false;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALViolation", "Proc: " + "OPAPP_PROC_GetVehicleViolationWarning", "GetVehicleViolationWarningCount");
            }
            return objViolationAndClamp;
        }
       

    }
}