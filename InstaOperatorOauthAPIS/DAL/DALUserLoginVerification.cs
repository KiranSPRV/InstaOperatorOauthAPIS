using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.FirebaseModel;
using InstaOperatorOauthAPIS.VMModels;
using ISTAOnlineWebAPI.DAL;
using ISTAOnlineWebAPI.DAL.EncryptDecrypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InstaOperatorOauthAPIS.DAL
{
    public class DALUserLoginVerification
    {
        DataEncryptDecrypt objencrypt;
        DALExceptionManagment objExceptionlog = new DALExceptionManagment();
        PasswordEncryptDecrypt objpwdencrypt;
        public User GetLoginUserDetails(UserLogin objUser)
        {

            User objOutPutuser = new User();
            objencrypt = new DataEncryptDecrypt();
            objpwdencrypt = new PasswordEncryptDecrypt();
            string resultMsg = string.Empty;
            string encryptKey = string.Empty;
            try
            {
                string resultkey = getEncryptdata(objUser.UserName);
                string pwd = objpwdencrypt.Encrypt(objUser.Password, resultkey);
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_LoginVerification", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@USERNAME", objUser.UserName);
                        sqlcmd_obj.Parameters.AddWithValue("@PASSWORD", pwd);
                        sqlcmd_obj.Parameters.AddWithValue("@Lattitude", objUser.Latitude);
                        sqlcmd_obj.Parameters.AddWithValue("@Longitude", objUser.Longitude);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            objOutPutuser.UserName = Convert.ToString(resultdt.Rows[0]["USERNAME"]);
                            objOutPutuser.UserCode = Convert.ToString(resultdt.Rows[0]["UserCode"]);
                            objOutPutuser.UserID = resultdt.Rows[0]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["UserID"]);
                            objOutPutuser.UserTypeID.UserTypeID = resultdt.Rows[0]["UserTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["UserTypeID"]);
                            objOutPutuser.LocationParkingLotID.LocationParkingLotID = resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationParkingLotID"]);
                            objOutPutuser.LocationParkingLotID.LocationParkingLotName = Convert.ToString(resultdt.Rows[0]["LocationParkingLotName"]);
                            objOutPutuser.LocationParkingLotID.LocationParkingLotCode = Convert.ToString(resultdt.Rows[0]["LocationParkingLotCode"]);
                            objOutPutuser.LocationParkingLotID.LocationID.LocationID = resultdt.Rows[0]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[0]["LocationID"]);
                            objOutPutuser.LocationParkingLotID.LocationID.LocationName = Convert.ToString(resultdt.Rows[0]["LocationName"]);
                            objOutPutuser.UserTypeID.UserTypeName = Convert.ToString(resultdt.Rows[0]["UserTypeName"]);
                            objOutPutuser.LocationParkingLotID.Lattitude = Convert.ToDecimal(objUser.Latitude);
                            objOutPutuser.LocationParkingLotID.Longitude = Convert.ToDecimal(objUser.Longitude);
                            objOutPutuser.PhoneNumber = Convert.ToString(resultdt.Rows[0]["PhoneNumber"]);
                            objOutPutuser.LoginTime = DateTime.Now;
                            objOutPutuser.LoginDeviceID = objUser.LoginDeviceID;
                            objOutPutuser.Photo = resultdt.Rows[0]["Photo"]==DBNull.Value ? null : (byte[])resultdt.Rows[0]["Photo"];
                            List<LocationParkingLot> lstLocationParkingLots = GetLoginUserAllocatedLocationLots(objOutPutuser);
                            if (lstLocationParkingLots.Count > 0)
                            {
                                LocationParkingLot userlot = lstLocationParkingLots[0];
                                objOutPutuser.LocationParkingLotID.LotCloseTime = userlot.LotCloseTime;
                                objOutPutuser.LocationParkingLotID.LotOpenTime = userlot.LotOpenTime;
                                if (resultdt.Rows[0]["LocationParkingLotID"] == DBNull.Value)
                                {
                                    objOutPutuser.LocationParkingLotID.LocationParkingLotID = userlot.LocationParkingLotID;
                                    objOutPutuser.LocationParkingLotID.LocationParkingLotName = userlot.LocationParkingLotName;
                                    objOutPutuser.LocationParkingLotID.LocationID.LocationID = userlot.LocationID.LocationID;
                                    objOutPutuser.LocationParkingLotID.LocationID.LocationName = userlot.LocationID.LocationName;
                                }
                                DALLocationLot dalLocation = new DALLocationLot();
                                var lotavilability = dalLocation.GetLocationLotVehicleAvailabilityDetails(userlot.LocationID.LocationID, userlot.LocationParkingLotID);
                                objOutPutuser.LocationParkingLotID.LotVehicleAvailabilityName = lotavilability.LotVehicleAvailabilityName;


                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_LoginVerification", "GetLoginUserDetails");
                throw;

            }
            return objOutPutuser;

        }
        public string GetUserDeviceLoginStatus(UserLogin objUser)
        {

            string resultmsg = string.Empty;
            objencrypt = new DataEncryptDecrypt();
            objpwdencrypt = new PasswordEncryptDecrypt();
            string resultMsg = string.Empty;
            string encryptKey = string.Empty;
            try
            {
                string resultkey = getEncryptdata(objUser.UserName);
                string pwd = objpwdencrypt.Encrypt(objUser.Password, resultkey);
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetUserDeviceLoginStatus", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserName", objUser.UserName);
                        sqlcmd_obj.Parameters.AddWithValue("@Password", pwd);
                        sqlcmd_obj.Parameters.AddWithValue("@LoginDeviceID", objUser.LoginDeviceID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            resultmsg = "Please check user already logged in another device";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetUserDeviceLoginStatus", "GetUserDeviceLoginStatus");
                throw;

            }
            return resultmsg;

        }
        public string UpateLoginUserPassword(User objUpdateUser)
        {

            string resultmsg = string.Empty;
            objencrypt = new DataEncryptDecrypt();
            objpwdencrypt = new PasswordEncryptDecrypt();
            string resultMsg = string.Empty;
            string encryptKey = string.Empty;
            try
            {
                string resultkey = getEncryptdata(objUpdateUser.UserCode);
                string pwd = objpwdencrypt.Encrypt(objUpdateUser.Password, resultkey); //objUpdateUser.Password;//
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UpateLoginUserPassword", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objUpdateUser.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@PASSWORD", pwd);
                        sqlcmd_obj.Parameters.AddWithValue("@UpdatedBy", objUpdateUser.UserID);
                        sqlconn_obj.Open();
                        int resultcount = sqlcmd_obj.ExecuteNonQuery();
                        if (resultcount > 0)
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
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_UpateLoginUserPassword", "UpateLoginUserPassword");
                throw;

            }
            return resultmsg;

        }
        public List<LocationParkingLot> GetLoginUserAllocatedLocationLots(User objLoginUser)
        {
            List<LocationParkingLot> lstLocationParkingLot = new List<LocationParkingLot>();
            string resultMsg = string.Empty;
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetUserAllocatedStationLots", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objLoginUser.UserID == null || objLoginUser.UserID == 0) ? (object)DBNull.Value : objLoginUser.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@UserTypeID", (objLoginUser.UserTypeID.UserTypeID == null || objLoginUser.UserTypeID.UserTypeID == 0) ? (object)DBNull.Value : objLoginUser.UserTypeID.UserTypeID);
                        sqlcmd_obj.Parameters.AddWithValue("@UserTypeName", (objLoginUser.UserTypeID.UserTypeName == null || objLoginUser.UserTypeID.UserTypeName == "null") ? (object)DBNull.Value : objLoginUser.UserTypeID.UserTypeName);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", (objLoginUser.LocationParkingLotID.LocationID.LocationID == null || objLoginUser.LocationParkingLotID.LocationID.LocationID == 0) ? (object)DBNull.Value : objLoginUser.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objLoginUser.LocationParkingLotID.LocationParkingLotID == null || objLoginUser.LocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : objLoginUser.LocationParkingLotID.LocationParkingLotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                LocationParkingLot objLocationParkingLot = new LocationParkingLot();
                                objLocationParkingLot.LocationParkingLotID = Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]);
                                objLocationParkingLot.LocationParkingLotName = Convert.ToString(resultdt.Rows[i]["LocationParkingLotName"]);
                                objLocationParkingLot.LocationID.LocationID = Convert.ToInt32(resultdt.Rows[i]["LocationID"]);
                                objLocationParkingLot.LocationID.LocationName = Convert.ToString(resultdt.Rows[i]["LocationName"]);
                                objLocationParkingLot.IsActive = resultdt.Rows[i]["IsHoliday"] == DBNull.Value ? false : Convert.ToBoolean(resultdt.Rows[i]["IsHoliday"]);
                                objLocationParkingLot.LotOpenTime = resultdt.Rows[i]["LotOpenTime"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LotOpenTime"]);
                                objLocationParkingLot.LotCloseTime = resultdt.Rows[i]["LOTCLOSETIME"] == DBNull.Value ? "" : Convert.ToString(resultdt.Rows[i]["LOTCLOSETIME"]);
                                DALLocationLot dalLocation = new DALLocationLot();
                                var lotavilability = dalLocation.GetLocationLotVehicleAvailabilityDetails(Convert.ToInt32(resultdt.Rows[i]["LocationID"]), Convert.ToInt32(resultdt.Rows[i]["LocationParkingLotID"]));
                                objLocationParkingLot.LotVehicleAvailabilityName = lotavilability.LotVehicleAvailabilityName;
                                lstLocationParkingLot.Add(objLocationParkingLot);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetUserAllocatedStationLots", "GetLoginUserAllocatedLocationLots");
            }
            return lstLocationParkingLot;

        }
        public string SaveUserLoginTimes(User objDailyLogin)
        {

            string resultMsg = string.Empty;
            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_SaveUserDailyLogin", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objDailyLogin.UserID == null || objDailyLogin.UserID == 0) ? (object)DBNull.Value : objDailyLogin.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objDailyLogin.LocationParkingLotID.LocationParkingLotID == null || objDailyLogin.LocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@Lattitude", (objDailyLogin.LocationParkingLotID.Lattitude == null || objDailyLogin.LocationParkingLotID.Lattitude == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.Lattitude);
                        sqlcmd_obj.Parameters.AddWithValue("@Longitude", (objDailyLogin.LocationParkingLotID.Longitude == null || objDailyLogin.LocationParkingLotID.Longitude == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.Longitude);
                        sqlcmd_obj.Parameters.AddWithValue("@LoginTime", (objDailyLogin.LoginTime == null ? (object)DBNull.Value : objDailyLogin.LoginTime));
                        sqlcmd_obj.Parameters.AddWithValue("@LogOutTime", (objDailyLogin.LogoutTime == null ? (object)DBNull.Value : objDailyLogin.LogoutTime));
                        sqlcmd_obj.Parameters.AddWithValue("@LoginDeviceID", (objDailyLogin.LoginDeviceID == null ? (object)DBNull.Value : objDailyLogin.LoginDeviceID));
                        sqlconn_obj.Open();
                        int resultvalue = sqlcmd_obj.ExecuteNonQuery();
                        if (resultvalue > 0)
                        {
                            resultMsg = "Success";

                        }
                        else
                        {
                            resultMsg = "Fail";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_SaveUserDailyLogin", "SaveUserLoginTimes");
            }
            return resultMsg;
        }
        public string UpdateUserLogOutTimes(User objDailyLogin)
        {

            string resultMsg = string.Empty;
            try
            {
                DateTime login = Convert.ToDateTime(objDailyLogin.LoginTime);
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_UserLogOut", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objDailyLogin.UserID == null || objDailyLogin.UserID == 0) ? (object)DBNull.Value : objDailyLogin.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objDailyLogin.LocationParkingLotID.LocationParkingLotID == null || objDailyLogin.LocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@Lattitude", (objDailyLogin.LocationParkingLotID.Lattitude == null || objDailyLogin.LocationParkingLotID.Lattitude == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.Lattitude);
                        sqlcmd_obj.Parameters.AddWithValue("@Longitude", (objDailyLogin.LocationParkingLotID.Longitude == null || objDailyLogin.LocationParkingLotID.Longitude == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.Longitude);
                        sqlcmd_obj.Parameters.AddWithValue("@LoginTime", (objDailyLogin.LoginTime == null ? (object)DBNull.Value : objDailyLogin.LoginTime));
                        sqlcmd_obj.Parameters.AddWithValue("@LogOutTime", (objDailyLogin.LogoutTime == null ? (object)DBNull.Value : objDailyLogin.LogoutTime));
                        sqlconn_obj.Open();
                        int resultvalue = sqlcmd_obj.ExecuteNonQuery();
                        if (resultvalue > 0)
                        {
                            resultMsg = "Success";
                        }
                        else
                        {
                            resultMsg = "Fail";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_SaveUserDailyLogin", "SaveUserLoginTimes");
            }
            return resultMsg;
        }
        public VMUserDailyLogin GetUserDailyLoginHistory(UserDailyLogin objDailyLogin)
        {
            VMUserDailyLogin vmuserlogin = new VMUserDailyLogin();
            DataSet dslogins = new DataSet();
            try
            {
                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetUserDailyLoginHistory", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objDailyLogin.UserID.UserID == null || objDailyLogin.UserID.UserID == 0) ? (object)DBNull.Value : objDailyLogin.UserID.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objDailyLogin.LocationParkingLotID.LocationParkingLotID == null || objDailyLogin.LocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : objDailyLogin.LocationParkingLotID.LocationParkingLotID);
                        sqlcmd_obj.Parameters.AddWithValue("@HistoryFromDate", (objDailyLogin.HistoryFromDate == null ? (object)DBNull.Value : objDailyLogin.HistoryFromDate));
                        sqlcmd_obj.Parameters.AddWithValue("@HistoryToDate", (objDailyLogin.HistoryToDate == null ? (object)DBNull.Value : objDailyLogin.HistoryToDate));
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        sqldap.Fill(dslogins);
                        if (dslogins.Tables.Count > 0)
                        {
                            DataTable dtUserLogins = new DataTable();
                            DataTable dtSummary = new DataTable();
                            dtUserLogins = dslogins.Tables[0];
                            dtSummary = dslogins.Tables[1];
                            List<UserDailyLogin> lstlogins = new List<UserDailyLogin>();
                            for (var i = 0; i < dtUserLogins.Rows.Count; i++)
                            {
                                UserDailyLogin objUserDailyLogin = new UserDailyLogin();
                                //objUserDailyLogin.UserID.UserID = dtUserLogins.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(dtUserLogins.Rows[i]["UserID"]);
                                //objUserDailyLogin.UserID.UserName = Convert.ToString(dtUserLogins.Rows[i]["UserName"]);
                                objUserDailyLogin.LocationParkingLotID.LocationParkingLotID = dtUserLogins.Rows[i]["LocationParkingLotID"] == DBNull.Value ? 0 : Convert.ToInt32(dtUserLogins.Rows[i]["LocationParkingLotID"]);
                                objUserDailyLogin.LocationParkingLotID.LocationParkingLotName = Convert.ToString(dtUserLogins.Rows[i]["LocationParkingLotName"]);
                                objUserDailyLogin.LocationParkingLotID.LocationID.LocationID = dtUserLogins.Rows[i]["LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(dtUserLogins.Rows[i]["LocationID"]);
                                objUserDailyLogin.LocationParkingLotID.LocationID.LocationName = Convert.ToString(dtUserLogins.Rows[i]["LocationName"]);
                                //objUserDailyLogin.HistoryFromDate = dtUserLogins.Rows[i]["LoginTime"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(dtUserLogins.Rows[i]["LoginTime"]);
                                objUserDailyLogin.TimeSheetDate = dtUserLogins.Rows[i]["TimeSheetDate"] == DBNull.Value ? (Nullable<DateTime>)null : Convert.ToDateTime(dtUserLogins.Rows[i]["TimeSheetDate"]);
                                objUserDailyLogin.LoginTime = dtUserLogins.Rows[i]["LoginTime"] == DBNull.Value ? "" : Convert.ToString(dtUserLogins.Rows[i]["LoginTime"]);
                                objUserDailyLogin.LogoutTime = dtUserLogins.Rows[i]["LogoutTime"] == DBNull.Value ? "" : Convert.ToString(dtUserLogins.Rows[i]["LogoutTime"]);
                                objUserDailyLogin.NoofHours = dtUserLogins.Rows[i]["NoofHours"] == DBNull.Value ? "0" : Convert.ToString(dtUserLogins.Rows[i]["NoofHours"]);
                                lstlogins.Add(objUserDailyLogin);

                            }

                            if (dtSummary.Rows.Count > 0)
                            {
                                vmuserlogin.WorkedDays = dtSummary.Rows[0]["WorkedDays"] == DBNull.Value ? 0 : Convert.ToInt32(dtSummary.Rows[0]["WorkedDays"]);
                                vmuserlogin.AbsentDays = dtSummary.Rows[0]["AbsentDays"] == DBNull.Value ? 0 : Convert.ToInt32(dtSummary.Rows[0]["AbsentDays"]);
                                vmuserlogin.TotalHours = dtSummary.Rows[0]["TotalHours"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["TotalHours"]);
                                vmuserlogin.SuperVisorName = dtSummary.Rows[0]["SuperVisorName"] == DBNull.Value ? "0" : Convert.ToString(dtSummary.Rows[0]["SuperVisorName"]);
                            }
                            vmuserlogin.UserDailyLoginID = lstlogins;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetUserDailyLoginHistory", "GetUserDailyLoginHistory");
            }
            return vmuserlogin;
        }
        public string getEncryptdata(string UserName)
        {
            string encryptkey = string.Empty;

            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetEncryptKey", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserCode", UserName == "" ? (object)DBNull.Value : UserName);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            encryptkey = Convert.ToString(resultdt.Rows[0]["EncryptedKey"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "PARK_PROC_GetEncryptKey", "getEncryptdata");
                throw;

            }
            return encryptkey;


        }

        public List<User> GetSuperVisorOperators(User objSupervisor)
        {

            List<User> lstOperator = new List<User>();

            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetSupervisorOperators", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", objSupervisor.UserID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                User objUser = new User();
                                objUser.UserName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objUser.UserCode = Convert.ToString(resultdt.Rows[i]["UserName"]) + "-" + Convert.ToString(resultdt.Rows[i]["UserCode"]);
                                objUser.UserID = resultdt.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserID"]);
                                objUser.UserTypeID.UserTypeID = resultdt.Rows[i]["UserTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserTypeID"]);
                                objUser.UserTypeID.UserTypeName = Convert.ToString(resultdt.Rows[i]["UserTypeName"]);
                                lstOperator.Add(objUser);
                            }


                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetSupervisorOperators", "GetSuperVisorOperators");
                throw;

            }
            return lstOperator;

        }
        public List<User> LocationLotActiveOperartors(LocationParkingLot objLocationParkingLot)
        {

            List<User> lstOperator = new List<User>();

            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetLocationLotActiveOperartors", sqlconn_obj))
                    {

                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;

                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", (objLocationParkingLot.LocationID.LocationID == null || objLocationParkingLot.LocationID.LocationID == 0) ? (object)DBNull.Value : objLocationParkingLot.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objLocationParkingLot.LocationParkingLotID == null || objLocationParkingLot.LocationParkingLotID == 0) ? (object)DBNull.Value : objLocationParkingLot.LocationParkingLotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                User objUser = new User();
                                objUser.UserName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objUser.UserCode = Convert.ToString(resultdt.Rows[i]["UserName"]) + "-" + Convert.ToString(resultdt.Rows[i]["UserCode"]);
                                objUser.UserID = resultdt.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserID"]);
                                objUser.UserTypeID.UserTypeID = resultdt.Rows[i]["UserTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserTypeID"]);
                                objUser.UserTypeID.UserTypeName = Convert.ToString(resultdt.Rows[i]["UserTypeName"]);
                                lstOperator.Add(objUser);
                            }


                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetSupervisorOperators", "GetSuperVisorOperators");
                throw;

            }
            return lstOperator;

        }
        public List<User> LocationLotActiveOperartors(User objUserLocationParkingLot)
        {

            List<User> lstOperator = new List<User>();

            try
            {

                using (SqlConnection sqlconn_obj = new SqlConnection(SqlHelper.GetDBConnectionString()))
                {
                    using (SqlCommand sqlcmd_obj = new SqlCommand("OPAPP_PROC_GetLocationLotActiveOperartors", sqlconn_obj))
                    {
                        sqlcmd_obj.CommandType = CommandType.StoredProcedure;
                        sqlcmd_obj.Parameters.AddWithValue("@UserID", (objUserLocationParkingLot.UserID == null || objUserLocationParkingLot.UserID == 0) ? (object)DBNull.Value : objUserLocationParkingLot.UserID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationID", (objUserLocationParkingLot.LocationParkingLotID.LocationID.LocationID == null || objUserLocationParkingLot.LocationParkingLotID.LocationID.LocationID == 0) ? (object)DBNull.Value : objUserLocationParkingLot.LocationParkingLotID.LocationID.LocationID);
                        sqlcmd_obj.Parameters.AddWithValue("@LocationParkingLotID", (objUserLocationParkingLot.LocationParkingLotID.LocationParkingLotID == null || objUserLocationParkingLot.LocationParkingLotID.LocationParkingLotID == 0) ? (object)DBNull.Value : objUserLocationParkingLot.LocationParkingLotID.LocationParkingLotID);
                        sqlconn_obj.Open();
                        SqlDataAdapter sqldap = new SqlDataAdapter(sqlcmd_obj);
                        DataTable resultdt = new DataTable();
                        sqldap.Fill(resultdt);
                        if (resultdt.Rows.Count > 0)
                        {
                            for (var i = 0; i < resultdt.Rows.Count; i++)
                            {
                                User objUser = new User();
                                objUser.UserName = Convert.ToString(resultdt.Rows[i]["UserName"]);
                                objUser.UserCode = Convert.ToString(resultdt.Rows[i]["UserName"]) + "-" + Convert.ToString(resultdt.Rows[i]["UserCode"]);
                                objUser.UserID = resultdt.Rows[i]["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserID"]);
                                objUser.UserTypeID.UserTypeID = resultdt.Rows[i]["UserTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(resultdt.Rows[i]["UserTypeID"]);
                                objUser.UserTypeID.UserTypeName = Convert.ToString(resultdt.Rows[i]["UserTypeName"]);
                                lstOperator.Add(objUser);
                            }


                        }
                    }
                }
            }

            catch (Exception ex)
            {
                objExceptionlog.InsertException("WebAPI", ex.Message, "DALUserLoginVerification", "Proc: " + "OPAPP_PROC_GetSupervisorOperators", "GetSuperVisorOperators");
                throw;

            }
            return lstOperator;

        }


        

    }

}