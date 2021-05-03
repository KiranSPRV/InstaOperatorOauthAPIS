using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

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
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                CustomerVehicle objVehicle = new CustomerVehicle();
                                objVehicle.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);

                                objVehicle.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objVehicle.VehicleTypeID.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
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
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicle", "Proc: " + "OPAPP_PROC_GetVehicleRegistrationNumber", "GetAllVehicleRegistrationNumbers");
                throw;

            }
            return lstRegistrationNumber;

        }
        public List<CustomerVehicle> GetAllVehicleRegistrationNumbersBySearch(string RegistrationNumber)
        {
            List<CustomerVehicle> lstRegistrationNumber = new List<CustomerVehicle>();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleRegistrationNumber", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", (RegistrationNumber == string.Empty || RegistrationNumber == "") ? (object)DBNull.Value : RegistrationNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)

                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                CustomerVehicle objVehicle = new CustomerVehicle();
                                string vehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objVehicle.CustomerVehicleID = resultdt.Rows[i]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);
                                objVehicle.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objVehicle.VehicleTypeID.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objVehicle.RegistrationNumber = resultdt.Rows[i]["RegistrationNumber"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                objVehicle.Model = resultdt.Rows[i]["Model"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Model"]);
                                objVehicle.Color = resultdt.Rows[i]["Color"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Color"]);
                                if (vehicleTypeCode.ToUpper() == "2W")
                                {

                                    objVehicle.VehicleTypeID.VehicleTypeDisplayName = "BIKE";
                                    objVehicle.VehicleTypeID.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                    objVehicle.VehicleTypeID.VehicleIcon = "bike_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "3W")
                                {

                                    objVehicle.VehicleTypeID.VehicleTypeDisplayName = "Three Wheeler";
                                    objVehicle.VehicleTypeID.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                    objVehicle.VehicleTypeID.VehicleIcon = "ThreeW_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "4W")
                                {

                                    objVehicle.VehicleTypeID.VehicleTypeDisplayName = "CAR";
                                    objVehicle.VehicleTypeID.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                    objVehicle.VehicleTypeID.VehicleIcon = "car_black.png";
                                }
                                else if (vehicleTypeCode.ToUpper() == "HW")
                                {

                                    objVehicle.VehicleTypeID.VehicleTypeDisplayName = "Heavy Vehicle";
                                    objVehicle.VehicleTypeID.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                    objVehicle.VehicleTypeID.VehicleIcon = "hv_black.png";
                                }

                                lstRegistrationNumber.Add(objVehicle);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicle", "Proc: " + "OPAPP_PROC_GetVehicleRegistrationNumber", "GetAllVehicleRegistrationNumbers");
                throw;

            }
            return lstRegistrationNumber;

        }
        public Customer GetCustomerDetailsByRegistrationNumber(string RegistrationNumber, string VehicleTypeCode)
        {
            Customer objCustomer = new Customer();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Get_CustomerDetails_RegistrationNumber", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", (RegistrationNumber == string.Empty || RegistrationNumber == "") ? (object)DBNull.Value : RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (VehicleTypeCode == string.Empty || VehicleTypeCode == "") ? (object)DBNull.Value : VehicleTypeCode);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objCustomer.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomer.PhoneNumber = resultdt.Rows[0]["PhoneNumber"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objCustomer.Name = resultdt.Rows[0]["Name"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["Name"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicle", "Proc: " + "OPAPP_PROC_Get_CustomerDetails_RegistrationNumber", "GetCustomerDetailsByRegistrationNumber");
                throw;

            }
            return objCustomer;

        }
        public decimal GetVehicleDueAmount(string RegistrationNumber, string VehicleTypeCode)
        {
            decimal DueAmount = 0;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Get_VehicleDueAmount", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", (RegistrationNumber == string.Empty || RegistrationNumber == "") ? (object)DBNull.Value : RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (VehicleTypeCode == string.Empty || VehicleTypeCode == "") ? (object)DBNull.Value : VehicleTypeCode);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            DueAmount = resultdt.Rows[0]["DUEAMOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["DUEAMOUNT"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCustomerVehicle", "Proc: " + "OPAPP_PROC_Get_VehicleDueAmount", "GetVehicleDueAmount");
                throw;

            }
            return DueAmount;

        }
        public List<CustomerParkingSlot> GetVehicleDueAmountHistory(string RegistrationNumber, string VehicleTypeCode)
        {
            List<CustomerParkingSlot> lstVehicleParkingDetails = new List<CustomerParkingSlot>();
            DataTable resultdt = new DataTable();
            int VehicleWarningCount = 0;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_Get_VehicleDueAmountHistory", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", VehicleTypeCode);
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
    }
}