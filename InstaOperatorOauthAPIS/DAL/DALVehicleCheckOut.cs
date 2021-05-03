using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Notification;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALVehicleCheckOut
    {
        public CustomerParkingSlot VehicleCheckOut(CustomerParkingSlot objInPut)
        {

            CustomerParkingSlot objcheckOut = new CustomerParkingSlot();
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveVehicleCheckOut", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotID", objInPut.CustomerParkingSlotID);
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", objInPut.CustomerVehicleID.CustomerVehicleID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objInPut.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@StatusName", objInPut.StatusID.StatusName);
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentType", objInPut.PaymentTypeID.PaymentTypeName);
                        sqlcmd_obj.Parameters.AddWithValue("@IsClamp", objInPut.IsClamp);
                        sqlcmd_obj.Parameters.AddWithValue("@ActualEndTime", objInPut.ActualEndTime);
                        sqlcmd_obj.Parameters.AddWithValue("@Amount", objInPut.Amount);
                        sqlcmd_obj.Parameters.AddWithValue("@ViolationFees", objInPut.ViolationFees);
                        sqlcmd_obj.Parameters.AddWithValue("@ViolationReasonID", (objInPut.ViolationReasonID.ViolationReasonID == 0 || objInPut.ViolationReasonID.ViolationReasonID == null) ? (object)DBNull.Value : objInPut.ViolationReasonID.ViolationReasonID);
                        sqlcmd_obj.Parameters.AddWithValue("@FOCReasonID", (objInPut.FOCReasonID.ViolationReasonID == 0 || objInPut.FOCReasonID.ViolationReasonID == null) ? (object)DBNull.Value : objInPut.FOCReasonID.ViolationReasonID); 
                        sqlcmd_obj.Parameters.AddWithValue("@ClampFees", objInPut.ClampFees);
                        sqlcmd_obj.Parameters.AddWithValue("@ExtendAmount", objInPut.ExtendAmount);
                        sqlcmd_obj.Parameters.AddWithValue("@PaidDueAmount", objInPut.DueAmount == 0  ? (object)DBNull.Value : objInPut.DueAmount);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingDuration", objInPut.Duration);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objInPut.CreatedBy);
                     
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            objcheckOut.CustomerParkingSlotID = resultdt.Rows[0]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerParkingSlotID"]);
                            objcheckOut.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objcheckOut.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objcheckOut.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objcheckOut.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objcheckOut.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objcheckOut.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objcheckOut.LocationParkingLotID.LotCloseTime = Convert.ToString(resultdt.Rows[0]["LotCloseTime"]);
                            objcheckOut.LocationParkingLotID.LotOpenTime = Convert.ToString(resultdt.Rows[0]["LotOpenTime"]);
                            objcheckOut.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[0]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ParkingBayID"]);
                            objcheckOut.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[0]["ParkingBayName"]);
                            objcheckOut.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[0]["ParkingBayRange"]);
                            objcheckOut.ExpectedStartTime = resultdt.Rows[0]["ExpectedStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(resultdt.Rows[0]["ExpectedStartTime"]);
                            objcheckOut.ExpectedEndTime = resultdt.Rows[0]["ExpectedEndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(resultdt.Rows[0]["ExpectedEndTime"]);
                            objcheckOut.ActualStartTime = resultdt.Rows[0]["ActualStartTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(resultdt.Rows[0]["ActualStartTime"]);
                            objcheckOut.ActualEndTime = resultdt.Rows[0]["ActualEndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(resultdt.Rows[0]["ActualEndTime"]);
                            objcheckOut.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objcheckOut.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objcheckOut.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            string vehicleTypeCode = resultdt.Rows[0]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]); ;
                            objcheckOut.Duration = Convert.ToString(resultdt.Rows[0]["Duration"]);
                            objcheckOut.PaymentTypeID.PaymentTypeID = resultdt.Rows[0]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                            objcheckOut.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);
                            objcheckOut.CreatedBy = resultdt.Rows[0]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["UserID"]);
                            objcheckOut.CreatedByName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                            objcheckOut.UserCode = resultdt.Rows[0]["UserCode"] == DBNull.Value ? "": Convert.ToString(resultdt.Rows[0]["UserCode"]);
                            objcheckOut.Amount = resultdt.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                            objcheckOut.ViolationFees = resultdt.Rows[0]["ViolationFees"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ViolationFees"]);
                            objcheckOut.ClampFees = resultdt.Rows[0]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ClampFee"]);
                            objcheckOut.PaidAmount = resultdt.Rows[0]["PaidAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["PaidAmount"]);
                            objcheckOut.DueAmount = resultdt.Rows[0]["DueAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["DueAmount"]);
                            objcheckOut.PaidDueAmount = resultdt.Rows[0]["PaidDueAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["PaidDueAmount"]);
                            objcheckOut.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objcheckOut.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            objcheckOut.IsClamp = resultdt.Rows[0]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IsClamp"]);
                            objcheckOut.ExtendAmount = resultdt.Rows[0]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ExtendAmount"]);
                            objcheckOut.SuperVisorID.UserCode = resultdt.Rows[0]["SUPERVISORCODE"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["SUPERVISORCODE"]);
                            objcheckOut.SuperVisorID.PhoneNumber = resultdt.Rows[0]["SUPERVISORPHONENUMBER"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["SUPERVISORPHONENUMBER"]);
                            objcheckOut.ApplicationTypeID.ApplicationTypeCode = resultdt.Rows[0]["ApplicationTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["ApplicationTypeCode"]);
                            objcheckOut.ApplicationTypeID.ApplicationTypeName = resultdt.Rows[0]["ApplicationTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[0]["ApplicationTypeName"]);
                            objcheckOut.CustomerVehicleID.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objcheckOut.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objcheckOut.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objcheckOut.GSTNumber = "36AACFZ1015E1ZL";
                            if (vehicleTypeCode.ToUpper() == "2W")
                            {
                                objcheckOut.VehicleTypeID.VehicleTypeDisplayName = "BIKE";
                                objcheckOut.VehicleTypeID.VehicleDisplayImage = "Twowheeler_circle.png";  //Default InActive Image
                                objcheckOut.VehicleTypeID.VehicleIcon = "bike_black.png";
                            }
                            else if (vehicleTypeCode.ToUpper() == "3W")
                            {

                                objcheckOut.VehicleTypeID.VehicleTypeDisplayName = "Three Wheeler";
                                objcheckOut.VehicleTypeID.VehicleDisplayImage = "ThreeW.png";  //Default InActive Image
                                objcheckOut.VehicleTypeID.VehicleIcon = "ThreeW_black.png";
                            }
                            else if (vehicleTypeCode.ToUpper() == "4W")
                            {

                                objcheckOut.VehicleTypeID.VehicleTypeDisplayName = "CAR";
                                objcheckOut.VehicleTypeID.VehicleDisplayImage = "Fourwheeler_circle.png";  //Default InActive Image
                                objcheckOut.VehicleTypeID.VehicleIcon = "car_black.png";
                            }
                            else if (vehicleTypeCode.ToUpper() == "HW")
                            {

                                objcheckOut.VehicleTypeID.VehicleTypeDisplayName = "Heavy Vehicle";
                                objcheckOut.VehicleTypeID.VehicleDisplayImage = "bus.png";  //Default InActive Image
                                objcheckOut.VehicleTypeID.VehicleIcon = "hv_black.png";
                            }
                            PushNotification pushNotification = new PushNotification();
                            NotificationContent notificationContent = new NotificationContent();
                            notificationContent.DeviceID = Convert.ToString(resultdt.Rows[0]["DeviceID"]);
                            notificationContent.Title = "Check Out";
                            //notificationContent.TextMessage = "Your vehicle - " + Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]) + " has been checked out from " + Convert.ToString(resultdt.Rows[0]["LocationName"]) + " metro station in the " + Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]) + " lot";

                            notificationContent.TextMessage = "Your vehicle is checked out. Thank you for parking with us";

                            if (notificationContent.DeviceID != "")
                            {
                                pushNotification.SendPushNotification(notificationContent);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_SaveVehicleCheckOut", "VehicleCheckOut");
                throw;

            }
            return objcheckOut;

        }
        public string UpdateVehicleClampStaus(CustomerParkingSlot objInPut)
        {

            string resultmsg = string.Empty;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UpdateVehicleClampStatus", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotID", objInPut.CustomerParkingSlotID);
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", objInPut.CustomerVehicleID.CustomerVehicleID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objInPut.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@IsClamp", objInPut.IsClamp);
                        sqlcmd_obj.Parameters.AddWithValue("@IsWarning", Convert.ToBoolean(objInPut.IsWarning));
                        sqlcmd_obj.Parameters.AddWithValue("@StatusID", objInPut.StatusID.StatusID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeID", objInPut.VehicleTypeID.VehicleTypeID);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objInPut.CustomerVehicleID.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@ViolationReasonID", objInPut.ViolationReasonID.ViolationReasonID);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objInPut.CreatedBy);
                        sqlconn_obj.Open();

                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            PushNotification pushNotification = new PushNotification();
                            NotificationContent notificationContent = new NotificationContent();
                            notificationContent.DeviceID = Convert.ToString(resultdt.Rows[0]["DeviceID"]);
                            notificationContent.Title = "Clamp";
                            //notificationContent.TextMessage = "Your vehicle - " + Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]) + " has been clamped due to " + Convert.ToString(resultdt.Rows[0]["Reason"]) + " at " + Convert.ToString(resultdt.Rows[0]["LocationName"]) + " metro station in the " + Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]) + " lot";
                            notificationContent.TextMessage = "Sorry! Your vehicle is clamped. Please visit the parking lot";
                            if (notificationContent.DeviceID != "")
                            {
                                pushNotification.SendPushNotification(notificationContent);
                            }
                            resultmsg = "Success";
                        }
                        else
                        {
                            resultmsg = "Fail";
                        }

                        /*
                        int result = sqlcmd_obj.ExecuteNonQuery();
                        if (result > 0)
                        {
                            resultmsg = "Success";
                        }
                        else
                        {
                            resultmsg = "Fail";
                        }
                        */
                    }
                }

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_UpdateVehicleClampStatus", "UpdateVehicleClampStaus");
                throw;

            }
            return resultmsg;

        }

        #region Vehicle Auto Checkout/FOC Mehtods
        public int OverStayVehicleAutoCheckOutFOC()
        {

            int resultcount = 0;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@STATUSNAME", "OverStay");
                        sqlcmd_obj.CommandTimeout = 0;
                        sqlconn_obj.Open();
                        resultcount = sqlcmd_obj.ExecuteNonQuery();
                        
                    }
                }

               

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", "OverStayVehicleAutoCheckOutFOC");
                throw;

            }
            return resultcount;

        }
        public int ViolationVehicleAutoCheckOutFOC()
        {

            int resultcount = 0;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@STATUSNAME", "Violation");
                        sqlcmd_obj.CommandTimeout = 0;
                        sqlconn_obj.Open();
                        resultcount = sqlcmd_obj.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", "ViolationVehicleAutoCheckOutFOC");
                throw;

            }
            return resultcount;

        }
        public int CheckInVehicleAutoCheckOutFOC()
        {

            int resultcount = 0;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@STATUSNAME", "CheckIn");
                        sqlcmd_obj.CommandTimeout = 0;
                        sqlconn_obj.Open();
                        resultcount = sqlcmd_obj.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", "CheckInVehicleAutoCheckOutFOC");
                throw;

            }
            return resultcount;

        }
        public int GovernmentVehicleAutoCheckOut()
        {

            int resultcount = 0;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@STATUSNAME", "Government");
                        sqlcmd_obj.CommandTimeout = 0;
                        sqlconn_obj.Open();
                        resultcount = sqlcmd_obj.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALVehicleCheckOut", "Proc: " + "OPAPP_PROC_UPDATE_VEHICLEAUTOCHECKOUT", "GovernmentVehicleAutoCheckOut");
                throw;

            }
            return resultcount;

        }
        #endregion


    }
}