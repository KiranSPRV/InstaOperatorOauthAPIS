using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.Reports;
using InstaOperatorOauthAPIS.VMModels;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALCustomerVehicleParkingLot
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public VMLocationLotParkedVehicles GetParkedVehicles(ParkedVehiclesFilter obj)
        {
            VMLocationLotParkedVehicles objVMLocationLotParkedVehicles = new VMLocationLotParkedVehicles();
            string StatusID = string.Empty;
            string ApplicationTypeID = string.Empty;

            if (obj.ApplicationTypeCode != null && obj.ApplicationTypeCode.Count > 0)
            {
                ApplicationTypeID = "(";
                for (var a = 0; a < obj.ApplicationTypeCode.Count; a++)
                {
                    if (a == 0)
                    {
                        ApplicationTypeID = ApplicationTypeID + obj.ApplicationTypeCode[a].ApplicationTypeID;
                    }
                    else
                    {
                        ApplicationTypeID = ApplicationTypeID + "," + obj.ApplicationTypeCode[a].ApplicationTypeID;
                    }

                }
                ApplicationTypeID = ApplicationTypeID + ")";
            }
            if (obj.StatusCode != null && obj.StatusCode.Count > 0)
            {
                StatusID = "(";
                for (var s = 0; s < obj.StatusCode.Count; s++)
                {
                    if (s == 0)
                    {
                        StatusID = StatusID + obj.StatusCode[s].StatusID;
                    }
                    else
                    {
                        StatusID = StatusID + "," + obj.StatusCode[s].StatusID;
                    }

                }
                StatusID = StatusID + ")";



            }
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Get_All_Parked_Vehicles", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (obj.VehicleTypeCode == null || obj.VehicleTypeCode == "") ? (object)DBNull.Value : obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", (obj.LocationID == null || obj.LocationID == 0) ? (object)DBNull.Value : obj.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (obj.LocationParkingLotID == null || obj.LocationParkingLotID == 0) ? (object)DBNull.Value : obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StatusID", (StatusID == "" || StatusID == string.Empty) ? (object)DBNull.Value : StatusID);
                        sqlcmd_obj.Parameters.AddWithValue("@ApplicationTypeID", (ApplicationTypeID == "" || ApplicationTypeID == string.Empty) ? (object)DBNull.Value : ApplicationTypeID);
                        sqlcmd_obj.Parameters.AddWithValue("@IsClamped", (obj.IsClamped == null || obj.IsClamped == false ? (object)DBNull.Value : Convert.ToBoolean(obj.IsClamped)));

                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataSet resultds = new DataSet();
                        sqldap.Fill(resultds);
                        DataTable dtParkedVehicles = new DataTable();
                        DataTable dtTotalVehicles = new DataTable();
                        DataTable dtTotalVehiclesCheckOut = new DataTable();
                        if (resultds.Tables.Count > 0)
                        {
                            dtParkedVehicles = resultds.Tables[0];
                            dtTotalVehicles = resultds.Tables[1];
                            dtTotalVehiclesCheckOut = resultds.Tables[2];
                            if (dtParkedVehicles.Rows.Count > 0)
                            {
                                List<LocationLotParkedVehicles> lstCustomerParkingSlot = new List<LocationLotParkedVehicles>();
                                for (var i = 0; i < dtParkedVehicles.Rows.Count; i++)
                                {
                                    bool isClamp = false;
                                    LocationLotParkedVehicles objCustomerParkingSlot = new LocationLotParkedVehicles();
                                    objCustomerParkingSlot.CustomerParkingSlotID = Convert.ToInt32(dtParkedVehicles.Rows[i]["CustomerParkingSlotID"]);
                                    objCustomerParkingSlot.RegistrationNumber = Convert.ToString(dtParkedVehicles.Rows[i]["RegistrationNumber"]);
                                    objCustomerParkingSlot.ParkingBayName = Convert.ToString(dtParkedVehicles.Rows[i]["ParkingBayName"]);
                                    objCustomerParkingSlot.ParkingBayRange = Convert.ToString(dtParkedVehicles.Rows[i]["ParkingBayRange"]);
                                    objCustomerParkingSlot.StatusID = dtParkedVehicles.Rows[i]["StatusID"] == DBNull.Value ? 0 : Convert.ToInt32(dtParkedVehicles.Rows[i]["StatusID"]);
                                    objCustomerParkingSlot.StatusCode = Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]);
                                    objCustomerParkingSlot.StatusName = Convert.ToString(dtParkedVehicles.Rows[i]["StatusName"]);
                                    objCustomerParkingSlot.ApplicationTypeCode = Convert.ToString(dtParkedVehicles.Rows[i]["ApplicationTypeCode"]);
                                    isClamp = dtParkedVehicles.Rows[i]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(dtParkedVehicles.Rows[i]["IsClamp"]);
                                    objCustomerParkingSlot.IsClamp = isClamp;
                                    if (Convert.ToString(dtParkedVehicles.Rows[i]["VehicleTypeCode"]) == "2W")
                                    {
                                        if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "C")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "bike_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "V")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "bike_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                            }
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "O")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "bike_orange.png";
                                            objCustomerParkingSlot.BayNumberColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleStatusColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleClampImage = "clock_orange.png";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "CHKIN")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "bike_black.png";
                                            objCustomerParkingSlot.BayNumberColor = "#444444";
                                            objCustomerParkingSlot.VehicleStatusColor = "#3293fa";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "G")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "bike_green.png";
                                            objCustomerParkingSlot.BayNumberColor = "#008000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#008000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "G";
                                        }
                                    }
                                    if (Convert.ToString(dtParkedVehicles.Rows[i]["VehicleTypeCode"]) == "3W")
                                    {
                                        if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "C")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "ThreeW_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "V")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "ThreeW_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                            }
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "O")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "ThreeW_orange.png";
                                            objCustomerParkingSlot.BayNumberColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleStatusColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleClampImage = "clock_orange.png";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "CHKIN")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "ThreeW_black.png";
                                            objCustomerParkingSlot.BayNumberColor = "#444444";
                                            objCustomerParkingSlot.VehicleStatusColor = "#3293fa";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "G")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "ThreeW_green.png";
                                            objCustomerParkingSlot.BayNumberColor = "#008000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#008000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "G";
                                        }
                                    }
                                    if (Convert.ToString(dtParkedVehicles.Rows[i]["VehicleTypeCode"]) == "4W")
                                    {
                                        if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "C")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "car_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                        }
                                        if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "V")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "car_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                            }
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "O")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "car_orange.png";
                                            objCustomerParkingSlot.BayNumberColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleStatusColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleClampImage = "clock_orange.png";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }


                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "CHKIN")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "car_black.png";
                                            objCustomerParkingSlot.BayNumberColor = "#444444";
                                            objCustomerParkingSlot.VehicleStatusColor = "#3293fa";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "G")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "car_green.png";
                                            objCustomerParkingSlot.BayNumberColor = "#008000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#008000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "G";

                                        }
                                    }
                                    if (Convert.ToString(dtParkedVehicles.Rows[i]["VehicleTypeCode"]) == "HW")
                                    {
                                        if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "C")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "hv_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "V")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "hv_red.png";
                                            objCustomerParkingSlot.BayNumberColor = "#FF0000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#FF0000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "V";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp_small.png";
                                            }
                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "O")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "hv_orange.png";
                                            objCustomerParkingSlot.BayNumberColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleStatusColor = "#F39C12";
                                            objCustomerParkingSlot.VehicleClampImage = "clock_orange.png";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "CHKIN")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "hv_black.png";
                                            objCustomerParkingSlot.BayNumberColor = "#444444";
                                            objCustomerParkingSlot.VehicleStatusColor = "#3293fa";
                                            if (isClamp)
                                            {
                                                objCustomerParkingSlot.VehicleClampImage = "clamp.png";
                                            }

                                        }
                                        else if (Convert.ToString(dtParkedVehicles.Rows[i]["StatusCode"]) == "G")
                                        {
                                            objCustomerParkingSlot.VehicleImage = "hv_green.png";
                                            objCustomerParkingSlot.BayNumberColor = "#008000";
                                            objCustomerParkingSlot.VehicleStatusColor = "#008000";
                                            objCustomerParkingSlot.ApplicationTypeCode = "G";
                                        }
                                    }
                                    lstCustomerParkingSlot.Add(objCustomerParkingSlot);
                                }
                                objVMLocationLotParkedVehicles.CustomerParkingSlotID = lstCustomerParkingSlot;
                            }
                            if (dtTotalVehicles.Rows.Count > 0)
                            {
                                for (var j = 0; j < dtTotalVehicles.Rows.Count; j++)
                                {
                                    if (Convert.ToString(dtTotalVehicles.Rows[j]["VehicleTypeCode"]) == "2W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalTwoWheeler = dtTotalVehicles.Rows[j]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehicles.Rows[j]["Total"]);
                                    }
                                    if (Convert.ToString(dtTotalVehicles.Rows[j]["VehicleTypeCode"]) == "3W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalThreeWheeler = dtTotalVehicles.Rows[j]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehicles.Rows[j]["Total"]);
                                    }
                                    if (Convert.ToString(dtTotalVehicles.Rows[j]["VehicleTypeCode"]) == "4W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalFourWheeler = dtTotalVehicles.Rows[j]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehicles.Rows[j]["Total"]);
                                    }
                                    if (Convert.ToString(dtTotalVehicles.Rows[j]["VehicleTypeCode"]) == "HW")
                                    {
                                        objVMLocationLotParkedVehicles.TotalHVWheeler = dtTotalVehicles.Rows[j]["Total"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehicles.Rows[j]["Total"]);
                                    }
                                }
                            }
                            if (dtTotalVehiclesCheckOut.Rows.Count > 0)
                            {
                                for (var j = 0; j < dtTotalVehiclesCheckOut.Rows.Count; j++)
                                {
                                    if (Convert.ToString(dtTotalVehiclesCheckOut.Rows[j]["VehicleTypeCode"]) == "2W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalOutTwoWheeler = dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"]);
                                    }
                                    if (Convert.ToString(dtTotalVehicles.Rows[j]["VehicleTypeCode"]) == "3W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalOutThreeWheeler = dtTotalVehicles.Rows[j]["TotalOUT"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehicles.Rows[j]["TotalOUT"]);
                                    }
                                    if (Convert.ToString(dtTotalVehiclesCheckOut.Rows[j]["VehicleTypeCode"]) == "4W")
                                    {
                                        objVMLocationLotParkedVehicles.TotalOutFourWheeler = dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"]);
                                    }
                                    if (Convert.ToString(dtTotalVehiclesCheckOut.Rows[j]["VehicleTypeCode"]) == "HW")
                                    {
                                        objVMLocationLotParkedVehicles.TotalOutHVWheeler = dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"] == DBNull.Value ? 0 : Convert.ToInt32(dtTotalVehiclesCheckOut.Rows[j]["TotalOUT"]);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_Get_All_Parked_Vehicles", "GetParkedVehicles");
            }
            return objVMLocationLotParkedVehicles;
        }
        public CustomerParkingSlot GetParkedVehicleDetails(int CustomerParkingSlotID)
        {
            CustomerParkingSlot objCustomerParkingSlot = null;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Get_ParkedVehicleDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotID", CustomerParkingSlotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objCustomerParkingSlot = new CustomerParkingSlot();
                            objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[0]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerParkingSlotID"]);
                            objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomerParkingSlot.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objCustomerParkingSlot.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[0]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ParkingBayID"]);
                            objCustomerParkingSlot.ParkingBayID.ParkingBayID = resultdt.Rows[0]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ParkingBayID"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[0]["ParkingBayName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[0]["ParkingBayRange"]);
                            objCustomerParkingSlot.ExpectedStartTime = resultdt.Rows[0]["ExpectedStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ExpectedStartTime"]);
                            objCustomerParkingSlot.ExpectedEndTime = resultdt.Rows[0]["ExpectedEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ExpectedEndTime"]);
                            objCustomerParkingSlot.ActualStartTime = resultdt.Rows[0]["ActualStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ActualStartTime"]);
                            objCustomerParkingSlot.ActualEndTime = resultdt.Rows[0]["ActualEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ActualEndTime"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                           
                            
                            objCustomerParkingSlot.Duration = Convert.ToString(resultdt.Rows[0]["Duration"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeID = resultdt.Rows[0]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);
                            objCustomerParkingSlot.CreatedBy = resultdt.Rows[0]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["UserID"]);
                            objCustomerParkingSlot.CreatedByName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                            objCustomerParkingSlot.CreatedOn = Convert.ToDateTime(resultdt.Rows[0]["CreatedOn"]);
                            objCustomerParkingSlot.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);
                            objCustomerParkingSlot.IsClamp = resultdt.Rows[0]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IsClamp"]);
                            objCustomerParkingSlot.IsWarning = resultdt.Rows[0]["IsWarning"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IsWarning"]);
                            objCustomerParkingSlot.Amount = resultdt.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                            objCustomerParkingSlot.PaidAmount = resultdt.Rows[0]["PaidAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["PaidAmount"]);
                            objCustomerParkingSlot.ClampFees = resultdt.Rows[0]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ClampFee"]);
                            objCustomerParkingSlot.ExtendAmount = resultdt.Rows[0]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ExtendAmount"]);
                            objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeID = resultdt.Rows[0]["ApplicationTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ApplicationTypeID"]);
                            objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = Convert.ToString(resultdt.Rows[0]["ApplicationTypeCode"]);
                            objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeName = Convert.ToString(resultdt.Rows[0]["ApplicationTypeName"]);
                            objCustomerParkingSlot.ViolationReasonID.ViolationReasonID = resultdt.Rows[0]["ViolationReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ViolationReasonID"]);
                            objCustomerParkingSlot.ViolationReasonID.Reason = Convert.ToString(resultdt.Rows[0]["Reason"]);
                            objCustomerParkingSlot.VehicleParkingImage = resultdt.Rows[0]["VehicleParkingImage"] == DBNull.Value ? null : (byte[])resultdt.Rows[0]["VehicleParkingImage"];
                            objCustomerParkingSlot.GovernmentVehicleImage = resultdt.Rows[0]["GovernmentVehicleImage"] == DBNull.Value ? null : (byte[])resultdt.Rows[0]["GovernmentVehicleImage"];
                            objCustomerParkingSlot.StatusID.StatusID = resultdt.Rows[0]["StatusID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["StatusID"]);
                            objCustomerParkingSlot.StatusID.StatusName = Convert.ToString(resultdt.Rows[0]["StatusName"]);
                            objCustomerParkingSlot.StatusID.StatusCode = Convert.ToString(resultdt.Rows[0]["StatusCode"]);
                            objCustomerParkingSlot.ViolationWarningCount = resultdt.Rows[0]["WarningCount"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["WarningCount"]);
                            objCustomerParkingSlot.VehicleImageLottitude = resultdt.Rows[0]["VehicleImageLottitude"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["VehicleImageLottitude"]);
                            objCustomerParkingSlot.VehicleImageLongitude = resultdt.Rows[0]["VehicleImageLongitude"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["VehicleImageLongitude"]);
                            objCustomerParkingSlot.LocationParkingLotID.LotCloseTime = Convert.ToString(resultdt.Rows[0]["LotCloseTime"]);
                            objCustomerParkingSlot.LocationParkingLotID.LotOpenTime = Convert.ToString(resultdt.Rows[0]["LotOpenTime"]);
                            objCustomerParkingSlot.SuperVisorID.UserCode = resultdt.Rows[0]["SUPERVISORCODE"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["SUPERVISORCODE"]);
                            objCustomerParkingSlot.SuperVisorID.PhoneNumber = resultdt.Rows[0]["SUPERVISORPHONENUMBER"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["SUPERVISORPHONENUMBER"]);
                            objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            
                           
                            string vehicleTypeCode= Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            if (vehicleTypeCode.ToUpper() == "2W")
                            {

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "BIKE";
                                objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "bike_black.png";

                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleTypeDisplayName = "BIKE";
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "bike_black.png";
                            }
                            else if (vehicleTypeCode.ToUpper() == "3W")
                            {

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "THREE WHEELER";
                                objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "ThreeW_black.png";

                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleTypeDisplayName = "THREE WHEELER";
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "ThreeW_black.png";

                            }
                            else if (vehicleTypeCode.ToUpper() == "4W")
                            {

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "CAR";
                                objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "car_black.png";

                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleTypeDisplayName = "CAR";
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "car_black.png";

                            }
                            else if (vehicleTypeCode.ToUpper() == "HW")
                            {

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "Heavy Vehicle";
                                objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "hv_black.png";

                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleTypeDisplayName = "HEAVY VEHICLE";
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "hv_black.png";

                            }
                            objCustomerParkingSlot.GSTNumber = "36AACFZ1015E1ZL";

                            DALCustomerVehicle dal_CustomerVehicle = new DALCustomerVehicle();
                            objCustomerParkingSlot.DueAmount = dal_CustomerVehicle.GetVehicleDueAmount(Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]), Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]));


                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetParkedVehicleDetails", "GetParkedVehicleDetails");


            }
            return objCustomerParkingSlot;

        }
        public ViolationVehicleFees GetViolationVehicleCharges(ViolationVehicleFees objInput)
        {
            ViolationVehicleFees objViolationVehicleFees = null;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleViolationFee", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        //  sqlcmd_obj.Parameters.AddWithValue("@ParkingStartTime", objInput.ParkingStartTime.ToString("MM/dd/yyyy hh:mm tt"));
                        // sqlcmd_obj.Parameters.AddWithValue("@ParkingEndTime", objInput.ParkingEndTime.ToString("MM/dd/yyyy hh:mm tt"));


                        int hoursDuration = (Convert.ToInt32((objInput.ParkingEndTime - objInput.ParkingStartTime).TotalMinutes)) / 60;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotId", objInput.CustomerParkingSlotId);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingStartTime", objInput.ParkingStartTime);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingEndTime", objInput.ParkingEndTime);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objInput.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeID", objInput.VehicleTypeID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objInput.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@HoursDuration", hoursDuration);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objViolationVehicleFees = new ViolationVehicleFees();
                            objViolationVehicleFees.ParkingFee = resultdt.Rows[0]["ParkingFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ParkingFee"]);
                            objViolationVehicleFees.ClampFee = resultdt.Rows[0]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ClampFee"]);
                            objViolationVehicleFees.TotalFee = resultdt.Rows[0]["TotalFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["TotalFee"]);
                            objViolationVehicleFees.TotalHours = resultdt.Rows[0]["TotalHours"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["TotalHours"]);

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleViolationFee", "GetViolationVehicleCharges");
            }
            return objViolationVehicleFees;

        }
        public List<ApplicationType> GetAllApplicationTypes()
        {
            List<ApplicationType> lstApplicationType = new List<ApplicationType>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllApplicationTypes", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                ApplicationType objapplicationType = new ApplicationType();
                                objapplicationType.ApplicationTypeID = resultdt.Rows[i]["ApplicationTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ApplicationTypeID"]);
                                objapplicationType.ApplicationTypeName = resultdt.Rows[i]["ApplicationTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["ApplicationTypeName"]);
                                objapplicationType.ApplicationTypeDesc = resultdt.Rows[i]["ApplicationTypeDesc"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["ApplicationTypeDesc"]);
                                objapplicationType.ApplicationTypeCode = resultdt.Rows[i]["ApplicationTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["ApplicationTypeCode"]);
                                lstApplicationType.Add(objapplicationType);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetAllApplicationTypes", "GetAllApplicationTypes");
            }
            return lstApplicationType;
        }
        public List<Status> GetAllStatus()
        {
            List<Status> lstStatus = new List<Status>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllStatus", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {

                                Status objStatus = new Status();
                                objStatus.StatusID = resultdt.Rows[i]["StatusID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["StatusID"]);
                                objStatus.StatusName = resultdt.Rows[i]["StatusName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["StatusName"]).ToUpper();
                                objStatus.StatusCode = resultdt.Rows[i]["StatusCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["StatusCode"]);
                                objStatus.StatusDesc = resultdt.Rows[i]["StatusDesc"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["StatusDesc"]);
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "V")
                                {
                                    objStatus.ShowStatusImage = false;
                                    objStatus.ShowStatus = true;
                                }
                                else
                                {
                                    objStatus.ShowStatusImage = true;
                                    objStatus.ShowStatus = false;
                                    if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "C")
                                    {
                                        objStatus.StatusImage = "clamp_small.png";
                                    }
                                    if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "O")
                                    {
                                        objStatus.StatusImage = "clock_orange.png";
                                    }
                                }
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) != "CHKIN" && Convert.ToString(resultdt.Rows[i]["StatusCode"]) != "CHKOut" && Convert.ToString(resultdt.Rows[i]["StatusCode"]) != "FOC")
                                {
                                    lstStatus.Add(objStatus);
                                }

                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetAllStatus", "GetAllStatus");
            }
            return lstStatus;
        }
        public List<CustomerParkingSlot> GetVehicleParkingHistory(CustomerVehicle objCustomerVehicle)
        {
            List<CustomerParkingSlot> lstVehicleParkingDetails = new List<CustomerParkingSlot>();
            DataTable resultdt = new DataTable();
            int VehicleWarningCount = 0;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleParkingHistory", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", objCustomerVehicle.CustomerVehicleID);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objCustomerVehicle.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeID", objCustomerVehicle.VehicleTypeID.VehicleTypeID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                decimal totalParkingAmount = 0;
                                CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
                                objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[i]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerParkingSlotID"]);
                                objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[i]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerID"]);
                                objCustomerParkingSlot.UserCode = Convert.ToString(resultdt.Rows[i]["UserCode"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[i]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[i]["LocationParkingLotName"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[i]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ParkingBayID"]);
                                objCustomerParkingSlot.ParkingBayID.ParkingBayID = resultdt.Rows[i]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ParkingBayID"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[i]["ParkingBayName"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[i]["ParkingBayRange"]);
                                objCustomerParkingSlot.ExpectedStartTime = resultdt.Rows[i]["ExpectedStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ExpectedStartTime"]);
                                objCustomerParkingSlot.ExpectedEndTime = resultdt.Rows[i]["ExpectedEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ExpectedEndTime"]);
                                objCustomerParkingSlot.ActualStartTime = resultdt.Rows[i]["ActualStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ActualStartTime"]);
                                objCustomerParkingSlot.ActualEndTime = resultdt.Rows[i]["ActualEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ActualEndTime"]);
                                
                                objCustomerParkingSlot.Duration = Convert.ToString(resultdt.Rows[i]["Duration"]);
                                objCustomerParkingSlot.PaymentTypeID.PaymentTypeID = resultdt.Rows[i]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["PaymentTypeID"]);
                                objCustomerParkingSlot.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[i]["PaymentTypeName"]);
                                objCustomerParkingSlot.CreatedBy = resultdt.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserID"]);
                                objCustomerParkingSlot.CreatedByName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objCustomerParkingSlot.SuperVisorID.PhoneNumber = Convert.ToString(resultdt.Rows[i]["SUPERVISORPHONENUMBER"]);
                                objCustomerParkingSlot.PaidAmount = resultdt.Rows[i]["PaidAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["PaidAmount"]);
                                objCustomerParkingSlot.DueAmount = resultdt.Rows[i]["DueAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["DueAmount"]);
                                objCustomerParkingSlot.IsWarning = resultdt.Rows[i]["IsWarning"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsWarning"]);
                                objCustomerParkingSlot.IsClamp = resultdt.Rows[i]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsClamp"]);
                                objCustomerParkingSlot.ClampFees = resultdt.Rows[i]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ClampFee"]);
                                objCustomerParkingSlot.ViolationFees = resultdt.Rows[i]["ViolationFees"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ViolationFees"]);

                                //Total Parking Amount
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) != "FOC")
                                {
                                    totalParkingAmount = ((resultdt.Rows[i]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["Amount"])) + (resultdt.Rows[i]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ExtendAmount"])));
                                }
                                objCustomerParkingSlot.Amount = totalParkingAmount;
                                objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeID = resultdt.Rows[i]["ApplicationTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ApplicationTypeID"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = Convert.ToString(resultdt.Rows[i]["ApplicationTypeCode"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeName = Convert.ToString(resultdt.Rows[i]["ApplicationTypeName"]);
                                objCustomerParkingSlot.ViolationReasonID.ViolationReasonID = resultdt.Rows[i]["ViolationReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ViolationReasonID"]);
                                objCustomerParkingSlot.ViolationReasonID.Reason = Convert.ToString(resultdt.Rows[i]["Reason"]);
                                //FOC Reason
                                objCustomerParkingSlot.FOCReasonID.ViolationReasonID = resultdt.Rows[i]["FOCReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["FOCReasonID"]);
                                objCustomerParkingSlot.FOCReasonID.Reason = Convert.ToString(resultdt.Rows[i]["FOCReason"]);
                                //FOC Reason END
                                objCustomerParkingSlot.VehicleParkingImage = resultdt.Rows[i]["VehicleParkingImage"] == DBNull.Value ? null : (byte[])resultdt.Rows[i]["VehicleParkingImage"];
                                objCustomerParkingSlot.StatusID.StatusCode = Convert.ToString(resultdt.Rows[i]["StatusCode"]);
                                objCustomerParkingSlot.StatusID.StatusName = Convert.ToString(resultdt.Rows[i]["StatusName"]);
                                objCustomerParkingSlot.StatusID.StatusColor = "#010101";  // App Style
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeColor = "#3293fa";
                                bool IsViolationRec = resultdt.Rows[i]["IsViolation"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsViolation"]);
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "C" || Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "V" || IsViolationRec)
                                {
                                    objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = "V";
                                    objCustomerParkingSlot.StatusID.StatusColor = "#ff0000";  // App Style
                                    objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeColor = "#ff0000";
                                }
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "FOC")
                                {
                                    if (IsViolationRec)
                                    {
                                        objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = "V";
                                    }
                                    objCustomerParkingSlot.StatusID.StatusColor = "#ff0000";  // App Style
                                }
                                if (((resultdt.Rows[i]["IsWarning"] == DBNull.Value) ? false : Convert.ToBoolean(resultdt.Rows[i]["IsWarning"])))
                                {
                                    VehicleWarningCount = VehicleWarningCount + 1;
                                    objCustomerParkingSlot.ViolationWarningCount = VehicleWarningCount;// resultdt.Rows[i]["ViolationWarningCount"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ViolationWarningCount"]);
                                }
                                else
                                {
                                    objCustomerParkingSlot.ViolationWarningCount = VehicleWarningCount;
                                }

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                objCustomerParkingSlot.GSTNumber = "36AACFZ1015E1ZL";
                                string vehicleTypeCode = Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                if (vehicleTypeCode.ToUpper() == "2W")
                                {

                                    objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "BIKE";
                                    objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                    objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "bike_black.png";
                                    objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "bike_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "3W")
                                {

                                    objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "Three Wheeler";
                                    objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                    objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "ThreeW_black.png";
                                    objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "ThreeW_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "4W")
                                {

                                    objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "CAR";
                                    objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                    objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "car_black.png";
                                    objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "car_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "HW")
                                {

                                    objCustomerParkingSlot.VehicleTypeID.VehicleTypeDisplayName = "Heavy Vehicle";
                                    objCustomerParkingSlot.VehicleTypeID.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                    objCustomerParkingSlot.VehicleTypeID.VehicleIcon = "hv_black.png";
                                    objCustomerParkingSlot.CustomerVehicleID.VehicleTypeID.VehicleIcon = "hv_black.png";
                                }
                                
                                
                                lstVehicleParkingDetails.Add(objCustomerParkingSlot);



                            }
                            var resultparking = lstVehicleParkingDetails.OrderByDescending(d => d.CustomerParkingSlotID).ToList();
                            lstVehicleParkingDetails = resultparking;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetVehicleParkingHistory");


            }

            return lstVehicleParkingDetails;

        }
        public List<VehicleType> GetAllVehicleTypes()
        {
            List<VehicleType> lstVehicleType = new List<VehicleType>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllVehicleTypes", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                               
                                VehicleType objVehicleType = new VehicleType();
                                string vehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objVehicleType.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objVehicleType.VehicleTypeName = resultdt.Rows[i]["VehicleTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                objVehicleType.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objVehicleType.VehicleImage = resultdt.Rows[i]["VehicleImage"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleImage"]);
                                
                                if(vehicleTypeCode.ToUpper()=="2W")
                                {
                                    objVehicleType.VehicleInActiveImage = "Twowheeler_circle.png";
                                    objVehicleType.VehicleActiveImage = "Twowheeler_circle_ticked.png";
                                    objVehicleType.VehicleIcon = "bike_black.png";
                                    objVehicleType.VehicleTypeDisplayName =  "BIKE";
                                    objVehicleType.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                }
                                else if (vehicleTypeCode.ToUpper() == "3W")
                                {
                                    objVehicleType.VehicleInActiveImage = "ThreeW.png";
                                    objVehicleType.VehicleActiveImage = "ThreeW_active.png";
                                    objVehicleType.VehicleIcon = "ThreeW_black.png";
                                    objVehicleType.VehicleTypeDisplayName = "THREE WHEELER";
                                    objVehicleType.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                }
                                else if (vehicleTypeCode.ToUpper() == "4W")
                                {
                                    objVehicleType.VehicleInActiveImage = "Fourwheeler_circle.png";
                                    objVehicleType.VehicleActiveImage = "Fourwheeler_circle_ticked.png";
                                    objVehicleType.VehicleIcon = "car_black.png";
                                    objVehicleType.VehicleTypeDisplayName = "CAR";
                                    objVehicleType.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                }
                                else if (vehicleTypeCode.ToUpper() == "HW")
                                {
                                    objVehicleType.VehicleInActiveImage = "bus.png";
                                    objVehicleType.VehicleActiveImage = "bus_active.png";
                                    objVehicleType.VehicleIcon = "hv_black.png";
                                    objVehicleType.VehicleTypeDisplayName = "Heavy Vehicle";
                                    objVehicleType.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                }
                                
                                lstVehicleType.Add(objVehicleType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetAllVehicleTypes", "GetAllVehicleTypes");
            }
            return lstVehicleType;
        }

        public decimal GetVehicleDueAmount(CustomerVehicle objCustomerVehicle)
        {
            DataTable resultdt = new DataTable();
            decimal totalDueAmount = 0;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleDueAmount", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objCustomerVehicle.VehicleTypeID.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objCustomerVehicle.RegistrationNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            totalDueAmount = resultdt.Rows[0]["TOTALDUEAMOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["TOTALDUEAMOUNT"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleDueAmount", "GetVehicleDueAmount");


            }

            return totalDueAmount;
        }

        #region Reports
        public VMLocationLotParkingReport GetLocationLotParkingReport(User objlotuser)
        {
            VMLocationLotParkingReport objParkingReport = new VMLocationLotParkingReport();

            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetLocationLotParkingReport", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            objParkingReport = new VMLocationLotParkingReport();
                            DataTable resultdt = new DataTable();
                            DataTable dtSummary = new DataTable();
                            resultdt = ds.Tables[0];
                            dtSummary = ds.Tables[1];
                            if (resultdt.Rows.Count > 0)
                            {
                                ObservableCollection<LotParkingReport> lstLotParkingReport = new ObservableCollection<LotParkingReport>();
                                for (var i = 0; i < resultdt.Rows.Count; i++)
                                {
                                    LotParkingReport objLotParkingReport = new LotParkingReport();
                                    objLotParkingReport.Id = (i + 1);
                                    objLotParkingReport.Currency = "₹";
                                    objLotParkingReport.VehicleType = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                    objLotParkingReport.IsVisible = false;
                                    objLotParkingReport.TotalIn = resultdt.Rows[i]["In"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["In"]);
                                    objLotParkingReport.TotalOut = resultdt.Rows[i]["Out"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Out"]);
                                    objLotParkingReport.TotalCash = resultdt.Rows[i]["Cash"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Cash"]);
                                    objLotParkingReport.TotalEpay = resultdt.Rows[i]["Epay"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Epay"]);
                                    objLotParkingReport.TotalFOC = resultdt.Rows[i]["FOC"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["FOC"]);
                                    if (resultdt.Rows[i]["VehicleTypeCode"] != DBNull.Value || Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]) == "")
                                    {
                                        objLotParkingReport.LocationLotOperations = GetLocationLotOperationsReport(objlotuser, Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]));
                                        DataTable resultOtherInOut = GetLocationLotOtherInOut(objlotuser, Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]));
                                        if (resultOtherInOut.Rows.Count > 0)
                                        {
                                            objLotParkingReport.OtherIn = resultOtherInOut.Rows[0]["OtherIn"] == DBNull.Value ? "" : Convert.ToString(resultOtherInOut.Rows[0]["OtherIn"]);
                                            objLotParkingReport.OtherOut = resultOtherInOut.Rows[0]["OtherOut"] == DBNull.Value ? "" : Convert.ToString(resultOtherInOut.Rows[0]["OtherOut"]);
                                        }
                                    }

                                    lstLotParkingReport.Add(objLotParkingReport);

                                }
                                objParkingReport.LotParkingReportList = lstLotParkingReport;



                            }
                            if (dtSummary.Rows.Count > 0)
                            {

                                objParkingReport.LotTotalCheckIn = dtSummary.Rows[0]["In"] == DBNull.Value ? "" : Convert.ToString(dtSummary.Rows[0]["In"]);
                                objParkingReport.LotTotalCheckOut = dtSummary.Rows[0]["Out"] == DBNull.Value ? "" : Convert.ToString(dtSummary.Rows[0]["Out"]);
                                objParkingReport.LotTotalFOC = dtSummary.Rows[0]["FOC"] == DBNull.Value ? "" : Convert.ToString(dtSummary.Rows[0]["FOC"]);
                                objParkingReport.LotRevenueCash = dtSummary.Rows[0]["Cash"] == DBNull.Value ? "" : Convert.ToString(dtSummary.Rows[0]["Cash"]);
                                objParkingReport.LotRevenueEpay = dtSummary.Rows[0]["Epay"] == DBNull.Value ? "" : Convert.ToString(dtSummary.Rows[0]["Epay"]);
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetVehicleParkingHistory");
            }
            return objParkingReport;

        }
        public List<LocationLotOperations> GetLocationLotOperationsReport(User objlotuser, string VehicleTypeCode)
        {
            List<LocationLotOperations> lstLocationLotOperations = new List<LocationLotOperations>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_LocationLotOperations", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", VehicleTypeCode);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                LocationLotOperations objLocationLotOperations = new LocationLotOperations();
                                objLocationLotOperations.ParkingHours = resultdt.Rows[i]["Duration"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["Duration"]);
                                objLocationLotOperations.CheckIn = resultdt.Rows[i]["In"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["In"]);
                                objLocationLotOperations.CheckOut = resultdt.Rows[i]["Out"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["Out"]);
                                objLocationLotOperations.Cash = resultdt.Rows[i]["Cash"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["Cash"]);
                                objLocationLotOperations.Epay = resultdt.Rows[i]["Epay"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["Epay"]);
                                objLocationLotOperations.FOC = resultdt.Rows[i]["FOC"] == DBNull.Value ? "0" : Convert.ToString(resultdt.Rows[i]["FOC"]);
                                lstLocationLotOperations.Add(objLocationLotOperations);
                            }
                        }

                    }
                }


            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetLocationLotOperationsReport");


            }
            return lstLocationLotOperations;

        }  // Revenue Report

        public VMLocationLotViolations GetLocationLotViolationsReport(User objlotuser)
        {
            VMLocationLotViolations objVMLocationLotViolations = new VMLocationLotViolations();
            List<StationClampedReport> lstLocationLotViolationReport = new List<StationClampedReport>();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetLocationLotViolationReport", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {

                            DataTable dtViolations = new DataTable();
                            DataTable dtViolationSummary = new DataTable();

                            dtViolations = ds.Tables[0];
                            dtViolationSummary = ds.Tables[1];

                            if (dtViolations.Rows.Count > 0)
                            {

                                for (var i = 0; i < dtViolations.Rows.Count; i++)
                                {
                                    StationClampedReport objLocationLotViolationReport = new StationClampedReport();
                                    objLocationLotViolationReport.VehicleTypeID.VehicleTypeCode = dtViolations.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(dtViolations.Rows[i]["VehicleTypeCode"]);
                                    objLocationLotViolationReport.NoOfClamps = dtViolations.Rows[i]["Violation"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolations.Rows[i]["Violation"]);
                                    objLocationLotViolationReport.NoOfWarningClamps = dtViolations.Rows[i]["NoOfWarningClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolations.Rows[i]["NoOfWarningClamps"]);
                                    objLocationLotViolationReport.NoOfPaidClamps = dtViolations.Rows[i]["NoOfPaidClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolations.Rows[i]["NoOfPaidClamps"]);
                                    objLocationLotViolationReport.NoOfUnPaidClamps = dtViolations.Rows[i]["NoOfUnPaidClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolations.Rows[i]["NoOfUnPaidClamps"]);
                                    objLocationLotViolationReport.Cash = dtViolations.Rows[i]["Cash"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtViolations.Rows[i]["Cash"]);
                                    objLocationLotViolationReport.EPay = dtViolations.Rows[i]["EPay"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtViolations.Rows[i]["EPay"]);
                                    objLocationLotViolationReport.Currency = "₹";
                                    lstLocationLotViolationReport.Add(objLocationLotViolationReport);

                                }

                                objVMLocationLotViolations.LocationLotViolationReport = lstLocationLotViolationReport;
                                objVMLocationLotViolations.TotalClamp = dtViolationSummary.Rows[0]["TotalViolation"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolationSummary.Rows[0]["TotalViolation"]);
                                objVMLocationLotViolations.TotalWarningClamps = dtViolationSummary.Rows[0]["TotalWarningClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolationSummary.Rows[0]["TotalWarningClamps"]);
                                objVMLocationLotViolations.TotalUnPaidClamps = dtViolationSummary.Rows[0]["TotalUnPaidClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolationSummary.Rows[0]["TotalUnPaidClamps"]);
                                objVMLocationLotViolations.TotalPaidClamps = dtViolationSummary.Rows[0]["TotalPaidClamps"] == DBNull.Value ? 0 : Convert.ToInt32(dtViolationSummary.Rows[0]["TotalPaidClamps"]);
                                objVMLocationLotViolations.TotalCash = dtViolationSummary.Rows[0]["TotalCash"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtViolationSummary.Rows[0]["TotalCash"]);
                                objVMLocationLotViolations.TotalEPay = dtViolationSummary.Rows[0]["TotalEPay"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtViolationSummary.Rows[0]["TotalEPay"]);

                                objVMLocationLotViolations.Currency = "₹";

                            }
                        }

                    }

                }


            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_Report_GetLocationLotViolationReport", "GetLocationLotViolationsReport");
            }
            return objVMLocationLotViolations;

        }  // Violations
        public VMLocationLotPassReport GetLocationLotPassReport(User objlotuser)
        {
            VMLocationLotPassReport objVMLocationLotPassReport = new VMLocationLotPassReport();
            List<StationPassesReport> lstLocationLotPassesReport = new List<StationPassesReport>();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetLocationLotPassReport", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {

                            DataTable dtPasses = new DataTable();
                            DataTable dtPassSummary = new DataTable();
                            dtPasses = ds.Tables[0];
                            dtPassSummary = ds.Tables[1];
                            if (dtPasses.Rows.Count > 0)
                            {
                                for (var i = 0; i < dtPasses.Rows.Count; i++)
                                {
                                    StationPassesReport objLocationLotPassesReport = new StationPassesReport();
                                    objLocationLotPassesReport.VehicleTypeID.VehicleTypeCode = dtPasses.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(dtPasses.Rows[i]["VehicleTypeCode"]);
                                    objLocationLotPassesReport.PassID.PassTypeCode = dtPasses.Rows[i]["PassTypeCode"] == DBNull.Value ? "" : Convert.ToString(dtPasses.Rows[i]["PassTypeCode"]) + "-" + Convert.ToString(dtPasses.Rows[i]["StationAccess"]);
                                    objLocationLotPassesReport.PassesSold = dtPasses.Rows[i]["Sold"] == DBNull.Value ? 0 : Convert.ToInt32(dtPasses.Rows[i]["Sold"]);
                                    objLocationLotPassesReport.NFCSold = dtPasses.Rows[i]["NFC"] == DBNull.Value ? 0 : Convert.ToInt32(dtPasses.Rows[i]["NFC"]);
                                    objLocationLotPassesReport.PassesCash = dtPasses.Rows[i]["Cash"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtPasses.Rows[i]["Cash"]);
                                    objLocationLotPassesReport.PassesEPay = dtPasses.Rows[i]["EPay"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtPasses.Rows[i]["EPay"]);
                                    objLocationLotPassesReport.Currency = "₹";
                                    lstLocationLotPassesReport.Add(objLocationLotPassesReport);
                                }
                                objVMLocationLotPassReport.StationPasses = lstLocationLotPassesReport;
                                objVMLocationLotPassReport.TotalSold = dtPassSummary.Rows[0]["TOTALSOLD"] == DBNull.Value ? 0 : Convert.ToInt32(dtPassSummary.Rows[0]["TOTALSOLD"]);
                                objVMLocationLotPassReport.TotalCash = dtPassSummary.Rows[0]["TOTALCASH"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtPassSummary.Rows[0]["TOTALCASH"]);
                                objVMLocationLotPassReport.TotalEPay = dtPassSummary.Rows[0]["TOTALEPAY"] == DBNull.Value ? 0.00m : Convert.ToDecimal(dtPassSummary.Rows[0]["TOTALEPAY"]);
                                objVMLocationLotPassReport.TotalNFC = dtPassSummary.Rows[0]["TotalNFC"] == DBNull.Value ? 0 : Convert.ToInt32(dtPassSummary.Rows[0]["TotalNFC"]);
                                objVMLocationLotPassReport.Currency = "₹";

                            }
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetLocationLotPassReport");


            }
            return objVMLocationLotPassReport;

        }
        public DataTable GetLocationLotOtherInOut(User objlotuser, string VehicleTypeCode)
        {
            DataTable dtOtherInOut = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_LocationLotOtherInOut", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", VehicleTypeCode);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(dtOtherInOut);

                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_Report_LocationLotOtherInOut", "GetLocationLotOtherInOut");
            }
            return dtOtherInOut;
        }

        #endregion

        #region RecentCheckOuts
        public RecentCheckOutReport GetRecentCheckOutsReport(RecentCheckOutFilter objfilter)
        {
            RecentCheckOutReport objCheckOutReport = new RecentCheckOutReport();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetRecentCheckOuts", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objfilter.LocationID == 0 ? (object)DBNull.Value : objfilter.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objfilter.LocationParkingLotID == 0 ? (object)DBNull.Value : objfilter.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objfilter.UserID == null || objfilter.UserID == 0) ? (object)DBNull.Value : objfilter.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@SelectedDay", objfilter.SelectedDay == null ? (object)DBNull.Value : objfilter.SelectedDay);
                        sqlcmd_obj.Parameters.AddWithValue("@Ins", objfilter.Ins == false ? false : objfilter.Ins);
                        sqlcmd_obj.Parameters.AddWithValue("@Outs", objfilter.Outs == false ? false : objfilter.Outs);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objfilter.VehicleTypeCode == "" ? (object)DBNull.Value : objfilter.VehicleTypeCode);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dtRecentCheckOuts = ds.Tables[0];
                            DataTable dtCashSummary = ds.Tables[1];

                            if (dtRecentCheckOuts.Rows.Count > 0)
                            {

                                for (var i = 0; i < dtRecentCheckOuts.Rows.Count; i++)
                                {
                                    VMRecentCheckOuts objVMCheckOut = new VMRecentCheckOuts();
                                    objVMCheckOut.CustomerParkingSlotID = dtRecentCheckOuts.Rows[i]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecentCheckOuts.Rows[i]["CustomerParkingSlotID"]);
                                    objVMCheckOut.RegistrationNumber = dtRecentCheckOuts.Rows[i]["RegistrationNumber"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["RegistrationNumber"]);
                                    objVMCheckOut.StatusID.StatusCode = dtRecentCheckOuts.Rows[i]["StatusCode"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["StatusCode"]);
                                    objVMCheckOut.ActualStartTime = dtRecentCheckOuts.Rows[i]["ActualStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(dtRecentCheckOuts.Rows[i]["ActualStartTime"]);
                                    objVMCheckOut.ActualEndTime = dtRecentCheckOuts.Rows[i]["ActualEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(dtRecentCheckOuts.Rows[i]["ActualEndTime"]);
                                    objVMCheckOut.Duration = dtRecentCheckOuts.Rows[i]["Duration"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["Duration"]) + " hr";
                                    objVMCheckOut.Operator.UserID = dtRecentCheckOuts.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecentCheckOuts.Rows[i]["UserID"]);
                                    objVMCheckOut.Operator.UserName = dtRecentCheckOuts.Rows[i]["UserName"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["UserName"]);
                                    objVMCheckOut.Operator.UserCode = dtRecentCheckOuts.Rows[i]["UserCode"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["UserCode"]);
                                    objVMCheckOut.CashAmount = dtRecentCheckOuts.Rows[i]["Cash"] == DBNull.Value ? 0 : Convert.ToDecimal(dtRecentCheckOuts.Rows[i]["Cash"]);
                                    objVMCheckOut.EpayAmount = dtRecentCheckOuts.Rows[i]["Epay"] == DBNull.Value ? 0 : Convert.ToDecimal(dtRecentCheckOuts.Rows[i]["Epay"]);
                                    objVMCheckOut.ApplicationTypeID.ApplicationTypeCode = dtRecentCheckOuts.Rows[i]["ApplicationTypeCode"] == DBNull.Value ? "" : Convert.ToString(dtRecentCheckOuts.Rows[i]["ApplicationTypeCode"]);
                                    objVMCheckOut.VehicleTypeID.VehicleTypeID = Convert.ToInt32(dtRecentCheckOuts.Rows[i]["VehicleTypeID"]);
                                    objVMCheckOut.VehicleTypeID.VehicleTypeCode = Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeCode"]);
                                    objVMCheckOut.VehicleTypeID.VehicleTypeName = Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeName"]);
                                    //bool IsViolationRec = dtRecentCheckOuts.Rows[i]["IsViolation"] == DBNull.Value ? false : Convert.ToBoolean(dtRecentCheckOuts.Rows[i]["IsViolation"]);
                                    objVMCheckOut.ApplicationTypeID.ApplicationTypeColor = "#3293fa";
                                    if (Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeCode"]).ToUpper() == "2W".ToUpper())
                                    {
                                        objVMCheckOut.VehicleTypeID.VehicleImage = "bike_black.png";
                                    }
                                    else if (Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeCode"]).ToUpper() == "4W".ToUpper())
                                    {
                                        objVMCheckOut.VehicleTypeID.VehicleImage = "car_black.png";
                                    }
                                    else if (Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeCode"]).ToUpper() == "3W".ToUpper())
                                    {
                                        objVMCheckOut.VehicleTypeID.VehicleImage = "ThreeW_black.png";
                                    }
                                    else if (Convert.ToString(dtRecentCheckOuts.Rows[i]["VehicleTypeCode"]).ToUpper() == "HW".ToUpper())
                                    {
                                        objVMCheckOut.VehicleTypeID.VehicleImage = "hv_black.png";
                                    }
                                    objVMCheckOut.VehilceStatusColor = "#010101";
                                    if (Convert.ToString(dtRecentCheckOuts.Rows[i]["StatusCode"]) == "V")
                                    {
                                        objVMCheckOut.VehilceStatusColor = "#ff0000";
                                        objVMCheckOut.ApplicationTypeID.ApplicationTypeCode = "V";
                                        objVMCheckOut.ApplicationTypeID.ApplicationTypeColor = "#ff0000";
                                    }
                                    if (Convert.ToString(dtRecentCheckOuts.Rows[i]["StatusCode"]) == "FOC")
                                    {
                                        objVMCheckOut.VehilceStatusColor = "#ff0000";
                                        objVMCheckOut.ApplicationTypeID.ApplicationTypeColor = "#ff0000";
                                    }
                                    objCheckOutReport.RecentCheckOutID.Add(objVMCheckOut);
                                }
                                if (dtCashSummary.Rows.Count > 0)
                                {
                                    objCheckOutReport.TotalCash = dtCashSummary.Rows[0]["Cash"] == DBNull.Value ? 0 : Convert.ToDecimal(dtCashSummary.Rows[0]["Cash"]);
                                    objCheckOutReport.TotalEpay = dtCashSummary.Rows[0]["Epay"] == DBNull.Value ? 0 : Convert.ToDecimal(dtCashSummary.Rows[0]["Epay"]);
                                }
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetRecentCheckOutsReport");
            }
            return objCheckOutReport;

        }
        public List<CustomerParkingSlot> GetRecentCheckOutsVehicleDetails(int CustomerParkingSlotID)
        {
            List<CustomerParkingSlot> lstCustomerParkingSlot = new List<CustomerParkingSlot>();
            DataTable resultdt = new DataTable();
            int VehicleWarningCount = 0;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetRecentCheckOutVehicleDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotID", CustomerParkingSlotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                decimal totalParkingAmount = 0;
                                CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
                                objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[i]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerParkingSlotID"]);
                                objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[i]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerID"]);
                                objCustomerParkingSlot.CustomerID.Name = Convert.ToString(resultdt.Rows[i]["Name"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[i]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]);
                                objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[i]["LocationParkingLotName"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[i]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ParkingBayID"]);
                                objCustomerParkingSlot.ParkingBayID.ParkingBayID = resultdt.Rows[i]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ParkingBayID"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[i]["ParkingBayName"]);
                                objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[i]["ParkingBayRange"]);
                                objCustomerParkingSlot.ExpectedStartTime = resultdt.Rows[i]["ExpectedStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ExpectedStartTime"]);
                                objCustomerParkingSlot.ExpectedEndTime = resultdt.Rows[i]["ExpectedEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ExpectedEndTime"]);
                                objCustomerParkingSlot.ActualStartTime = resultdt.Rows[i]["ActualStartTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ActualStartTime"]);

                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objCustomerParkingSlot.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "2W".ToUpper())
                                {
                                    objCustomerParkingSlot.VehicleTypeID.VehicleImage = "bike_black.png";
                                }
                                else if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "4W".ToUpper())
                                {
                                    objCustomerParkingSlot.VehicleTypeID.VehicleImage = "car_black.png";
                                }
                                else if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "3W".ToUpper())
                                {
                                    objCustomerParkingSlot.VehicleTypeID.VehicleImage = "ThreeW_black.png";
                                }
                                else if (Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]).ToUpper() == "HW".ToUpper())
                                {
                                    objCustomerParkingSlot.VehicleTypeID.VehicleImage = "hv_black.png";
                                }
                                objCustomerParkingSlot.Duration = Convert.ToString(resultdt.Rows[i]["Duration"]);
                                objCustomerParkingSlot.PaymentTypeID.PaymentTypeID = resultdt.Rows[i]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["PaymentTypeID"]);
                                objCustomerParkingSlot.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[i]["PaymentTypeName"]);
                                objCustomerParkingSlot.CreatedBy = resultdt.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserID"]);
                                objCustomerParkingSlot.CreatedByName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objCustomerParkingSlot.UserCode = Convert.ToString(resultdt.Rows[i]["UserCode"]);

                                if (resultdt.Rows.Count > 1 && i == 0)
                                {
                                    objCustomerParkingSlot.UpdatedBy = "";
                                    objCustomerParkingSlot.ActualEndTime = (Nullable<DateTime>)null;
                                }
                                else
                                {
                                    objCustomerParkingSlot.ActualEndTime = resultdt.Rows[i]["ActualEndTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ActualEndTime"]);
                                    objCustomerParkingSlot.UpdatedBy = resultdt.Rows[i]["CheckOutBy"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["CheckOutBy"]);
                                }
                                objCustomerParkingSlot.PaidAmount = resultdt.Rows[i]["PARKINGAMOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["PARKINGAMOUNT"]);


                                if (Convert.ToString(resultdt.Rows[i]["ApplicationTypeCode"]) == "P")
                                {
                                    objCustomerParkingSlot.PaidAmount = ((resultdt.Rows[i]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ExtendAmount"])));
                                }
                                objCustomerParkingSlot.ExtendAmount = resultdt.Rows[i]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ExtendAmount"]);
                                objCustomerParkingSlot.DueAmount = resultdt.Rows[i]["DueAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["DueAmount"]);
                                objCustomerParkingSlot.IsWarning = resultdt.Rows[i]["IsWarning"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsWarning"]);
                                objCustomerParkingSlot.IsClamp = resultdt.Rows[i]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsClamp"]);
                                objCustomerParkingSlot.ClampFees = resultdt.Rows[i]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ClampFee"]);
                                //Total Parking Amount
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) != "FOC")
                                {
                                    totalParkingAmount = ((resultdt.Rows[i]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["Amount"])) + (resultdt.Rows[i]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["ExtendAmount"])));
                                }

                                objCustomerParkingSlot.Amount = totalParkingAmount;
                                objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeID = resultdt.Rows[i]["ApplicationTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ApplicationTypeID"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = Convert.ToString(resultdt.Rows[i]["ApplicationTypeCode"]);
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeName = Convert.ToString(resultdt.Rows[i]["ApplicationTypeName"]);
                                objCustomerParkingSlot.ViolationReasonID.ViolationReasonID = resultdt.Rows[i]["ViolationReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ViolationReasonID"]);
                                objCustomerParkingSlot.ViolationReasonID.Reason = Convert.ToString(resultdt.Rows[i]["Reason"]);
                                //FOC Reason
                                objCustomerParkingSlot.FOCReasonID.ViolationReasonID = resultdt.Rows[i]["FOCReasonID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["FOCReasonID"]);
                                objCustomerParkingSlot.FOCReasonID.Reason = Convert.ToString(resultdt.Rows[i]["FOCReason"]);
                                //FOC Reason END
                                objCustomerParkingSlot.VehicleParkingImage = resultdt.Rows[i]["VehicleParkingImage"] == DBNull.Value ? null : (byte[])resultdt.Rows[i]["VehicleParkingImage"];
                                objCustomerParkingSlot.StatusID.StatusCode = Convert.ToString(resultdt.Rows[i]["StatusCode"]);
                                objCustomerParkingSlot.StatusID.StatusName = Convert.ToString(resultdt.Rows[i]["StatusName"]);
                                objCustomerParkingSlot.StatusID.StatusColor = "#010101";  // App Style
                                objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeColor = "#3293fa";
                                bool IsViolationRec = resultdt.Rows[i]["IsViolation"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsViolation"]);
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "C" || Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "V" || IsViolationRec)
                                {
                                    objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = "V";
                                    objCustomerParkingSlot.StatusID.StatusColor = "#ff0000";  // App Style
                                    objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeColor = "#ff0000";
                                }
                                if (Convert.ToString(resultdt.Rows[i]["StatusCode"]) == "FOC")
                                {
                                    if (IsViolationRec)
                                    {
                                        objCustomerParkingSlot.PaidAmount = 0; // If record entered through violatiion and status is FOC
                                        objCustomerParkingSlot.ApplicationTypeID.ApplicationTypeCode = "V";
                                    }
                                    objCustomerParkingSlot.StatusID.StatusColor = "#ff0000";  // App Style
                                }
                                objCustomerParkingSlot.ViolationWarningCount = resultdt.Rows[i]["ViolationWarningCount"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ViolationWarningCount"]);
                                lstCustomerParkingSlot.Add(objCustomerParkingSlot);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_Get_ParkedVehicleDetails", "GetRecentCheckOutsVehicleDetails");


            }
            return lstCustomerParkingSlot;

        }
        #endregion

        #region Lot Occupancy
        public VMLocationLotOccupancyReport GetLotOccupancyReport(User objlotuser)
        {
            List<LocationLotOccupancyReport> lstLotOccupancyReport = new List<LocationLotOccupancyReport>();
            VMLocationLotOccupancyReport objVMLocationLotOccupancyReport = new VMLocationLotOccupancyReport();

            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Report_GetLotOccupancy", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objlotuser.LocationParkingLotID.LocationID.LocationID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objlotuser.LocationParkingLotID.LocationParkingLotID == 0 ? (object)DBNull.Value : objlotuser.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StartDateTime", objlotuser.LoginTime == null ? (object)DBNull.Value : objlotuser.LoginTime);
                        sqlcmd_obj.Parameters.AddWithValue("@EndDateTime", objlotuser.LogoutTime == null ? (object)DBNull.Value : objlotuser.LogoutTime);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objlotuser.UserID == 0 ? (object)DBNull.Value : objlotuser.UserID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataSet ds = new DataSet();
                        sqldap.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dtLotOccupancy = new DataTable();
                            DataTable dtSummary = new DataTable();
                            dtLotOccupancy = ds.Tables[0];
                            dtSummary = ds.Tables[1];
                            if (dtLotOccupancy.Rows.Count > 0)
                            {
                                for (var i = 0; i < dtLotOccupancy.Rows.Count; i++)
                                {
                                    LocationLotOccupancyReport objoccupancy = new LocationLotOccupancyReport();
                                    objoccupancy.ChekcInDuration = dtLotOccupancy.Rows[i]["Duration"] == DBNull.Value ? "0" : Convert.ToString(dtLotOccupancy.Rows[i]["Duration"]);
                                    objoccupancy.TwoWheeler = dtLotOccupancy.Rows[i]["2W"] == DBNull.Value ? "" : Convert.ToString(dtLotOccupancy.Rows[i]["2W"]);
                                    objoccupancy.FourWheeler = dtLotOccupancy.Rows[i]["4W"] == DBNull.Value ? "" : Convert.ToString(dtLotOccupancy.Rows[i]["4W"]);
                                    objoccupancy.ThreeWheeler = dtLotOccupancy.Rows[i]["3W"] == DBNull.Value ? "" : Convert.ToString(dtLotOccupancy.Rows[i]["3W"]);
                                    objoccupancy.HeavyWheeler = dtLotOccupancy.Rows[i]["HW"] == DBNull.Value ? "" : Convert.ToString(dtLotOccupancy.Rows[i]["HW"]);
                                    lstLotOccupancyReport.Add(objoccupancy);
                                }
                                if (dtSummary.Rows.Count > 0)
                                {
                                    LocationLotOccupancyReport objtoalSum = new LocationLotOccupancyReport();
                                    objtoalSum.ChekcInDuration = "Total";
                                    objtoalSum.TwoWheeler = dtSummary.Rows[0]["TOTAL2W"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TOTAL2W"]);
                                    objtoalSum.FourWheeler = dtSummary.Rows[0]["TOTAL4W"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TOTAL4W"]);
                                    objtoalSum.ThreeWheeler = dtSummary.Rows[0]["TOTAL3W"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TOTAL3W"]);
                                    objtoalSum.HeavyWheeler = dtSummary.Rows[0]["TOTALHW"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TOTALHW"]);

                                    objVMLocationLotOccupancyReport.LocationLotOccupancyReportID = lstLotOccupancyReport;
                                    objVMLocationLotOccupancyReport.TotalTwoWheelerPercentage = dtSummary.Rows[0]["TwoWheelerPercentage"] == DBNull.Value ? "0" + " (%)" : Convert.ToString(dtSummary.Rows[0]["TwoWheelerPercentage"]) + " (%)";
                                    objVMLocationLotOccupancyReport.TotalThreeWheelerPercentage = dtSummary.Rows[0]["ThreeWheelerPercentage"] == DBNull.Value ? "0" + " (%)" : Convert.ToString(dtSummary.Rows[0]["ThreeWheelerPercentage"]) + " (%)";
                                    objVMLocationLotOccupancyReport.TotalFourWheelerPercentage = dtSummary.Rows[0]["FourWheelerPercentage"] == DBNull.Value ? "0" + " (%)" : Convert.ToString(dtSummary.Rows[0]["FourWheelerPercentage"]) + " (%)";
                                    objVMLocationLotOccupancyReport.TotalHeavyWheelerPercentage = dtSummary.Rows[0]["HeavyWheelerPercentage"] == DBNull.Value ? "0" + " (%)" : Convert.ToString(dtSummary.Rows[0]["HeavyWheelerPercentage"]) + " (%)";

                                    objVMLocationLotOccupancyReport.TotalTwoWheelerLotCapacity = dtSummary.Rows[0]["TwoWheelerLotCapacity"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TwoWheelerLotCapacity"]);
                                    objVMLocationLotOccupancyReport.TotalThreeWheelerLotCapacity = dtSummary.Rows[0]["ThreeWheelerLotCapacity"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["ThreeWheelerLotCapacity"]);
                                    objVMLocationLotOccupancyReport.TotalFourWheelerLotCapacity = dtSummary.Rows[0]["FourWheelerLotCapacity"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["FourWheelerLotCapacity"]);
                                    objVMLocationLotOccupancyReport.TotalHeavyWheelerLotCapacity = dtSummary.Rows[0]["HeavyWheelerLotCapacity"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["HeavyWheelerLotCapacity"]);

                                    objVMLocationLotOccupancyReport.TotalTwoWheelerLotCapacity = objVMLocationLotOccupancyReport.TotalTwoWheelerLotCapacity + "(" + objtoalSum.TwoWheeler + ")";
                                    objVMLocationLotOccupancyReport.TotalThreeWheelerLotCapacity = objVMLocationLotOccupancyReport.TotalThreeWheelerLotCapacity + "(" + objtoalSum.ThreeWheeler + ")";
                                    objVMLocationLotOccupancyReport.TotalFourWheelerLotCapacity = objVMLocationLotOccupancyReport.TotalFourWheelerLotCapacity + "(" + objtoalSum.FourWheeler + ")";
                                    objVMLocationLotOccupancyReport.TotalHeavyWheelerLotCapacity = objVMLocationLotOccupancyReport.TotalHeavyWheelerLotCapacity + "(" + objtoalSum.HeavyWheeler + ")";
                                }


                            }




                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "OPAPP_PROC_Report_GetLotOccupancy", "Proc: " + "OPAPP_PROC_GetVehicleParkingHistory", "GetLotOccupancyReport");
            }
            return objVMLocationLotOccupancyReport;

        }
        #endregion

        #region Firebase - SQL Server Functions
        public CustomerParkingSlot GetParkedVehicleDetailsFromFirebase(string RegistrationNumber)
        {
            CustomerParkingSlot objCustomerParkingSlot = null;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("Firebase_PROC_GetCheckInVehicleDetails_RegistrationNumber", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objCustomerParkingSlot = new CustomerParkingSlot();
                            objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[0]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerParkingSlotID"]);
                            objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objCustomerParkingSlot.StatusID.StatusID = resultdt.Rows[0]["StatusID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["StatusID"]);
                            objCustomerParkingSlot.StatusID.StatusCode = Convert.ToString(resultdt.Rows[0]["StatusCode"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicleParkingLot", "Proc: " + "OPAPP_PROC_GetParkedVehicleDetails", "GetParkedVehicleDetails");


            }
            return objCustomerParkingSlot;

        }
        #endregion
    }
}