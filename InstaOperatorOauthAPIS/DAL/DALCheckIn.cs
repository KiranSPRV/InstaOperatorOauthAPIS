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
    public class DALCheckIn
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public List<ParkingBay> GetLocationLotBayNumbers(LocationParkingLot obj)
        {

            List<ParkingBay> lstParkingBayNumbers = new List<ParkingBay>();
            string resultMsg = string.Empty;
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetLocationLotBayNumbers", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", obj.LocationID.LocationID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                ParkingBay objparkingbay = new ParkingBay();
                                objparkingbay.ParkingBayID = resultdt.Rows[i]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["ParkingBayID"]);
                                objparkingbay.ParkingBayName = Convert.ToString(resultdt.Rows[i]["ParkingBayName"]);
                                objparkingbay.ParkingBayRange = Convert.ToString(resultdt.Rows[i]["ParkingBayRange"]);
                                objparkingbay.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objparkingbay.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                lstParkingBayNumbers.Add(objparkingbay);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetLocationLotBayNumbers", "GetLocationLotBayNumbers");
                throw;

            }
            return lstParkingBayNumbers;

        }
        public CustomerVehiclePass VerifyVehicleHasPass(VehiclePass obj)
        {


            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVerifyVehicleHasPass", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", obj.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", obj.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", obj.LocationID == 0 ? (object)DBNull.Value : obj.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID == 0 ? (object)DBNull.Value : obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@NFCCardNumber", obj.NFCCardNumber == "" ? (object)DBNull.Value : obj.NFCCardNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                            objResultVehicle.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);

                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);

                            objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);

                            objResultVehicle.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);

                            objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objResultVehicle.PassPriceID.PassPriceID = resultdt.Rows[0]["PassPriceID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PassPriceID"]);
                            objResultVehicle.PassPriceID.NFCCardPrice = resultdt.Rows[0]["NFCCardPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["NFCCardPrice"]);
                            objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[0]["StationAccess"]);
                            objResultVehicle.PassPriceID.Price = resultdt.Rows[0]["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["Price"]);

                            objResultVehicle.PassPriceID.PassTypeID.PassTypeID = resultdt.Rows[0]["PassTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PassTypeID"]);
                            objResultVehicle.PassPriceID.PassTypeID.PassTypeCode = Convert.ToString(resultdt.Rows[0]["PassTypeCode"]);
                            objResultVehicle.PassPriceID.PassTypeID.PassTypeName = Convert.ToString(resultdt.Rows[0]["PassTypeName"]);
                            objResultVehicle.StartDate = resultdt.Rows[0]["StartDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["StartDate"]);
                            objResultVehicle.ExpiryDate = resultdt.Rows[0]["ExpiryDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ExpiryDate"]);
                            objResultVehicle.Amount = resultdt.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                            objResultVehicle.CardAmount = resultdt.Rows[0]["CardAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["CardAmount"]);
                            objResultVehicle.TotalAmount = resultdt.Rows[0]["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["TotalAmount"]);


                            objResultVehicle.CreatedBy.UserName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                            objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);
                            objResultVehicle.IssuedCard = resultdt.Rows[0]["IssuedCard"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IssuedCard"]);
                            objResultVehicle.CardNumber = Convert.ToString(resultdt.Rows[0]["CardNumber"]);
                            objResultVehicle.PaymentTypeID.PaymentTypeID = resultdt.Rows[0]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                            objResultVehicle.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);




                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetVerifyVehicleHasPass", "VerifyVehicleHasPass");
                throw;

            }
            return objResultVehicle;

        }
       

        public VehicleParkingFee GetVehicleParkingFee(VehicleParkingFee obj)
        {


            VehicleParkingFee objVehicleParkingFee = new VehicleParkingFee();
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetVehicleParkingFeeDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingHours", obj.ParkingHours);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeID", obj.VehicleTypeID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingStartTime", obj.ParkingStartTime);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objVehicleParkingFee.Fees = Convert.ToDecimal(resultdt.Rows[0]["PRICE"]);
                            objVehicleParkingFee.ParkingHours = obj.ParkingHours;
                            objVehicleParkingFee.LocationParkingLotID = obj.LocationParkingLotID;
                            objVehicleParkingFee.SpotExpireTime = Convert.ToString(resultdt.Rows[0]["SpotExpireTime"]);
                            objVehicleParkingFee.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetVehicleParkingFeeDetails", "GetVehicleParkingFee");


            }
            return objVehicleParkingFee;

        }
        public List<VehicleParkingFee> GetLocationParkingLotVehicleParkingFee(VehicleParkingFee obj)
        {
            List<VehicleParkingFee> lstVehicleParkingFee = new List<VehicleParkingFee>();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetLocationParkingLotVehicleParkingFeeDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingHours", obj.ParkingHours);
                        sqlcmd_obj.Parameters.AddWithValue("@PaidAmount", (obj.PaidAmount==null|| obj.PaidAmount ==0)?0: obj.PaidAmount); 
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                VehicleParkingFee objVehicleParkingFee = new VehicleParkingFee();
                                objVehicleParkingFee.Fees = resultdt.Rows[i]["PRICE"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["PRICE"]);
                                objVehicleParkingFee.Duration = resultdt.Rows[i]["Duration"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["Duration"]);
                                objVehicleParkingFee.LocationParkingLotID = resultdt.Rows[i]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]);
                                objVehicleParkingFee.LotOpenTime = Convert.ToString(resultdt.Rows[i]["LotOpenTime"]);
                                objVehicleParkingFee.LotCloseTime = Convert.ToString(resultdt.Rows[i]["LotCloseTime"]);
                                objVehicleParkingFee.DayOfWeek = Convert.ToString(resultdt.Rows[i]["DayOfWeek"]);
                                objVehicleParkingFee.IsFullDay = resultdt.Rows[i]["IsFullDay"] ==DBNull.Value?false: Convert.ToBoolean(resultdt.Rows[i]["IsFullDay"]);
                                lstVehicleParkingFee.Add(objVehicleParkingFee);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetLocationParkingLotVehicleParkingFeeDetails", "GetLocationParkingLotVehicleParkingFee");
            }
            return lstVehicleParkingFee;
        }
        public string SavePassVehicleCheckIn(VehicleCheckIn obj)
        {
            string resultmsg = string.Empty;
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SavePassVehicleCheckIn", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerID", obj.CustomerID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (obj.LocationParkingLotID == null || obj.LocationParkingLotID == 0) ? (object)DBNull.Value : obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", obj.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@BayNumberID", (obj.BayNumberID==null|| obj.BayNumberID == 0) ?(object)DBNull.Value: obj.BayNumberID);
                        sqlcmd_obj.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber=="" ? (object)DBNull.Value: obj.PhoneNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", obj.UserID);
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
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_SavePassVehicleCheckIn", "SavePassVehicleCheckIn");
                throw;

            }
            return resultmsg;
        }
        public string GovernmentVehicleCheckIn(VehicleCheckIn obj)
        {
            string resultmsg = string.Empty;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveGovernmentVehicleCheckIn", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerID", obj.CustomerID == null || obj.CustomerID == 0 ? (Object)DBNull.Value : obj.CustomerID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", obj.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@BayNumberID", obj.BayNumberID);
                        sqlcmd_obj.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@StatusName", obj.StatusName);
                        sqlcmd_obj.Parameters.AddWithValue("@GovernmentVehicleImage", obj.GovernmentVehicleImage);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", obj.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleImageLottitude", obj.VehicleImageLottitude==null|| obj.VehicleImageLottitude==0?(Object)DBNull.Value: obj.VehicleImageLottitude);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleImageLongitude", obj.VehicleImageLongitude == null || obj.VehicleImageLongitude == 0 ? (Object)DBNull.Value : obj.VehicleImageLongitude);


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
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_SaveGovernmentVehicleCheckIn", "GovernmentVehicleCheckIn");
                throw;

            }
            return resultmsg;
        }
        public CustomerVehiclePass GetNFCCardVehiclePassDetails(string NFCCardNumber)
        {
            CustomerVehiclePass objCustomerVehiclePass = new CustomerVehiclePass();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetNFCCardVehiclePassDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@NFCCardNumber", NFCCardNumber);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            objCustomerVehiclePass.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                            objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objCustomerVehiclePass.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objCustomerVehiclePass.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objCustomerVehiclePass.StartDate = resultdt.Rows[0]["StartDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["StartDate"]);
                            objCustomerVehiclePass.ExpiryDate = resultdt.Rows[0]["ExpiryDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ExpiryDate"]);
                            objCustomerVehiclePass.IssuedCard = resultdt.Rows[0]["IssuedCard"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IssuedCard"]);
                            objCustomerVehiclePass.CardNumber = Convert.ToString(resultdt.Rows[0]["CardNumber"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_GetNFCCardVehiclePassDetails", "GetNFCCardVehiclePassDetails");
            }
            return objCustomerVehiclePass;
        }
        public string VerifyCustomerNFCCardExpiry(CustomerVehiclePass objNFCCustomer,int checkinLocationID)
        {
            string resultmsg = string.Empty;
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                if (Convert.ToDateTime(objNFCCustomer.ExpiryDate).Date >= indianTime.Date)
                {
                    if (!objNFCCustomer.IsMultiLot)
                    {
                        if(objNFCCustomer.LocationID.LocationID==checkinLocationID)
                        {
                            resultmsg = "Valid";
                        }
                        else
                        {
                            resultmsg = "Invalid";
                        }

                    }
                    else
                    {
                        resultmsg = "Valid";
                    }
                        
                }
                else
                {
                    resultmsg = "Invalid";
                }

            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_SaveGovernmentVehicleCheckIn", "GovernmentVehicleCheckIn");
                throw;

            }
            return resultmsg;
        }
        public CustomerParkingSlot SaveVehicleNewCheckIn(VehicleCheckIn obj)
        {
            CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveVehicleNewCheckIn", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerParkingSlotID", (obj.CustomerParkingSlotID == 0) ? (object)DBNull.Value : obj.CustomerParkingSlotID);
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", obj.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", obj.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", obj.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@BayNumberID", obj.BayNumberID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", obj.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", obj.RegistrationNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingStartTime", obj.ParkingStartTime);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingEndTime", obj.ParkingEndTime);
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingHours", obj.ParkingHours);
                        sqlcmd_obj.Parameters.AddWithValue("@ParkingFees", obj.ParkingFees);
                        sqlcmd_obj.Parameters.AddWithValue("@ClampFees", (obj.ClampFees == null || obj.ClampFees == 0) ? (object)DBNull.Value : obj.ClampFees);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[0]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerParkingSlotID"]);
                            objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomerParkingSlot.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[0]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ParkingBayID"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[0]["ParkingBayName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[0]["ParkingBayRange"]);
                            objCustomerParkingSlot.ExpectedStartTime = Convert.ToDateTime(resultdt.Rows[0]["ExpectedStartTime"]);
                            objCustomerParkingSlot.ExpectedEndTime = Convert.ToDateTime(resultdt.Rows[0]["ExpectedEndTime"]);
                            objCustomerParkingSlot.ActualStartTime = Convert.ToDateTime(resultdt.Rows[0]["ActualStartTime"]);
                            objCustomerParkingSlot.ActualEndTime = Convert.ToDateTime(resultdt.Rows[0]["ActualEndTime"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = resultdt.Rows[0]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objCustomerParkingSlot.Duration = Convert.ToString(resultdt.Rows[0]["Duration"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeID = Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);
                            objCustomerParkingSlot.CreatedBy = resultdt.Rows[0]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["UserID"]);
                            objCustomerParkingSlot.UserCode = resultdt.Rows[0]["USERCODE"] == DBNull.Value ?"": Convert.ToString(resultdt.Rows[0]["USERCODE"]); 
                            objCustomerParkingSlot.CreatedByName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                            objCustomerParkingSlot.Amount = resultdt.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                            objCustomerParkingSlot.ClampFees = resultdt.Rows[0]["ClampFee"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ClampFee"]);
                            objCustomerParkingSlot.PaidAmount = resultdt.Rows[0]["PaidAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["PaidAmount"]);
                            objCustomerParkingSlot.ExtendAmount = resultdt.Rows[0]["ExtendAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["ExtendAmount"]);
                            objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultdt.Rows[0]["CustomerVehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            objCustomerParkingSlot.IsClamp = resultdt.Rows[0]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IsClamp"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_SaveVehicleNewCheckIn", "SaveVehicleNewCheckIn");
            }
            return objCustomerParkingSlot;

        }
        public CustomerParkingSlot VerifyVehicleInCheckInStatus(VehicleCheckIn objVehicle)
        {
            CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_VerifyVehicleInCheckInStatus", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", objVehicle.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", objVehicle.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", objVehicle.VehicleTypeCode);
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", objVehicle.RegistrationNumber == "" ? (object)DBNull.Value : objVehicle.RegistrationNumber);
                       
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {

                            objCustomerParkingSlot.CustomerParkingSlotID = resultdt.Rows[0]["CustomerParkingSlotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerParkingSlotID"]);
                            objCustomerParkingSlot.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objCustomerParkingSlot.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayID = resultdt.Rows[0]["ParkingBayID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["ParkingBayID"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayName = Convert.ToString(resultdt.Rows[0]["ParkingBayName"]);
                            objCustomerParkingSlot.LocationParkingLotID.ParkingBayID.ParkingBayRange = Convert.ToString(resultdt.Rows[0]["ParkingBayRange"]);
                            objCustomerParkingSlot.ExpectedStartTime = Convert.ToDateTime(resultdt.Rows[0]["ExpectedStartTime"]);
                            objCustomerParkingSlot.ExpectedEndTime = Convert.ToDateTime(resultdt.Rows[0]["ExpectedEndTime"]);
                            objCustomerParkingSlot.ActualStartTime = Convert.ToDateTime(resultdt.Rows[0]["ActualStartTime"]);
                            objCustomerParkingSlot.ActualEndTime = Convert.ToDateTime(resultdt.Rows[0]["ActualEndTime"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);
                            objCustomerParkingSlot.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objCustomerParkingSlot.Duration = Convert.ToString(resultdt.Rows[0]["Duration"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeID = Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                            objCustomerParkingSlot.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);
                            objCustomerParkingSlot.CreatedBy = Convert.ToInt32(resultdt.Rows[0]["UserID"]);
                            objCustomerParkingSlot.CreatedByName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                            objCustomerParkingSlot.Amount = Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                            objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);
                            objCustomerParkingSlot.IsClamp = resultdt.Rows[0]["IsClamp"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IsClamp"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALCheckIn", "Proc: " + "OPAPP_PROC_VerifyVehicleInCheckInStatus", "VerifyVehicleInCheckInStatus");
            }
            return objCustomerParkingSlot;

        }

    }
}