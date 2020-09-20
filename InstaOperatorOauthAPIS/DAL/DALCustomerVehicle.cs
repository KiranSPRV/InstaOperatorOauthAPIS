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
    public class DALCustomerVehicle
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public List<CustomerVehicle> GetAllVehicleRegistrationNumbers()
        {

            List<CustomerVehicle> lstRegistrationNumber = new List<CustomerVehicle>();
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleRegistrationNumber", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for(var i=0;i<resultdt.Rows.Count;i++)
                            {
                                CustomerVehicle objVehicle = new CustomerVehicle();
                                objVehicle.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                
                                objVehicle.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objVehicle.VehicleTypeID.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ?"": Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objVehicle.RegistrationNumber = resultdt.Rows[i]["RegistrationNumber"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                objVehicle.Model = resultdt.Rows[i]["Model"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Model"]);
                                objVehicle.Color = resultdt.Rows[i]["Color"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Color"]);
                                if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "2W".ToUpper())
                                {
                                    objVehicle.VehicleTypeID.VehicleImage = "bike_black.png";
                                }
                                else if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "4W".ToUpper())
                                {
                                    objVehicle.VehicleTypeID.VehicleImage = "car_black.png";
                                }
                                lstRegistrationNumber.Add(objVehicle);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetVehicleRegistrationNumber", "GetAllVehicleRegistrationNumbers");
                throw;

            }
            return lstRegistrationNumber;

        }

    }
}