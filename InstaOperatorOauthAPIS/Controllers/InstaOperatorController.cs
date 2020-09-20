using InstaOperatorOauthAPIS.DAL;
using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.APIResult;
using InstaOperatorOauthAPIS.Models.Pass;
using InstaOperatorOauthAPIS.VMModels;
using ISTAOnlineWebAPI.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace InstaOperatorOauthAPIS.Controllers
{
    [Authorize]
    public class InstaOperatorController : ApiController
    {
        APIResponse ObjAPIResponse;

        #region App Exception Log
        [HttpPost]
        [ActionName("postOPAPPExceptionLog")]
        public HttpResponseMessage postOPAPPExceptionLog(ExceptionLog obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALExceptionManagment dal_ExceptionManagment = new DALExceptionManagment();
                string resultmsgs = dal_ExceptionManagment.InsertException(obj.ApplicationType, obj.ExcepionMessage, obj.Module, obj.Procedure, obj.Method);
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = true;
                ObjAPIResponse.Message = resultmsgs;
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }
        #endregion

        #region Login Verification

        [HttpPost]
        [ActionName("postOPAPPLoginVerification")]
        public HttpResponseMessage getOPAPPLoginVerification(UserLogin obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                string loginmsg = dal_UserLoginVerification.GetUserDeviceLoginStatus(obj);
                if (loginmsg == string.Empty)
                {
                    User resultobj = dal_UserLoginVerification.GetLoginUserDetails(obj);
                    if (resultobj.UserID != 0)
                    {
                        ObjAPIResponse.Object = (object)resultobj;
                        ObjAPIResponse.Result = true;
                        ObjAPIResponse.Message = "Success";

                    }
                    else
                    {
                        loginmsg = "Invalid Credentials" ;
                        ObjAPIResponse.Object = null;
                        ObjAPIResponse.Result = false;
                        ObjAPIResponse.Message = "Invalid Credentials";
                    }
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = loginmsg;
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPVerifyUserDeviceLoginStatus")]
        public HttpResponseMessage postOPAPPVerifyUserDeviceLoginStatus(UserLogin obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();

                string loginmsg = dal_UserLoginVerification.GetUserDeviceLoginStatus(obj);
                if (loginmsg == string.Empty)
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "User already in login with another device";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPSaveUserDailyLogin")]
        public HttpResponseMessage postOPAPPSaveUserDailyLogin(User objDailyLogin)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                string resultmsg = dal_UserLoginVerification.SaveUserLoginTimes(objDailyLogin);
                if (resultmsg != string.Empty && resultmsg == "Success")
                {
                    ObjAPIResponse.Object = (object)resultmsg;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("updateUserPassword")]
        public HttpResponseMessage updateUserPassword(User objUpdateUser)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                string msg = dal_UserLoginVerification.UpateLoginUserPassword(objUpdateUser);
                if (msg == "Success")
                {

                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }




        [HttpPost]
        [ActionName("postOPAPPUpdateUserDailyLogOut")]
        public HttpResponseMessage postOPAPPUpdateUserDailyLogOut(User objDailyLogin)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                string resultmsg = dal_UserLoginVerification.UpdateUserLogOutTimes(objDailyLogin);
                if (resultmsg != string.Empty && resultmsg == "Success")
                {
                    ObjAPIResponse.Object = (object)resultmsg;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPLocationLotActiveOperartor")]
        public HttpResponseMessage postOPAPPLocationLotActiveOperartors(LocationParkingLot objLocationParkingLot)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                List<User> resultobj = dal_UserLoginVerification.LocationLotActiveOperartors(objLocationParkingLot);
                if (resultobj.Count > 0)
                {
                    ObjAPIResponse.Object = (object)resultobj;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPUserDailyLoginHistory")]
        public HttpResponseMessage postOPAPPUserDailyLoginHistory(UserDailyLogin objDailyLogin)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                VMUserDailyLogin resultobj = dal_UserLoginVerification.GetUserDailyLoginHistory(objDailyLogin);
                if (resultobj.UserDailyLoginID.Count > 0)
                {
                    ObjAPIResponse.Object = (object)resultobj;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        #endregion

        #region Location ByNumbers

        [HttpPost]
        [ActionName("postOPAPPLocationByNumbers")]
        public HttpResponseMessage postOPAPPLocationByNumbers(LocationParkingLot obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                List<ParkingBay> lstbayNumber = dal_CheckIn.GetLocationLotBayNumbers(obj);
                if (lstbayNumber.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstbayNumber;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        #endregion

        #region CheckIn

        [HttpPost]
        [ActionName("postOPAPPGetVehicleParkingFee")]
        public HttpResponseMessage postOPAPPGetVehicleParkingFee(VehicleParkingFee obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                VehicleParkingFee objVehiclePass = dal_CheckIn.GetVehicleParkingFee(obj);
                ObjAPIResponse.Object = (object)objVehiclePass;
                ObjAPIResponse.Result = true;
                ObjAPIResponse.Message = "Success";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPGetLocaitonParkingLotVehicleParkingFee")]
        public HttpResponseMessage postOPAPPGetLocaitonParkingLotVehicleParkingFee(VehicleParkingFee obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                List<VehicleParkingFee> lstVehiclePass = dal_CheckIn.GetLocationParkingLotVehicleParkingFee(obj);
                if (lstVehiclePass.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstVehiclePass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)lstVehiclePass;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPPassVehicleCheckIn")]
        public HttpResponseMessage postOPAPPPassVehicleCheckIn(VehicleCheckIn obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                string result = dal_CheckIn.SavePassVehicleCheckIn(obj);
                if (result == "Success")
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPGovernmentVehicleCheckIn")]
        public HttpResponseMessage postOPAPPGovernmentVehicleCheckIn(VehicleCheckIn obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                string result = dal_CheckIn.GovernmentVehicleCheckIn(obj);
                if (result == "Success")
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPVehicleNewCheckIn")]
        public HttpResponseMessage postOPAPPVehicleNewCheckIn(VehicleCheckIn obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                CustomerParkingSlot objCustomerParkingSlot = dal_CheckIn.SaveVehicleNewCheckIn(obj);
                if (objCustomerParkingSlot.CustomerParkingSlotID != 0)
                {
                    ObjAPIResponse.Object = (object)objCustomerParkingSlot;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPVerifyVehicleInCheckInStatus")]
        public HttpResponseMessage postOPAPPVerifyVehicleInCheckInStatus(VehicleCheckIn obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                CustomerParkingSlot objCustomerParkingSlot = dal_CheckIn.VerifyVehicleInCheckInStatus(obj);
                if (objCustomerParkingSlot.CustomerParkingSlotID != 0)
                {
                    ObjAPIResponse.Object = (object)objCustomerParkingSlot;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPNFCCardVehicleCheckIn")]
        public HttpResponseMessage postOPAPPNFCCardVehicleCheckIn(VehicleCheckIn obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                CustomerVehiclePass objCustomerpass = dal_CheckIn.GetNFCCardVehiclePassDetails(obj.NFCCardNumber);
                if (objCustomerpass.CustomerVehiclePassID != 0)
                {
                    string resultmsg = dal_CheckIn.VerifyCustomerNFCCardExpiry(objCustomerpass, obj.LocationID);
                    if (resultmsg == "Valid")
                    {
                        obj.CustomerID = objCustomerpass.CustomerVehicleID.CustomerID.CustomerID;
                        obj.RegistrationNumber = objCustomerpass.CustomerVehicleID.RegistrationNumber;
                        obj.BayNumber = null;
                        obj.VehicleTypeCode = objCustomerpass.CustomerVehicleID.VehicleTypeID.VehicleTypeCode;

                        var objresult = dal_CheckIn.VerifyVehicleInCheckInStatus(obj); // Verify Is Vehicle Alreday in Check-In
                        if (objresult.CustomerParkingSlotID == 0)
                        {
                            string checkinmsg = dal_CheckIn.SavePassVehicleCheckIn(obj);
                            if (checkinmsg == "Success")
                            {
                                ObjAPIResponse.Object = null;
                                ObjAPIResponse.Result = true;
                                ObjAPIResponse.Message = checkinmsg;
                            }
                            else
                            {
                                ObjAPIResponse.Object = null;
                                ObjAPIResponse.Result = false;
                                ObjAPIResponse.Message = "Fail:Please contact admin";
                            }
                        }
                        else
                        {
                            ObjAPIResponse.Object = null;
                            ObjAPIResponse.Result = false;
                            ObjAPIResponse.Message = "Vehicle already in checkin status " + (objresult.LocationParkingLotID.LocationID.LocationName + "-" + objresult.LocationParkingLotID.LocationParkingLotName);
                        }

                    }
                    else
                    {
                        string respmsg = "(" + objCustomerpass.CardNumber + "," + objCustomerpass.CustomerVehicleID.RegistrationNumber + "," + Convert.ToDateTime(objCustomerpass.ExpiryDate).ToString("MMM/dd/yyy") + ")";
                        ObjAPIResponse.Object = null;
                        ObjAPIResponse.Result = false;
                        ObjAPIResponse.Message = "Please verify your NFC Card-" + respmsg;
                    }

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        #endregion

        #region Violation and Reason

        [HttpGet]
        [ActionName("getOPAPPGetViolationReasons")]
        public HttpResponseMessage getOPAPPGetViolationReasons(string StatusCode)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALViolation dal_Violation = new DALViolation();
                List<ViolationReason> lstViolationReason = dal_Violation.GetViolationReasons(StatusCode);
                if (lstViolationReason.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstViolationReason;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postVehicleViolationWaring")]
        public HttpResponseMessage postVehicleViolationWaring(CustomerVehicle objCustVehicle)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALViolation dal_Violation = new DALViolation();
                ViolationAndClamp ojResult = dal_Violation.GetVehicleViolationWarningCount(objCustVehicle);

                ObjAPIResponse.Object = (object)null;
                ObjAPIResponse.Result = Convert.ToBoolean(ojResult.IsWarning);
                ObjAPIResponse.Message = (ojResult.IsWarning == false) ? "Fail" : "Success";

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPSaveVehicleViolationAndClamp")]
        public HttpResponseMessage postOPAPPSaveVehicleViolationAndClamp(ViolationAndClamp objViolationAndClamp)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALViolation dal_Violation = new DALViolation();
                string resultmsg = dal_Violation.SaveVehicleViolationAndClamp(objViolationAndClamp);
                ObjAPIResponse.Object = (object)null;
                ObjAPIResponse.Result = true;
                ObjAPIResponse.Message = resultmsg;
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        #endregion

        #region Home 

        [HttpPost]
        [ActionName("postOPAPPAllParkedVehicleDetails")]
        public HttpResponseMessage postOPAPPAllParkedVehicleDetails(ParkedVehiclesFilter objLocationAllParkedVehicles)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                VMLocationLotParkedVehicles objresult = dal_CustomerVehicleParkingLot.GetParkedVehicles(objLocationAllParkedVehicles);

                if (objresult.CustomerParkingSlotID.Count > 0)
                {
                    ObjAPIResponse.Object = (object)objresult;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {

                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPLoginUserAllocatedLocationAndLots")]
        public HttpResponseMessage postOPAPPLoginUserAllocatedLocationAndLots(User objloginUser)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                List<LocationParkingLot> lstLocationParkingLot = dal_UserLoginVerification.GetLoginUserAllocatedLocationLots(objloginUser);

                if (lstLocationParkingLot.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstLocationParkingLot;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {

                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getSelectedParkedVehicleDetails")]
        public HttpResponseMessage getSelectedParkedVehicleDetails(int CustomerParkingSlotID)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                CustomerParkingSlot objresult = dal_CustomerVehicleParkingLot.GetParkedVehicleDetails(CustomerParkingSlotID);

                if (objresult != null)
                {
                    ObjAPIResponse.Object = (object)objresult;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("getViolationVehicleCharges")]
        public HttpResponseMessage getViolationVehicleCharges(ViolationVehicleFees objinput)
        {
            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                ViolationVehicleFees objresult = dal_CustomerVehicleParkingLot.GetViolationVehicleCharges(objinput);
                ObjAPIResponse.Object = (object)objresult;
                ObjAPIResponse.Result = true;
                ObjAPIResponse.Message = "Success";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getAllApplicationTypes")]
        public HttpResponseMessage getAllApplicationTypes()
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                List<ApplicationType> lstapptypes = dal_CustomerVehicleParkingLot.GetAllApplicationTypes();

                if (lstapptypes.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstapptypes;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getAllStatus")]
        public HttpResponseMessage getAllStatus()
        {
            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                List<Status> lstStatus = dal_CustomerVehicleParkingLot.GetAllStatus();
                if (lstStatus.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstStatus;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        #endregion

        #region CheckOut
        [HttpPost]
        [ActionName("postOPAPPSaveVehcileCheckOut")]
        public HttpResponseMessage putOPAPPSaveVehcileCheckOut(CustomerParkingSlot objCustomerParkingSlot)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALVehicleCheckOut dal_checkOut = new DALVehicleCheckOut();
                CustomerParkingSlot resultOut = dal_checkOut.VehicleCheckOut(objCustomerParkingSlot);
                if (resultOut != null)
                {
                    ObjAPIResponse.Object = (object)resultOut;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPUpdateVehicleClampStaus")]
        public HttpResponseMessage putOPAPPUpdateVehicleClampStaus(CustomerParkingSlot objCustomerParkingSlot)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALVehicleCheckOut dal_checkOut = new DALVehicleCheckOut();
                string resultmsg = dal_checkOut.UpdateVehicleClampStaus(objCustomerParkingSlot);
                ObjAPIResponse.Object = (object)null;
                ObjAPIResponse.Result = true;
                ObjAPIResponse.Message = resultmsg;
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        #endregion

        #region Pass
        [HttpPost]
        [ActionName("postOPAPPVerifyVehicleHasPass")]
        public HttpResponseMessage postOPAPPVerifyVehicleHasPass(VehiclePass obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                CustomerVehiclePass objCustomerVehiclePass = dal_CheckIn.VerifyVehicleHasPass(obj);
                if (objCustomerVehiclePass.CustomerVehiclePassID != 0)
                {
                    ObjAPIResponse.Object = (object)objCustomerVehiclePass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPVerifyNFCCardPass")]
        public HttpResponseMessage postOPAPPVerifyNFCCardPass(VehiclePass obj)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCheckIn dal_CheckIn = new DALCheckIn();
                CustomerVehiclePass objCustomerVehiclePass = dal_CheckIn.VerifyVehicleHasPass(obj);
                if (objCustomerVehiclePass.CustomerVehiclePassID != 0)
                {
                    ObjAPIResponse.Object = (object)objCustomerVehiclePass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getOPAPPGetAllLocations")]
        public HttpResponseMessage getOPAPPGetAllLocations()
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<Location> lstLocation = dal_Pass.GetAllLocation();
                if (lstLocation.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstLocation;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getOPAPPGetAllLocationsByVehicleType")]
        public HttpResponseMessage getOPAPPGetAllLocationsByVehicleType(string VehicleTypeCode)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<Location> lstLocation = dal_Pass.GetAllLocationByVehicleType(VehicleTypeCode);
                if (lstLocation.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstLocation;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpGet]
        [ActionName("getOPAPPGetAllPassLocationsByVehicleType")]
        public HttpResponseMessage getOPAPPGetAllPassLocationsByVehicleType(string VehicleTypeCode,string CustomerVehiclePassId)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<Location> lstLocation = dal_Pass.GetAllPassLocationByVehicleType(VehicleTypeCode, CustomerVehiclePassId);
                if (lstLocation.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstLocation;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postOPAPPPassPriceDetails")]
        public HttpResponseMessage getOPAPPPassPriceDetails(VehicleLotPassPrice objVehicleType)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<PassPrice> lstPassPrices = dal_Pass.GetPassPriceDetails(objVehicleType);
                if (lstPassPrices.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstPassPrices;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("getCustomerVehiclePassDetails")]
        public HttpResponseMessage GetCustomerVehiclePassDetails(CustomerVehiclePass objInputPass)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                CustomerVehiclePass objResultCustomerPass = dal_Pass.GetCustomerVehiclePassDetails(objInputPass);
                if (objResultCustomerPass.CustomerVehiclePassID != 0)
                {
                    ObjAPIResponse.Object = (object)objResultCustomerPass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postSaveCustomerVehiclePass")]
        public HttpResponseMessage postSaveCustomerVehiclePass(CustomerVehiclePass objCustomerVehiclePass)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                CustomerVehiclePass objResult = dal_Pass.SaveCustomerVehiclePass(objCustomerVehiclePass);
                if (objResult.CustomerVehiclePassID != 0)
                {
                    ObjAPIResponse.Object = (object)objResult;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }



                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postSaveMultiStationCustomerVehiclePass")]
        public HttpResponseMessage postSaveMultiStationCustomerVehiclePass(VMMultiStationCustomerVehiclePass objvmCustomerVehiclePass)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<CustomerVehiclePass> lstMultiPass = null;
                int resultID = 0;
                if (objvmCustomerVehiclePass.LocationID.Count > 0)
                {
                    for (int i = 0; i < objvmCustomerVehiclePass.LocationID.Count; i++)
                    {
                        objvmCustomerVehiclePass.CustomerVehiclePassID.LocationID.LocationID = objvmCustomerVehiclePass.LocationID[i].LocationID;
                        if (i == 0)
                        {
                            objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = 0;
                            resultID = dal_Pass.SaveCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                        }
                        else
                        {

                            objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = resultID;
                            dal_Pass.SaveCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                        }



                    }
                    lstMultiPass = dal_Pass.GetCustomerVehiclePassesByVehicle(objvmCustomerVehiclePass.CustomerVehiclePassID.CustomerVehicleID.RegistrationNumber);
                }


                if (lstMultiPass != null)
                {
                    ObjAPIResponse.Object = (object)lstMultiPass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        [HttpPost]
        [ActionName("postValidateVehiclePass")]
        public HttpResponseMessage postValidateVehiclePass(CustomerVehicle selectedVehicle)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                List<CustomerVehiclePass> lstMultiPass = null;
                if (selectedVehicle.RegistrationNumber != null && selectedVehicle.RegistrationNumber != "")
                {
                    lstMultiPass = dal_Pass.GetCustomerVehiclePassesByVehicle(selectedVehicle.RegistrationNumber);
                    if (lstMultiPass != null)
                    {
                        ObjAPIResponse.Object = (object)lstMultiPass;
                        ObjAPIResponse.Result = true;
                        ObjAPIResponse.Message = "Success";
                    }
                    else
                    {
                        ObjAPIResponse.Object = null;
                        ObjAPIResponse.Result = false;
                        ObjAPIResponse.Message = "Fail";
                    }
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "RegistrationNumber not able to find,Please contact admin";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }



        [HttpPost]
        [ActionName("postVerifyVehiclePass")]
        public HttpResponseMessage postVerifyVehiclePass(CustomerVehiclePass objCustomerVehiclePass)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                bool isDayPassExist = dal_Pass.ValidateCustomerVehiclePass(objCustomerVehiclePass);
                if (isDayPassExist)
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = isDayPassExist;
                    ObjAPIResponse.Message = "Vehicle alreday has pass";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = isDayPassExist;
                    ObjAPIResponse.Message = "No Records Found";
                }



                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        #endregion

        #region ReNewPass

        [HttpGet]
        [ActionName("getOPAPPGetAllPassVehicles")]
        public HttpResponseMessage getOPAPPGetAllPassVehicles()
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALReNewPass dal_ReNewPass = new DALReNewPass();
                List<CustomerVehicle> lstCustomerVehicle = dal_ReNewPass.GetAllPassVehicles();
                if (lstCustomerVehicle.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstCustomerVehicle;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPGetCustomerVehicleDetailsByVehicle")]
        public HttpResponseMessage postOPAPPGetCustomerVehicleDetailsByVehicle(CustomerVehicle objInputCustomerVehicle)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                CustomerVehiclePass objCustomerVehiclePass = dal_Pass.GetCustomerVehicleDetailsByVehicle(objInputCustomerVehicle);
                if (objCustomerVehiclePass != null)
                {
                    ObjAPIResponse.Object = (object)objCustomerVehiclePass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postOPAPPActivateCustomerVehiclePass")]
        public HttpResponseMessage postOPAPPActivateCustomerVehiclePass(CustomerVehiclePass objInputCustomerVehiclePass)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALPass dal_Pass = new DALPass();
                CustomerVehiclePass objCustomerVehiclePass = dal_Pass.ActivateCustomerVehiclePass(objInputCustomerVehiclePass);
                if (objCustomerVehiclePass != null)
                {
                    ObjAPIResponse.Object = (object)objCustomerVehiclePass;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = (object)null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "Fail";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }



        #endregion

        #region  Menu Bar Items

        [HttpGet]
        [ActionName("getAllVehicleRegistrationNumbers")]
        public HttpResponseMessage getAllVehicleRegistrationNumbers()
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicle dal_CustomerVehicle = new DALCustomerVehicle();
                List<CustomerVehicle> lstResult = dal_CustomerVehicle.GetAllVehicleRegistrationNumbers();
                if (lstResult.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstResult;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postVehicleParkingHistory")]
        public HttpResponseMessage getVehicleParkingHistory(CustomerVehicle objCustomerVehicle)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                List<CustomerParkingSlot> lsthistory = dal_CustomerVehicleParkingLot.GetVehicleParkingHistory(objCustomerVehicle);
                if (lsthistory.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lsthistory;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postLocationLotReport")]
        public HttpResponseMessage postLocationLotReport(User objlotuser)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                VMReportSummary objReport = new VMReportSummary();
                objReport.VMLocationLotParkingReportID = dal_CustomerVehicleParkingLot.GetLocationLotParkingReport(objlotuser);
                objReport.VMLocationLotPassReportID = dal_CustomerVehicleParkingLot.GetLocationLotPassReport(objlotuser);
                objReport.VMLocationLotViolationsID = dal_CustomerVehicleParkingLot.GetLocationLotViolationsReport(objlotuser);

                decimal ParkingRevenueCash = (objReport.VMLocationLotParkingReportID.LotRevenueCash == "" || objReport.VMLocationLotParkingReportID.LotRevenueCash == null) ? 0 : Convert.ToDecimal(objReport.VMLocationLotParkingReportID.LotRevenueCash);
                decimal ParkingRevenueEpay = (objReport.VMLocationLotParkingReportID.LotRevenueEpay == "" || objReport.VMLocationLotParkingReportID.LotRevenueEpay == null) ? 0 : Convert.ToDecimal(objReport.VMLocationLotParkingReportID.LotRevenueEpay);
                decimal PassCash = (objReport.VMLocationLotPassReportID.TotalCash == 0 || objReport.VMLocationLotPassReportID.TotalCash == null) ? 0 : objReport.VMLocationLotPassReportID.TotalCash;
                decimal PassEPay = (objReport.VMLocationLotPassReportID.TotalEPay == 0 || objReport.VMLocationLotPassReportID.TotalEPay == null) ? 0 : objReport.VMLocationLotPassReportID.TotalEPay;
                decimal ViolationCash = (objReport.VMLocationLotViolationsID.TotalCash == 0 || objReport.VMLocationLotViolationsID.TotalCash == null) ? 0 : objReport.VMLocationLotViolationsID.TotalCash;
                decimal ViolationEPay = (objReport.VMLocationLotViolationsID.TotalEPay == 0 || objReport.VMLocationLotViolationsID.TotalEPay == null) ? 0 : objReport.VMLocationLotViolationsID.TotalEPay;


                objReport.Cash = ParkingRevenueCash + PassCash + ViolationCash;
                objReport.EPay = ParkingRevenueEpay + PassEPay + ViolationEPay;

                if (objReport != null)
                {
                    ObjAPIResponse.Object = (object)objReport;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";

                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }

        [HttpPost]
        [ActionName("postSupervisorOperators")]
        public HttpResponseMessage postSupervisorOperators(User objSupervisor)
        {

            ObjAPIResponse = new APIResponse();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                List<User> lstOperators = dal_UserLoginVerification.GetSuperVisorOperators(objSupervisor);
                if (lstOperators.Count > 0)
                {
                    ObjAPIResponse.Object = (object)lstOperators;
                    ObjAPIResponse.Result = true;
                    ObjAPIResponse.Message = "Success";
                }
                else
                {
                    ObjAPIResponse.Object = null;
                    ObjAPIResponse.Result = false;
                    ObjAPIResponse.Message = "No Records Found";
                }

                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            catch (Exception ex)
            {
                ObjAPIResponse.Object = null;
                ObjAPIResponse.Result = false;
                ObjAPIResponse.Message = "Please contact administration";
                resp.Content = new StringContent(JsonConvert.SerializeObject(ObjAPIResponse));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }
            return resp;
        }


        #endregion

    }
}
