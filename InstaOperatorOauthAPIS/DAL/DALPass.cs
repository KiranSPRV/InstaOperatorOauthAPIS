using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.Pass;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALPass
    {
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        public List<PassPrice> GetPassPriceDetails(VehicleLotPassPrice objVehicleType)
        {
            List<PassPrice> lstPassPrice = null;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetPassPriceDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (objVehicleType.VehicleTypeCode == "" || objVehicleType.VehicleTypeCode == null) ? (object)DBNull.Value : Convert.ToString(objVehicleType.VehicleTypeCode));
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objVehicleType.LocationParkingLotID == 0 || objVehicleType.LocationParkingLotID == null) ? (object)DBNull.Value : Convert.ToInt32(objVehicleType.LocationParkingLotID));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            lstPassPrice = new List<PassPrice>();
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                PassPrice objPassPrice = new PassPrice();
                                objPassPrice.PassPriceID = resultdt.Rows[i]["PassPriceID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["PassPriceID"]);
                                objPassPrice.PassCode = resultdt.Rows[i]["PassCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["PassCode"]);
                                objPassPrice.StationAccess = resultdt.Rows[i]["StationAccess"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["StationAccess"]);
                                objPassPrice.StartDate = resultdt.Rows[i]["StartDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["StartDate"]);
                                objPassPrice.EndDate = resultdt.Rows[i]["EndDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["EndDate"]);
                                objPassPrice.Duration = resultdt.Rows[i]["Duration"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["Duration"]);
                                objPassPrice.Price = resultdt.Rows[i]["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["Price"]);
                                objPassPrice.NFCCardPrice = resultdt.Rows[i]["NFCCardPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["NFCCardPrice"]);
                                objPassPrice.NFCApplicable = resultdt.Rows[i]["NFCApplicable"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["NFCApplicable"]);
                                objPassPrice.VehicleTypeID.VehicleTypeID = resultdt.Rows[i]["VehicleTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objPassPrice.VehicleTypeID.VehicleTypeName = resultdt.Rows[i]["VehicleTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                objPassPrice.VehicleTypeID.VehicleTypeCode = resultdt.Rows[i]["VehicleTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);
                                objPassPrice.PassTypeID.PassTypeID = resultdt.Rows[i]["PassTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["PassTypeID"]);
                                objPassPrice.PassTypeID.PassTypeName = resultdt.Rows[i]["PassTypeName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["PassTypeName"]);
                                objPassPrice.PassTypeID.PassTypeCode = resultdt.Rows[i]["PassTypeCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["PassTypeCode"]);
                                lstPassPrice.Add(objPassPrice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetPassPriceDetails", "GetPassPriceDetails");
            }
            return lstPassPrice;
        }
        public CustomerVehiclePass SaveCustomerVehiclePass(CustomerVehiclePass objCustomerVehiclePass)
        {
            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveCustomerVehiclePass", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehiclePassID", ((objCustomerVehiclePass.CustomerVehiclePassID == null || objCustomerVehiclePass.CustomerVehiclePassID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehiclePassID)));
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", ((objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID == null || objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID)));
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerID", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID)));
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", ((objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == null || objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@PhoneNumber", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@Name", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassCardTypeMapperID", ((objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID == null || objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID)));
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", ((objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == null || objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode)));
                        sqlcmd_obj.Parameters.AddWithValue("@PrimaryLocationParkingLotID", ((objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID == null || objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID)));
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", ((objCustomerVehiclePass.LocationID.LocationID == null || objCustomerVehiclePass.LocationID.LocationID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.LocationID.LocationID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassPriceID", ((objCustomerVehiclePass.PassPriceID.PassPriceID == null || objCustomerVehiclePass.PassPriceID.PassPriceID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassPriceID.PassPriceID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeID", ((objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == null || objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeCode", ((objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode == null || objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode)));
                        sqlcmd_obj.Parameters.AddWithValue("@Duration", ((objCustomerVehiclePass.PassPriceID.Duration == null || objCustomerVehiclePass.PassPriceID.Duration == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.PassPriceID.Duration)));
                        sqlcmd_obj.Parameters.AddWithValue("@IsMultiLot", ((objCustomerVehiclePass.IsMultiLot == null) ? false : Convert.ToBoolean(objCustomerVehiclePass.IsMultiLot)));
                        sqlcmd_obj.Parameters.AddWithValue("@StartDate", ((objCustomerVehiclePass.StartDate == null) ? (object)DBNull.Value : objCustomerVehiclePass.StartDate));
                        sqlcmd_obj.Parameters.AddWithValue("@ExpiryDate", (objCustomerVehiclePass.ExpiryDate == null ? (object)DBNull.Value : objCustomerVehiclePass.ExpiryDate));
                        sqlcmd_obj.Parameters.AddWithValue("@IssuedCard", (objCustomerVehiclePass.IssuedCard == null ? (object)DBNull.Value : objCustomerVehiclePass.IssuedCard));
                        sqlcmd_obj.Parameters.AddWithValue("@CardNumber", (objCustomerVehiclePass.CardNumber == null ? (object)DBNull.Value : objCustomerVehiclePass.CardNumber));
                        sqlcmd_obj.Parameters.AddWithValue("@Amount", (objCustomerVehiclePass.Amount == null ? (object)DBNull.Value : objCustomerVehiclePass.Amount));
                        sqlcmd_obj.Parameters.AddWithValue("@CardAmount", (objCustomerVehiclePass.CardAmount == null ? (object)DBNull.Value : objCustomerVehiclePass.CardAmount));
                        sqlcmd_obj.Parameters.AddWithValue("@TotalAmount", (objCustomerVehiclePass.TotalAmount == null ? (object)DBNull.Value : objCustomerVehiclePass.TotalAmount));
                        sqlcmd_obj.Parameters.AddWithValue("@TransactionID", (objCustomerVehiclePass.TransactionID == null ? (object)DBNull.Value : objCustomerVehiclePass.TransactionID));
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentTypeID", (objCustomerVehiclePass.PaymentTypeID.PaymentTypeID == null ? (object)DBNull.Value : objCustomerVehiclePass.PaymentTypeID.PaymentTypeID));
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentTypeCode", (objCustomerVehiclePass.PaymentTypeID.PaymentTypeCode == null ? (object)DBNull.Value : objCustomerVehiclePass.PaymentTypeID.PaymentTypeCode));
                        sqlcmd_obj.Parameters.AddWithValue("@BarCode", (objCustomerVehiclePass.BarCode == null ? (object)DBNull.Value : objCustomerVehiclePass.BarCode));
                        sqlcmd_obj.Parameters.AddWithValue("@StatusID", ((objCustomerVehiclePass.StatusID == null || objCustomerVehiclePass.StatusID == 0) ? (object)DBNull.Value : objCustomerVehiclePass.StatusID));
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", ((objCustomerVehiclePass.CreatedBy.UserID == null || objCustomerVehiclePass.CreatedBy.UserID == 0) ? (object)DBNull.Value : objCustomerVehiclePass.CreatedBy.UserID));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                                objResultVehicle.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);

                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);

                                objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = resultdt.Rows[0]["CustomerID"] == DBNull.Value?0: Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                                objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                                objResultVehicle.CustomerVehiclePassID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);

                                objResultVehicle.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                                objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                                objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                                objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);

                                objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                                objResultVehicle.PassPriceID.PassTypeID.PassTypeID = Convert.ToInt32(resultdt.Rows[0]["PassTypeID"]);
                                objResultVehicle.PassPriceID.PassTypeID.PassTypeCode = Convert.ToString(resultdt.Rows[0]["PassTypeCode"]);
                                objResultVehicle.PassPriceID.PassTypeID.PassTypeName = Convert.ToString(resultdt.Rows[0]["PassTypeName"]);
                                objResultVehicle.StartDate = resultdt.Rows[0]["StartDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["StartDate"]);
                                objResultVehicle.ExpiryDate = resultdt.Rows[0]["ExpiryDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[0]["ExpiryDate"]);
                                objResultVehicle.Amount = resultdt.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["Amount"]);
                                objResultVehicle.TotalAmount = resultdt.Rows[0]["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[0]["TotalAmount"]);

                                objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[0]["StationAccess"]);
                                objResultVehicle.CreatedBy.UserName = Convert.ToString(resultdt.Rows[0]["UserName"]);
                                objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);
                                objResultVehicle.IssuedCard = resultdt.Rows[0]["IssuedCard"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[0]["IssuedCard"]);
                                objResultVehicle.PaymentTypeID.PaymentTypeID = resultdt.Rows[0]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["PaymentTypeID"]);
                                objResultVehicle.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[0]["PaymentTypeName"]);
                                objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_SaveCustomerVehiclePass", "SaveCustomerVehiclePass");
            }
            return objResultVehicle;
        }
        public int SaveCustomerMultiVehiclePass(CustomerVehiclePass objCustomerVehiclePass)
        {
            int customerVehiclePassID = 0;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveCustomerMultiLotVehiclePass", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehiclePassID", ((objCustomerVehiclePass.CustomerVehiclePassID == null || objCustomerVehiclePass.CustomerVehiclePassID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehiclePassID)));
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", ((objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID == null || objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehicleID.CustomerVehicleID)));
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerID", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.CustomerVehicleID.CustomerID.CustomerID)));
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", ((objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == null || objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@PhoneNumber", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.CustomerID.PhoneNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@Name", ((objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name == null || objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.CustomerID.Name)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassCardTypeMapperID", ((objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID == null || objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassCardTypeMapperID.PassCardTypeMapperID)));
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", ((objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == null || objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode)));
                        sqlcmd_obj.Parameters.AddWithValue("@PrimaryLocationParkingLotID", ((objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID == null || objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PrimaryLocationParkingLotID.LocationParkingLotID)));
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", ((objCustomerVehiclePass.LocationID.LocationID == null || objCustomerVehiclePass.LocationID.LocationID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.LocationID.LocationID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassPriceID", ((objCustomerVehiclePass.PassPriceID.PassPriceID == null || objCustomerVehiclePass.PassPriceID.PassPriceID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassPriceID.PassPriceID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeID", ((objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == null || objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeCode", ((objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode == null || objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeCode)));
                        sqlcmd_obj.Parameters.AddWithValue("@Duration", ((objCustomerVehiclePass.PassPriceID.Duration == null || objCustomerVehiclePass.PassPriceID.Duration == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.PassPriceID.Duration)));
                        sqlcmd_obj.Parameters.AddWithValue("@IsMultiLot", ((objCustomerVehiclePass.IsMultiLot == null) ? false : Convert.ToBoolean(objCustomerVehiclePass.IsMultiLot)));
                        sqlcmd_obj.Parameters.AddWithValue("@StartDate", ((objCustomerVehiclePass.StartDate == null) ? (object)DBNull.Value : objCustomerVehiclePass.StartDate));
                        sqlcmd_obj.Parameters.AddWithValue("@ExpiryDate", (objCustomerVehiclePass.ExpiryDate == null ? (object)DBNull.Value : objCustomerVehiclePass.ExpiryDate));
                        sqlcmd_obj.Parameters.AddWithValue("@IssuedCard", (objCustomerVehiclePass.IssuedCard == null ? false : objCustomerVehiclePass.IssuedCard));
                        sqlcmd_obj.Parameters.AddWithValue("@CardNumber", (objCustomerVehiclePass.CardNumber == null ? (object)DBNull.Value : objCustomerVehiclePass.CardNumber));
                        sqlcmd_obj.Parameters.AddWithValue("@Amount", (objCustomerVehiclePass.Amount == null ? (object)DBNull.Value : objCustomerVehiclePass.Amount));
                        sqlcmd_obj.Parameters.AddWithValue("@CardAmount", (objCustomerVehiclePass.CardAmount == null ? (object)DBNull.Value : objCustomerVehiclePass.CardAmount));
                        sqlcmd_obj.Parameters.AddWithValue("@TotalAmount", (objCustomerVehiclePass.TotalAmount == null ? (object)DBNull.Value : objCustomerVehiclePass.TotalAmount));
                        sqlcmd_obj.Parameters.AddWithValue("@TransactionID", (objCustomerVehiclePass.TransactionID == null ? (object)DBNull.Value : objCustomerVehiclePass.TransactionID));
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentTypeID", (objCustomerVehiclePass.PaymentTypeID.PaymentTypeID == null ? (object)DBNull.Value : objCustomerVehiclePass.PaymentTypeID.PaymentTypeID));
                        sqlcmd_obj.Parameters.AddWithValue("@PaymentTypeCode", (objCustomerVehiclePass.PaymentTypeID.PaymentTypeCode == null ? (object)DBNull.Value : objCustomerVehiclePass.PaymentTypeID.PaymentTypeCode));
                        sqlcmd_obj.Parameters.AddWithValue("@BarCode", (objCustomerVehiclePass.BarCode == null ? (object)DBNull.Value : objCustomerVehiclePass.BarCode));
                        sqlcmd_obj.Parameters.AddWithValue("@StatusID", ((objCustomerVehiclePass.StatusID == null || objCustomerVehiclePass.StatusID == 0) ? (object)DBNull.Value : objCustomerVehiclePass.StatusID));
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", ((objCustomerVehiclePass.CreatedBy.UserID == null || objCustomerVehiclePass.CreatedBy.UserID == 0) ? (object)DBNull.Value : objCustomerVehiclePass.CreatedBy.UserID));
                        sqlconn_obj.Open();
                        sqlcmd_obj.Parameters.Add("@OutVehiclePassID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlcmd_obj.ExecuteNonQuery();
                        customerVehiclePassID = Convert.ToInt32(sqlcmd_obj.Parameters["@OutVehiclePassID"].Value);

                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_SaveCustomerMultiLotVehiclePass", "SaveCustomerMultiVehiclePass");
            }
            return customerVehiclePassID;
        }

        public List<Location> GetAllLocation()
        {
            List<Location> lstLocation = new List<Location>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllLocations", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                Location objLocation = new Location();
                                objLocation.LocationID = resultdt.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objLocation.LocationName = resultdt.Rows[i]["LocationName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objLocation.LocationCode = resultdt.Rows[i]["LocationCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationCode"]);
                                lstLocation.Add(objLocation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetAllLocations", "GetAllLocation");
            }
            return lstLocation;
        }
        public List<Location> GetAllLocationByVehicleType(string VehicleTypeCode)
        {
            List<Location> lstLocation = new List<Location>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllLocationsByVehicleType", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (VehicleTypeCode=="" ? (object)DBNull.Value : VehicleTypeCode));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                Location objLocation = new Location();
                                objLocation.LocationID = resultdt.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objLocation.LocationName = resultdt.Rows[i]["LocationName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objLocation.LocationCode = resultdt.Rows[i]["LocationCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationCode"]);
                                lstLocation.Add(objLocation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetAllLocationsByVehicleType", "GetAllLocationByVehicleType");
            }
            return lstLocation;
        }
        public List<Location> GetAllPassLocationByVehicleType(string VehicleTypeCode,string CustomerVehiclePassId)
        {
            List<Location> lstLocation = new List<Location>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetAllPassLocationsByVehicleType", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", (VehicleTypeCode == "" ? (object)DBNull.Value : VehicleTypeCode));
                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehiclePassId", (CustomerVehiclePassId == "" ? (object)DBNull.Value :Convert.ToInt32( CustomerVehiclePassId)));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                Location objLocation = new Location();
                                objLocation.LocationID = resultdt.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objLocation.LocationName = resultdt.Rows[i]["LocationName"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objLocation.LocationCode = resultdt.Rows[i]["LocationCode"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LocationCode"]);
                                lstLocation.Add(objLocation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetAllPassLocationsByVehicleType", "GetAllPassLocationByVehicleType");
            }
            return lstLocation;
        }
        public bool ValidateCustomerVehiclePass(CustomerVehiclePass objCustomerVehiclePass)
        {
            bool IsCustomerhasDayPass = false;
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_ValidateCustomerVehiclePass", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", ((objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == null || objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.RegistrationNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@VehicleTypeCode", ((objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == null || objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode)));
                        sqlcmd_obj.Parameters.AddWithValue("@LocatinID", ((objCustomerVehiclePass.LocationID.LocationID == null || objCustomerVehiclePass.LocationID.LocationID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehiclePass.LocationID.LocationID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeID", ((objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == null || objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID == 0) ? (object)DBNull.Value : Convert.ToString(objCustomerVehiclePass.PassPriceID.PassTypeID.PassTypeID)));
                        sqlcmd_obj.Parameters.AddWithValue("@StartDate", ((objCustomerVehiclePass.StartDate == null) ? (object)DBNull.Value : objCustomerVehiclePass.StartDate));
                        sqlcmd_obj.Parameters.AddWithValue("@EndDate", ((objCustomerVehiclePass.ExpiryDate == null) ? (object)DBNull.Value : objCustomerVehiclePass.ExpiryDate));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            IsCustomerhasDayPass = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_ValidateCustomerVehicleDayPass", "ValidateCustomerVehicleDayPass");
            }
            return IsCustomerhasDayPass;
        }
        public List<CustomerVehiclePass> GetCustomerVehiclePassesByVehicle(string RegistrationNumber)
        {
            List<CustomerVehiclePass> lstResultVehicle = new List<CustomerVehiclePass>();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetPassVehicleDetailsByVehicle", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", (RegistrationNumber));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();

                                objResultVehicle.CustomerVehiclePassID = resultdt.Rows[i]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["CustomerVehiclePassID"]);
                                objResultVehicle.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[i]["CustomerVehicleID"]);

                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[i]["VehicleTypeID"]);
                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[i]["VehicleTypeName"]);
                                objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[i]["VehicleTypeCode"]);

                                objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = Convert.ToInt32(resultdt.Rows[i]["CustomerID"]);
                                objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[i]["Name"]);
                                objResultVehicle.CustomerVehicleID.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[i]["PhoneNumber"]);
                                objResultVehicle.CustomerVehiclePassID = Convert.ToInt32(resultdt.Rows[i]["CustomerVehiclePassID"]);

                                objResultVehicle.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[i]["LocationParkingLotName"]);
                                objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[i]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]);

                                objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[i]["RegistrationNumber"]);
                                objResultVehicle.PassPriceID.PassPriceID = Convert.ToInt32(resultdt.Rows[i]["PassPriceID"]);
                                objResultVehicle.PassPriceID.NFCCardPrice = resultdt.Rows[i]["NFCCardPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[i]["NFCCardPrice"]);
                                objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[i]["StationAccess"]);
                                objResultVehicle.PassPriceID.Price = resultdt.Rows[i]["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[i]["Price"]);
                                objResultVehicle.IsMultiLot = resultdt.Rows[i]["IsMultiLot"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsMultiLot"]);

                                objResultVehicle.PassPriceID.PassTypeID.PassTypeID = Convert.ToInt32(resultdt.Rows[i]["PassTypeID"]);
                                objResultVehicle.PassPriceID.PassTypeID.PassTypeCode = Convert.ToString(resultdt.Rows[i]["PassTypeCode"]);
                                objResultVehicle.PassPriceID.PassTypeID.PassTypeName = Convert.ToString(resultdt.Rows[i]["PassTypeName"]);
                                objResultVehicle.StartDate = resultdt.Rows[i]["StartDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["StartDate"]);
                                objResultVehicle.ExpiryDate = resultdt.Rows[i]["ExpiryDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(resultdt.Rows[i]["ExpiryDate"]);
                                objResultVehicle.Amount = resultdt.Rows[i]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["Amount"]);
                                objResultVehicle.CardAmount = resultdt.Rows[i]["CardAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["CardAmount"]);
                                objResultVehicle.TotalAmount = resultdt.Rows[i]["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(resultdt.Rows[i]["TotalAmount"]);

                                objResultVehicle.CreatedBy.UserName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[i]["UserCode"]);
                                objResultVehicle.IssuedCard = resultdt.Rows[i]["IssuedCard"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IssuedCard"]);
                                objResultVehicle.CardNumber = Convert.ToString(resultdt.Rows[i]["CardNumber"]);
                                objResultVehicle.PaymentTypeID.PaymentTypeID = resultdt.Rows[i]["PaymentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["PaymentTypeID"]);
                                objResultVehicle.PaymentTypeID.PaymentTypeName = Convert.ToString(resultdt.Rows[i]["PaymentTypeName"]);
                                objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[i]["UserCode"]);

                                lstResultVehicle.Add(objResultVehicle);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetPassVehicleDetailsByVehicle", "GetCustomerVehicleDetailsByVehicle");
            }
            return lstResultVehicle;
        }
        public CustomerVehiclePass GetCustomerVehicleDetailsByVehicle(CustomerVehicle objCustomerVehicle)
        {
            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetPassVehicleDetailsByVehicle", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;

                        // sqlcmd_obj.Parameters.AddWithValue("@CustomerVehicleID", ((objCustomerVehicle.CustomerVehicleID == null || objCustomerVehicle.CustomerVehicleID == 0) ? (object)DBNull.Value : Convert.ToInt32(objCustomerVehicle.CustomerVehicleID)));
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", ((objCustomerVehicle.RegistrationNumber == null || objCustomerVehicle.RegistrationNumber == "") ? (object)DBNull.Value : Convert.ToString(objCustomerVehicle.RegistrationNumber)));

                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                            objResultVehicle.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);

                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);

                            objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objResultVehicle.CustomerVehiclePassID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);

                            objResultVehicle.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);

                            objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objResultVehicle.PassPriceID.PassPriceID = Convert.ToInt32(resultdt.Rows[0]["PassPriceID"]);
                            objResultVehicle.PassPriceID.NFCCardPrice = resultdt.Rows[0]["NFCCardPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["NFCCardPrice"]);
                            objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[0]["StationAccess"]);
                            objResultVehicle.PassPriceID.Price = resultdt.Rows[0]["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["Price"]);

                            objResultVehicle.PassPriceID.PassTypeID.PassTypeID = Convert.ToInt32(resultdt.Rows[0]["PassTypeID"]);
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
                            objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_GetPassVehicleDetailsByVehicle", "GetCustomerVehicleDetailsByVehicle");
            }
            return objResultVehicle;
        }
        public CustomerVehiclePass ActivateCustomerVehiclePass(CustomerVehiclePass objPass)
        {
            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_ActivateCustomerVehiclePass", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;


                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehiclePassID", ((objPass.CustomerVehiclePassID == null || objPass.CustomerVehiclePassID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.CustomerVehiclePassID)));
                        sqlcmd_obj.Parameters.AddWithValue("@IssuedCard", ((objPass.IssuedCard == null) ? false : Convert.ToBoolean(objPass.IssuedCard)));
                        sqlcmd_obj.Parameters.AddWithValue("@CardNumber", ((objPass.CardNumber == null || objPass.CardNumber == "") ? (object)DBNull.Value : Convert.ToString(objPass.CardNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@Amount", ((objPass.Amount == null || objPass.Amount == 0) ? (object)DBNull.Value : Convert.ToDecimal(objPass.Amount)));
                        sqlcmd_obj.Parameters.AddWithValue("@CardAmount", ((objPass.CardAmount == null || objPass.CardAmount == 0) ? (object)DBNull.Value : Convert.ToDecimal(objPass.CardAmount)));
                        sqlcmd_obj.Parameters.AddWithValue("@TotalAmount", ((objPass.TotalAmount == null || objPass.TotalAmount == 0) ? (object)DBNull.Value : Convert.ToDecimal(objPass.TotalAmount)));
                        sqlcmd_obj.Parameters.AddWithValue("@BarCode", (objPass.BarCode == null ? (object)DBNull.Value : objPass.BarCode));
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", ((objPass.CreatedBy.UserID == null || objPass.CreatedBy.UserID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.CreatedBy.UserID)));

                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                            objResultVehicle.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);

                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);

                            objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objResultVehicle.CustomerVehiclePassID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);

                            objResultVehicle.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"]==DBNull.Value?0: Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);

                            objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objResultVehicle.PassPriceID.PassPriceID = Convert.ToInt32(resultdt.Rows[0]["PassPriceID"]);
                            objResultVehicle.PassPriceID.NFCCardPrice = resultdt.Rows[0]["NFCCardPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["NFCCardPrice"]);
                            objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[0]["StationAccess"]);
                            objResultVehicle.PassPriceID.Price = resultdt.Rows[0]["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["Price"]);

                            objResultVehicle.PassPriceID.PassTypeID.PassTypeID = Convert.ToInt32(resultdt.Rows[0]["PassTypeID"]);
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
                            objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_ActivateCustomerVehiclePass", "ActivateCustomerVehiclePass");
            }
            return objResultVehicle;
        }
        public CustomerVehiclePass GetCustomerVehiclePassDetails(CustomerVehiclePass objPass)
        {
            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            DataTable resultdt = new DataTable();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetCustomerVehiclePassDetails", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;


                        sqlcmd_obj.Parameters.AddWithValue("@CustomerVehiclePassID", ((objPass.CustomerVehiclePassID == null || objPass.CustomerVehiclePassID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.CustomerVehiclePassID)));
                        sqlcmd_obj.Parameters.AddWithValue("@RegistrationNumber", ((objPass.CustomerVehicleID.RegistrationNumber == null) ? (object)DBNull.Value : Convert.ToString(objPass.CustomerVehicleID.RegistrationNumber)));
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", ((objPass.LocationID.LocationID == null || objPass.LocationID.LocationID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.LocationID.LocationID)));
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", ((objPass.PrimaryLocationParkingLotID.LocationParkingLotID == null || objPass.PrimaryLocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.PrimaryLocationParkingLotID)));
                        sqlcmd_obj.Parameters.AddWithValue("@PassTypeID", ((objPass.PassPriceID.PassTypeID.PassTypeID == null || objPass.PassPriceID.PassTypeID.PassTypeID == 0) ? (object)DBNull.Value : Convert.ToInt32(objPass.PassPriceID.PassTypeID.PassTypeID)));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objResultVehicle.CustomerVehiclePassID = resultdt.Rows[0]["CustomerVehiclePassID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);
                            objResultVehicle.CustomerVehicleID.CustomerVehicleID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehicleID"]);

                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeID = Convert.ToInt32(resultdt.Rows[0]["VehicleTypeID"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeName = Convert.ToString(resultdt.Rows[0]["VehicleTypeName"]);
                            objResultVehicle.CustomerVehicleID.VehicleTypeID.VehicleTypeCode = Convert.ToString(resultdt.Rows[0]["VehicleTypeCode"]);

                            objResultVehicle.CustomerVehicleID.CustomerID.CustomerID = Convert.ToInt32(resultdt.Rows[0]["CustomerID"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.Name = Convert.ToString(resultdt.Rows[0]["Name"]);
                            objResultVehicle.CustomerVehicleID.CustomerID.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objResultVehicle.CustomerVehiclePassID = Convert.ToInt32(resultdt.Rows[0]["CustomerVehiclePassID"]);

                            objResultVehicle.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objResultVehicle.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objResultVehicle.PrimaryLocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);

                            objResultVehicle.CustomerVehicleID.RegistrationNumber = Convert.ToString(resultdt.Rows[0]["RegistrationNumber"]);
                            objResultVehicle.PassPriceID.PassPriceID = Convert.ToInt32(resultdt.Rows[0]["PassPriceID"]);
                            objResultVehicle.PassPriceID.NFCCardPrice = resultdt.Rows[0]["NFCCardPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["NFCCardPrice"]);
                            objResultVehicle.PassPriceID.StationAccess = Convert.ToString(resultdt.Rows[0]["StationAccess"]);
                            objResultVehicle.PassPriceID.Price = resultdt.Rows[0]["Price"] == DBNull.Value ? 0m : Convert.ToDecimal(resultdt.Rows[0]["Price"]);

                            objResultVehicle.PassPriceID.PassTypeID.PassTypeID = Convert.ToInt32(resultdt.Rows[0]["PassTypeID"]);
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
                            objResultVehicle.CreatedBy.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALPass", "Proc: " + "OPAPP_PROC_ActivateCustomerVehiclePass", "ActivateCustomerVehiclePass");
            }
            return objResultVehicle;
        }

    }
}