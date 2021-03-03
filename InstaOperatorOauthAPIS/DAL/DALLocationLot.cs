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
    public class DALLocationLot
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public LocationParkingLot GetLocationLotVehicleAvailabilityDetails(int LocationID, int LocationParkingLotID)
        {

            LocationParkingLot resultLocationParkingLot = new LocationParkingLot();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetLocationLotVehcileAvailabilityDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", LocationID == 0 ? (object)DBNull.Value : LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", LocationParkingLotID == 0 ? (object)DBNull.Value : LocationParkingLotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            string[] LotVehicleAvailabilityName = new string[resultdt.Rows.Count];
                            resultLocationParkingLot.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            resultLocationParkingLot.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            resultLocationParkingLot.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            for (var item = 0; item < resultdt.Rows.Count; item++)
                            {
                                LotVehicleAvailabilityName[item] = Convert.ToString(resultdt.Rows[item]["VEHICLETYPECODE"]);
                            }
                            resultLocationParkingLot.LotVehicleAvailabilityName = LotVehicleAvailabilityName;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALLocationLot", "Proc: " + "OPAPP_PROC_GetLocationLotVehcileAvailabilityDetails", "GetLocationLotVehicleAvailabilityDetails");
                throw;

            }
            return resultLocationParkingLot;

        }
    }
}