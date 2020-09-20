using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.Pass;
using InstaOperatorOauthAPIS.VMModels;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALReNewPass
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public List<CustomerVehicle> GetAllPassVehicles()
        {
            List<CustomerVehicle> lstCustomerVehicle = null;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllPassVehicles", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            lstCustomerVehicle = new List<CustomerVehicle>();
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                CustomerVehicle objCustomerVehicle = new CustomerVehicle();
                                objCustomerVehicle.CustomerVehicleID= resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                objCustomerVehicle.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                objCustomerVehicle.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objCustomerVehicle.VehicleTypeID.VehicleTypeName = resultdt.Rows[i]["VehicleTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                objCustomerVehicle.VehicleTypeID.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                //objCustomerVehicle.CustomerID.CustomerID = resultdt.Rows[i]["CUSTOMERID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CUSTOMERID"]);
                                objCustomerVehicle.RegistrationNumber = resultdt.Rows[i]["RegistrationNumber"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                lstCustomerVehicle.Add(objCustomerVehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALReNewPass", "Proc: " + "OPAPP_PROC_GetAllPassVehicles", "GetPassPriceDetails");
            }
            return lstCustomerVehicle;
        }
    }
}