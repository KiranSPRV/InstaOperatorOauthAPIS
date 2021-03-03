using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using InstaOperatorOauthAPIS.DAL;
using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.APIResult;
using InstaOperatorOauthAPIS.Models.FirebaseModel;
using InstaOperatorOauthAPIS.VMModels;
using ISTAOnlineWebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InstaOperatorOauthAPIS.Controllers
{
    public class ValuesController : ApiController
    {


        #region User Login 

        [HttpGet]
        [ActionName("getFBSaveUserDailyLogin")]
        public string getFBSaveUserDailyLogin(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                var objUserDailyLogin = Task.Run(async () => await
                                                                objFBHelper.Get_FBUserDailyLogin(id)
                                                              ).Result;
                if (objUserDailyLogin != null && objUserDailyLogin.UserID.UserID != 0)
                {
                    resultmsg = dal_UserLoginVerification.FB_SaveUserDailyLogin(objUserDailyLogin);
                }
            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        [HttpGet]
        [ActionName("getFBUpdateUserDailyLogin")]
        public string getFBUpdateUserDailyLogin(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                var objUserDailyLogin = Task.Run(async () => await
                                                                objFBHelper.Get_FBUserDailyLogin(id)
                                                              ).Result;
                if (objUserDailyLogin != null && objUserDailyLogin.UserID.UserID != 0)
                {
                    resultmsg = dal_UserLoginVerification.FB_UpdateUserDailyLogin(objUserDailyLogin);
                }
            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        [HttpGet]
        [ActionName("getFBUpdateUserPassword")]
        public string getFBUpdateUserPassword(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {
                DALUserLoginVerification dal_UserLoginVerification = new DALUserLoginVerification();
                var objUser = Task.Run(async () => await
                                                                objFBHelper.Get_FBUserDetails(id)
                                                              ).Result;
                if (objUser != null && objUser.UserID != 0)
                {
                    resultmsg = dal_UserLoginVerification.FB_UpateLoginUserPassword(objUser);
                }
                else
                {
                    resultmsg = objUser.UserID + "," + objUser.Password;
                }

            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        #endregion

        [HttpGet]
        [ActionName("postFBEntryVehicleNewCheckIn")]
        public string postFBEntryVehicleNewCheckIn(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {
                FBCustomerParkingSlot objFBCheckIn = null;
                CustomerParkingSlot objCustomerParkingSlot = null;
                DALCheckIn dal_CheckIn = new DALCheckIn();
                DALViolation dal_Violation = new DALViolation();
                try
                {

                    objFBCheckIn = Task.Run(async () => await
                                                                objFBHelper.GetAllLotParkedVehicles(id)
                                                              ).Result;
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        resultmsg = resultmsg + "," + errInner.Message; //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
                VehicleCheckIn objNewCheckIn = new VehicleCheckIn();
                if (objFBCheckIn != null && objFBCheckIn.CustomerVehicleID.RegistrationNumber != "")
                {
                    objNewCheckIn = new VehicleCheckIn();
                    objNewCheckIn.UserID = objFBCheckIn.CreatedBy.UserID;
                    objNewCheckIn.LocationID = objFBCheckIn.LocationParkingLotID.LocationID.LocationID;
                    objNewCheckIn.LocationName = objFBCheckIn.LocationParkingLotID.LocationID.LocationName;
                    objNewCheckIn.LocationParkingLotID = objFBCheckIn.LocationParkingLotID.LocationParkingLotID;
                    objNewCheckIn.LocationParkingLotName = objFBCheckIn.LocationParkingLotID.LocationParkingLotName;
                    objNewCheckIn.VehicleTypeCode = objFBCheckIn.VehicleTypeID.VehicleTypeCode;
                    objNewCheckIn.BayNumberID = objFBCheckIn.ParkingBayID;
                    objNewCheckIn.BayNumber = objFBCheckIn.ParkingBayName;
                    objNewCheckIn.RegistrationNumber = objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                    objNewCheckIn.PhoneNumber = objFBCheckIn.PhoneNumber;
                    objNewCheckIn.ParkingHours = objFBCheckIn.Duration == null || objFBCheckIn.Duration == string.Empty ? 0 : Convert.ToInt32(objFBCheckIn.Duration);
                    objNewCheckIn.ParkingFees = objFBCheckIn.PaidAmount;
                    objNewCheckIn.PaymentType = objFBCheckIn.PaymentTypeID.PaymentTypeCode;

                    objNewCheckIn.ParkingStartTime = Convert.ToDateTime(objFBCheckIn.ActualStartTime).ToString("MM / dd / yyyy hh: mm tt");
                    objNewCheckIn.ParkingEndTime = Convert.ToDateTime(objFBCheckIn.ActualEndTime).ToString("MM / dd / yyyy hh: mm tt");

                    if ((objFBCheckIn.ApplicationTypeID.ApplicationTypeCode.ToUpper() == "O".ToUpper() || (objFBCheckIn.ApplicationTypeID.ApplicationTypeCode.ToUpper() == "A".ToUpper())) && objFBCheckIn.StatusID.StatusCode.ToUpper() == "CHKIN".ToUpper())
                    {
                        objCustomerParkingSlot = dal_CheckIn.SaveVehicleNewCheckInFromFirebase(objNewCheckIn);
                        objExceptionlog.InsertException("WebAPI", "", "ValuesController", "postFBEntryVehicleNewCheckIn: " + objCustomerParkingSlot.CustomerParkingSlotID, "SaveVehicleNewCheckInFromFirebase");
                    }
                    else if (objFBCheckIn.ApplicationTypeID.ApplicationTypeCode.ToUpper() == "P".ToUpper() && objFBCheckIn.StatusID.StatusCode.ToUpper() == "CHKIN".ToUpper())
                    {
                        if (objFBCheckIn.IsNFCCheckIn)
                        {
                            string nfcChkInMsg = dal_CheckIn.SaveNFCCardPassVehicleCheckInFromFirebase(objNewCheckIn);
                            objExceptionlog.InsertException("WebAPI", "No", "ValuesController", "postFBEntryVehicleNewCheckIn: NFC Card CheckIn" + nfcChkInMsg, "SaveNFCCardPassVehicleCheckInFromFirebase");
                        }
                        else
                        {
                            objExceptionlog.InsertException("WebAPI", "No", "ValuesController", "postFBEntryVehicleNewCheckIn: Regualr Pass CheckIn HIT", "SavePassVehicleCheckInFromFirebase");
                            objCustomerParkingSlot = dal_CheckIn.SavePassVehicleCheckInFromFirebase(objNewCheckIn);
                            objExceptionlog.InsertException("WebAPI", "No", "ValuesController", "postFBEntryVehicleNewCheckIn: Regualr Pass CheckIn" + objCustomerParkingSlot.CustomerParkingSlotID, "SavePassVehicleCheckInFromFirebase");
                        }

                    }
                    else if (objFBCheckIn.ApplicationTypeID.ApplicationTypeCode.ToUpper() == "O".ToUpper() && objFBCheckIn.StatusID.StatusCode.ToUpper() == "G".ToUpper())
                    {
                        objNewCheckIn.GovernmentVehicleImage = objFBCheckIn.GovernmentVehicleImage;
                        objNewCheckIn.VehicleImageLottitude = objFBCheckIn.VehicleImageLottitude;
                        objNewCheckIn.VehicleImageLongitude = objFBCheckIn.VehicleImageLongitude;
                        objNewCheckIn.StatusName = objFBCheckIn.StatusID.StatusName.ToUpper();
                        objCustomerParkingSlot = dal_CheckIn.GovernmentVehicleCheckInFromFirebase(objNewCheckIn);
                        objExceptionlog.InsertException("WebAPI", "No", "ValuesController", "postFBEntryVehicleNewCheckIn: " + objCustomerParkingSlot.CustomerParkingSlotID, "GovernmentVehicleCheckInFromFirebase");
                    }
                    else if (objFBCheckIn.ApplicationTypeID.ApplicationTypeCode.ToUpper() == "O".ToUpper() && objFBCheckIn.StatusID.StatusCode.ToUpper() == "V".ToUpper())
                    {
                        ViolationAndClamp objSQLViolationAndClamp = new ViolationAndClamp();
                        objSQLViolationAndClamp.UserID = objFBCheckIn.CreatedBy.UserID;
                        objSQLViolationAndClamp.UserTypeID = objFBCheckIn.CreatedBy.UserTypeID.UserTypeID;
                        objSQLViolationAndClamp.LocationID = objFBCheckIn.LocationParkingLotID.LocationID.LocationID;
                        objSQLViolationAndClamp.LocationParkingLotID = objFBCheckIn.LocationParkingLotID.LocationParkingLotID;
                        objSQLViolationAndClamp.LocationName = objFBCheckIn.LocationParkingLotID.LocationID.LocationName;
                        objSQLViolationAndClamp.RegistrationNumber = objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                        objSQLViolationAndClamp.BayNumberID = objFBCheckIn.ParkingBayID;
                        objSQLViolationAndClamp.BayNumber = objFBCheckIn.ParkingBayName;
                        objSQLViolationAndClamp.IsClamp = objFBCheckIn.IsClamp;
                        objSQLViolationAndClamp.ReasonID = objFBCheckIn.ViolationReasonID.ViolationReasonID;
                        objSQLViolationAndClamp.ReasonName = objFBCheckIn.ViolationReasonID.Reason;
                        objSQLViolationAndClamp.VehicleTypeCode = objFBCheckIn.VehicleTypeID.VehicleTypeCode;
                        objSQLViolationAndClamp.ViolationStartTime = objFBCheckIn.ActualStartTime;
                        objSQLViolationAndClamp.ViolationTime = Convert.ToDateTime(objFBCheckIn.ActualStartTime).ToString("MM / dd / yyyy hh: mm tt");
                        objSQLViolationAndClamp.IsWarning = objFBCheckIn.IsWarning; ;
                        objSQLViolationAndClamp.VehicleImageLottitude = objFBCheckIn.VehicleImageLottitude;
                        objSQLViolationAndClamp.VehicleImageLongitude = objFBCheckIn.VehicleImageLongitude;
                        objSQLViolationAndClamp.ViolationImage = objFBCheckIn.ViolationImage;
                        objCustomerParkingSlot = dal_Violation.FBSaveVehicleViolationAndClamp(objSQLViolationAndClamp);
                        if (objFBCheckIn.IsWarning)
                        {
                            if (id != null && id != "null" && id != "")
                            {
                                VehicleViolationWarning objVehicleViolationWarning = new VehicleViolationWarning();
                                objVehicleViolationWarning.RegistrationNumber = objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                                objVehicleViolationWarning.VehicleTypeID.VehicleTypeCode = objFBCheckIn.VehicleTypeID.VehicleTypeCode;
                                objVehicleViolationWarning.CreatedBy = objFBCheckIn.CreatedBy.UserID;
                                objVehicleViolationWarning.CreatedOn = DateTime.Now;
                                objFBHelper.InsertUpdateFirebaseVehicleViolationWarning(objVehicleViolationWarning, id);




                            }

                        }


                        objExceptionlog.InsertException("WebAPI", "No", "ValuesController", "postFBEntryVehicleNewCheckIn: " + objCustomerParkingSlot.CustomerParkingSlotID, "FBSaveVehicleViolationAndClamp");
                    }
                    if (objCustomerParkingSlot.CustomerParkingSlotID != 0)
                    {
                        resultmsg = "Success :" + objFBCheckIn.CustomerVehicleID.RegistrationNumber + "," + objCustomerParkingSlot.CustomerParkingSlotID;
                    }
                    else
                    {
                        resultmsg = "Failed: " + objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                    }


                }
            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        #region CheckOut

        [HttpGet]
        [ActionName("getFBSaveVehcileCheckOut")]
        public string getFBSaveVehcileCheckOut(string id)
        {
            string checkOut = string.Empty;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            try
            {
                CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
                DALVehicleCheckOut dal_checkOut = new DALVehicleCheckOut();
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                FBCustomerParkingSlot objFBCheckIn = null;
                try
                {


                    objFBCheckIn = Task.Run(async () => await
                                                                objFBHelper.GetAllLotParkedVehicles(id)
                                                              ).Result;

                    var resultobj = dal_CustomerVehicleParkingLot.GetParkedVehicleDetailsFromFirebase(objFBCheckIn.RegistrationNumber);
                    objCustomerParkingSlot.CustomerParkingSlotID = resultobj.CustomerParkingSlotID;
                    objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultobj.CustomerVehicleID.CustomerVehicleID;
                    objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultobj.LocationParkingLotID.LocationParkingLotID;
                    objCustomerParkingSlot.StatusID.StatusCode = objFBCheckIn.StatusID.StatusCode;
                    objCustomerParkingSlot.PaymentTypeID.PaymentTypeCode = objFBCheckIn.PaymentTypeID.PaymentTypeCode;
                    objCustomerParkingSlot.IsClamp = objFBCheckIn.IsClamp;
                    objCustomerParkingSlot.ActualEndTime = objFBCheckIn.ActualEndTime;
                    objCustomerParkingSlot.Amount = objFBCheckIn.Amount;
                    objCustomerParkingSlot.ViolationFees = objFBCheckIn.ViolationFees;

                    if (objFBCheckIn.ViolationReasonID != null)
                    {
                        objCustomerParkingSlot.ViolationReasonID.ViolationReasonID = objFBCheckIn.ViolationReasonID.ViolationReasonID;
                    }
                    if (objFBCheckIn.FOCReasonID != null)
                    {
                        objCustomerParkingSlot.FOCReasonID.ViolationReasonID = objFBCheckIn.FOCReasonID.ViolationReasonID;
                    }


                    objCustomerParkingSlot.ClampFees = objFBCheckIn.ClampFees;
                    objCustomerParkingSlot.ExtendAmount = objFBCheckIn.ExtendAmount;
                    objCustomerParkingSlot.Duration = objFBCheckIn.Duration;
                    objCustomerParkingSlot.CreatedBy = objFBCheckIn.UpdatedBy.UserID;
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        checkOut = checkOut + "," + errInner.Message; //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
                CustomerParkingSlot resultOut = null;
                if (objFBCheckIn.StatusID.StatusCode == "O")
                {
                    resultOut = dal_checkOut.VehicleOverstayFromFirebase(objCustomerParkingSlot);
                    objExceptionlog.InsertException("WebAPI", "", "ValuesController", "getFBSaveVehcileCheckOut: " + objCustomerParkingSlot.CustomerParkingSlotID, "VehicleOverstayFromFirebase");
                }
                else
                {
                    resultOut = dal_checkOut.VehicleCheckOutFromFirebase(objCustomerParkingSlot);
                    objExceptionlog.InsertException("WebAPI", "", "ValuesController", "getFBSaveVehcileCheckOut: " + objCustomerParkingSlot.CustomerParkingSlotID, "VehicleCheckOutFromFirebase");
                }


                if (resultOut != null && resultOut.CustomerParkingSlotID != 0)
                {
                    checkOut = "Success";
                }
                else
                {

                    checkOut = "Fail";
                }


            }
            catch (Exception ex)
            {

                checkOut = ex.Message;
            }
            return checkOut;
        }

        [HttpGet]
        [ActionName("getFBSaveVehcileOverStay")]
        public string getFBSaveVehcileOverStay(string id)
        {
            string checkOut = string.Empty;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            FBHelper objFBHelper = new FBHelper();
            try
            {
                CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
                DALVehicleCheckOut dal_checkOut = new DALVehicleCheckOut();
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                FBCustomerParkingSlot objFBCheckIn = null;
                try
                {


                    objFBCheckIn = Task.Run(async () => await
                                                                objFBHelper.GetAllLotParkedVehicles(id)
                                                              ).Result;

                    var resultobj = dal_CustomerVehicleParkingLot.GetParkedVehicleDetailsFromFirebase(objFBCheckIn.RegistrationNumber);
                    objCustomerParkingSlot.CustomerParkingSlotID = resultobj.CustomerParkingSlotID;
                    objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultobj.CustomerVehicleID.CustomerVehicleID;
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        checkOut = checkOut + "," + errInner.Message; //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
                CustomerParkingSlot resultOut = null;
                if (objFBCheckIn.StatusID.StatusCode == "O")
                {
                    resultOut = dal_checkOut.VehicleOverstayFromFirebase(objCustomerParkingSlot);
                    objExceptionlog.InsertException("WebAPI", "", "ValuesController", "getFBSaveVehcileOverStay: " + objCustomerParkingSlot.CustomerParkingSlotID, "VehicleOverstayFromFirebase");
                }
                if (resultOut != null && resultOut.CustomerParkingSlotID != 0)
                {
                    checkOut = "Success:" + resultOut.CustomerParkingSlotID + " , " + objFBCheckIn.StatusID.StatusCode;
                }
                else
                {
                    checkOut = "Fail:" + resultOut.CustomerParkingSlotID + " , " + objFBCheckIn.StatusID.StatusCode;
                }

            }
            catch (Exception ex)
            {

                checkOut = ex.Message;
            }
            return checkOut;
        }

        #endregion

        [HttpGet]
        [ActionName("getFBUpdateVehicleClampStaus")]
        public string getFBUpdateVehicleClampStaus(string id)
        {

            string updateClamp = string.Empty;
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            try
            {
                CustomerParkingSlot objCustomerParkingSlot = new CustomerParkingSlot();
                DALVehicleCheckOut dal_checkOut = new DALVehicleCheckOut();
                DALCustomerVehicleParkingLot dal_CustomerVehicleParkingLot = new DALCustomerVehicleParkingLot();
                FBHelper objFBHelper = new FBHelper();
                FBCustomerParkingSlot objFBCheckIn = null;
                try
                {

                    objFBCheckIn = Task.Run(async () => await
                                                                objFBHelper.GetAllLotParkedVehicles(id)
                                                              ).Result;

                    var resultobj = dal_CustomerVehicleParkingLot.GetParkedVehicleDetailsFromFirebase(objFBCheckIn.RegistrationNumber);
                    objCustomerParkingSlot.CustomerParkingSlotID = resultobj.CustomerParkingSlotID;
                    objCustomerParkingSlot.StatusID.StatusID = resultobj.StatusID.StatusID;
                    objCustomerParkingSlot.CustomerVehicleID.CustomerVehicleID = resultobj.CustomerVehicleID.CustomerVehicleID;
                    objCustomerParkingSlot.LocationParkingLotID.LocationParkingLotID = resultobj.LocationParkingLotID.LocationParkingLotID;
                    objCustomerParkingSlot.VehicleTypeID.VehicleTypeID = resultobj.VehicleTypeID.VehicleTypeID;
                    objCustomerParkingSlot.StatusID.StatusCode = objFBCheckIn.StatusID.StatusCode;
                    objCustomerParkingSlot.PaymentTypeID.PaymentTypeCode = objFBCheckIn.PaymentTypeID.PaymentTypeCode;
                    objCustomerParkingSlot.IsClamp = objFBCheckIn.IsClamp;
                    objCustomerParkingSlot.ViolationReasonID.ViolationReasonID = objFBCheckIn.ViolationReasonID.ViolationReasonID;
                    objCustomerParkingSlot.IsWarning = objFBCheckIn.IsWarning;
                    objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber = objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                    objCustomerParkingSlot.Duration = objFBCheckIn.Duration;
                    objCustomerParkingSlot.CreatedBy = objFBCheckIn.UpdatedBy.UserID;
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        updateClamp = updateClamp + "," + errInner.Message; //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                    }
                }
                string updateresult = dal_checkOut.UpdateVehicleClampStausFromFirebase(objCustomerParkingSlot);
                if (objFBCheckIn.IsWarning)
                {
                    if (id != null && id != "null" && id != "")
                    {
                        VehicleViolationWarning objVehicleViolationWarning = new VehicleViolationWarning();
                        objVehicleViolationWarning.RegistrationNumber = objFBCheckIn.CustomerVehicleID.RegistrationNumber;
                        objVehicleViolationWarning.VehicleTypeID.VehicleTypeCode = objFBCheckIn.VehicleTypeID.VehicleTypeCode;
                        objVehicleViolationWarning.CreatedBy = objFBCheckIn.CreatedBy.UserID;
                        objVehicleViolationWarning.CreatedOn = DateTime.Now;
                        objFBHelper.InsertUpdateFirebaseVehicleViolationWarning(objVehicleViolationWarning, id);
                    }

                }
                objExceptionlog.InsertException("WebAPI", "", "ValuesController", "getFBUpdateVehicleClampStaus: " + objCustomerParkingSlot.CustomerVehicleID, "UpdateVehicleClampStausFromFirebase");
                if (updateresult == "Success")
                {
                    updateClamp = "Success";
                }
                else
                {

                    updateClamp = "Fail";
                }


            }
            catch (Exception ex)
            {

                updateClamp = ex.Message;
            }
            return updateClamp;
        }


        #region PASS Section

        [HttpGet]
        [ActionName("getFBSaveCustomerVehiclePassDetails")]
        public string getFBSaveCustomerVehiclePassDetails(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            DALPass dal_Pass = new DALPass();
            FBHelper objFBHelper = new FBHelper();
            CustomerVehiclePass objCustomerVehiclePass;
            string resultmsg = string.Empty;
            try
            {
                objCustomerVehiclePass = Task.Run(async () => await
                                                                objFBHelper.Get_FBCustomerVehiclePassDetails(id)
                                                              ).Result;
                if (objCustomerVehiclePass != null)
                {
                    CustomerVehiclePass objResult = dal_Pass.SaveCustomerVehiclePass(objCustomerVehiclePass);
                    if (objResult.CustomerVehiclePassID != 0)
                    {
                        resultmsg = "Success";
                    }
                    else
                    {

                        resultmsg = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        [HttpGet]
        [ActionName("getFBSaveMultiStationCustomerVehiclePass")]
        public string getFBSaveMultiStationCustomerVehiclePass(string id)
        {

            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            DALPass dal_Pass = new DALPass();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {

                List<CustomerVehiclePass> lstMultiPass = null;
                int resultID = 0;
                VMMultiStationCustomerVehiclePass objvmCustomerVehiclePass = null;
                objvmCustomerVehiclePass = Task.Run(async () => await
                                                                objFBHelper.Get_FBMultiStationCustomerVehiclePassDetails(id)
                                                              ).Result;
                if (objvmCustomerVehiclePass.LocationID.Count > 0)
                {
                    for (int i = 0; i < objvmCustomerVehiclePass.LocationID.Count; i++)
                    {
                        objvmCustomerVehiclePass.CustomerVehiclePassID.LocationID.LocationID = objvmCustomerVehiclePass.LocationID[i].LocationID;
                        if (i == 0)
                        {
                            objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = 0;
                            resultID = dal_Pass.SaveCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                            resultmsg = "Success";
                        }
                        else
                        {

                            objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = resultID;
                            dal_Pass.SaveCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                            resultmsg = "Success";
                        }



                    }
                    lstMultiPass = dal_Pass.GetCustomerVehiclePassesByVehicle(objvmCustomerVehiclePass.CustomerVehiclePassID.CustomerVehicleID.RegistrationNumber);
                }

            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }


        [HttpGet]
        [ActionName("getFBUpdateCustomerVehiclePassDetails")]
        public string getFBUpdateCustomerVehiclePassDetails(string id)
        {
            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            DALPass dal_Pass = new DALPass();
            FBHelper objFBHelper = new FBHelper();
            CustomerVehiclePass objCustomerVehiclePass;
            string resultmsg = string.Empty;
            try
            {
                objCustomerVehiclePass = Task.Run(async () => await
                                                                objFBHelper.Get_FBCustomerVehiclePassDetails(id)
                                                              ).Result;
                if (objCustomerVehiclePass != null)
                {
                    if (objCustomerVehiclePass.IsActivateNFCCard)
                    {
                        if (objCustomerVehiclePass.NFCCardActivatedByID != null)  // NFC CARD  ACTIVATE
                        {
                            if (objCustomerVehiclePass.NFCCardActivatedByID.UserID != 0)
                            {
                                CustomerVehiclePass objResult = dal_Pass.FBActivateCustomerVehiclePass(objCustomerVehiclePass);
                                if (objResult.CustomerVehiclePassID != 0)
                                {
                                    resultmsg = "NFC Card Activation Success";
                                }
                                else
                                {

                                    resultmsg = "NFC Card Activation Fail";
                                }
                            }
                        }
                    }
                    if (objCustomerVehiclePass.IsNewNFC)
                    {
                        if (objCustomerVehiclePass.NFCCardSoldByID != null) //New NFC Card Saving
                        {
                            if (objCustomerVehiclePass.NFCCardSoldByID.UserID != 0)
                            {
                                resultmsg = dal_Pass.FBSaveCustomerVehiclePassNFCCard(objCustomerVehiclePass);

                            }
                        }
                    }
                    if (objCustomerVehiclePass.IsRenewPass)  // PASS RENEW
                    {
                        CustomerVehiclePass objResult = dal_Pass.FBUpdateCustomerVehiclePass(objCustomerVehiclePass);
                        if (objResult.CustomerVehiclePassID != 0)
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

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        [HttpGet]
        [ActionName("getFBUpdateMultiStationCustomerVehiclePass")]
        public string getFBUpdateMultiStationCustomerVehiclePass(string id)
        {

            DALExceptionManagment objExceptionlog = new DALExceptionManagment();
            DALPass dal_Pass = new DALPass();
            FBHelper objFBHelper = new FBHelper();
            string resultmsg = string.Empty;
            try
            {

                List<CustomerVehiclePass> lstMultiPass = null;
                int resultID = 0;
                VMMultiStationCustomerVehiclePass objvmCustomerVehiclePass = null;
                objvmCustomerVehiclePass = Task.Run(async () => await
                                                                objFBHelper.Get_FBMultiStationCustomerVehiclePassDetails(id)
                                                              ).Result;

                if (objvmCustomerVehiclePass.CustomerVehiclePassID.NFCCardActivatedByID != null)
                {
                    if (objvmCustomerVehiclePass.CustomerVehiclePassID.NFCCardActivatedByID.UserID != 0)
                    {
                        CustomerVehiclePass objResult = dal_Pass.FBActivateCustomerVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                        if (objResult.CustomerVehiclePassID != 0)
                        {
                            resultmsg = "Success";
                        }
                        else
                        {

                            resultmsg = "Fail";
                        }
                    }


                }
                else
                {
                    if (objvmCustomerVehiclePass.LocationID.Count > 0)
                    {
                        for (int i = 0; i < objvmCustomerVehiclePass.LocationID.Count; i++)
                        {
                            objvmCustomerVehiclePass.CustomerVehiclePassID.LocationID.LocationID = objvmCustomerVehiclePass.LocationID[i].LocationID;
                            if (i == 0)
                            {
                                objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = 0;
                                resultID = dal_Pass.FBUpdateCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                                if (resultID != 0)
                                {
                                    resultmsg = "Success";
                                }
                            }
                            else
                            {

                                objvmCustomerVehiclePass.CustomerVehiclePassID.PrimaryLocationParkingLotID.LocationParkingLotID = resultID;
                                dal_Pass.FBUpdateCustomerMultiVehiclePass(objvmCustomerVehiclePass.CustomerVehiclePassID);
                                resultmsg = "Success";
                            }
                        }
                        lstMultiPass = dal_Pass.GetCustomerVehiclePassesByVehicle(objvmCustomerVehiclePass.CustomerVehiclePassID.CustomerVehicleID.RegistrationNumber);


                    }
                }


            }
            catch (Exception ex)
            {

                resultmsg = ex.Message;
            }
            return resultmsg;
        }

        #endregion

        #region Firebase API Calls Using For Fetch Reocords Slow Issue 

        [HttpGet]
        [ActionName("getFBGetVehicleParkingFee")]
        public List<VehicleParkingFee> FBGet_VehicleParkingFee(int LocationParkingLotID, string VehicleTypeCode, int Hours)
        {
            List<VehicleParkingFee> objlotprice = new List<VehicleParkingFee>();
            var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
            try
            {
                var pricelist = Task.Run(async () =>
                                             (await firebase.Child("Price").OnceAsync<FB_Price>())
                                             .Select(data => new FB_Price()
                                             {
                                                 ApplicationTypeCode = data.Object.ApplicationTypeCode,
                                                 LocationParkingLotID = data.Object.LocationParkingLotID,
                                                 VehicleTypeCode = data.Object.VehicleTypeCode,
                                                 Duration = data.Object.Duration,
                                                 Price = data.Object.Price
                                             })
                                          .Where(L => L.ApplicationTypeCode == "O"
                                       && L.LocationParkingLotID == LocationParkingLotID
                                       && L.VehicleTypeCode == VehicleTypeCode
                                       && L.Duration == Hours).ToList()
                                                ).Result;
                if (pricelist != null && pricelist.Count > 0)
                {
                    objlotprice = pricelist.Select(item => new VehicleParkingFee
                    {
                        IsFullDay = item.Duration == 6 ? true : false,
                        DayOfWeek = DateTime.Now.DayOfWeek.ToString(),
                        Fees = (decimal)item.Price,
                        VehicleTypeCode = item.VehicleTypeCode,
                        Duration = item.Duration,
                        ParkingHours = item.Duration,
                        LocationParkingLotID = item.LocationParkingLotID,
                    }).ToList();
                }




            }
            catch (Exception ex)
            {

            }
            return objlotprice;
        }

        [HttpGet]
        [ActionName("getFBAllLotParkedVehicles")]
        public List<LocationLotParkedVehicles> FBGet_AllLotParkedVehicles(int LocationParkingLotID)
        {
            List<LocationLotParkedVehicles> lstFBLocationLotParkedVehicles = new List<LocationLotParkedVehicles>();
            try
            {

                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var parkedVehicles = Task.Run(async () => (await firebase.Child("CustomerParkingSlot").OnceAsync<FBCustomerParkingSlot>())
                                                           .Where(L => L.Object.LocationParkingLotID.LocationParkingLotID == LocationParkingLotID
                                                                  ).OrderByDescending(R => R.Object.CreatedOn).ToList()
                                              ).Result;

                if (parkedVehicles.Count > 0)
                {
                    lstFBLocationLotParkedVehicles = parkedVehicles.Select(item => new LocationLotParkedVehicles
                    {
                        CustomerParkingSlotID = item.Object.CustomerParkingSlotID,
                        FBCustomerParkingSlotKey = item.Key,
                        VehicleImage = item.Object.VehicleTypeImage,
                        RegistrationNumber = item.Object.RegistrationNumber,
                        ParkingBayRange = item.Object.ParkingBayRange,
                        BayNumberColor = item.Object.BayNumberColor,
                        VehicleStatusColor = item.Object.VehicleStatusColor,
                        ApplicationTypeCode = item.Object.ApplicationTypeID.ApplicationTypeCode,
                        StatusCode = item.Object.StatusID.StatusCode,
                        VehicleClampImage = item.Object.StatusID.StatusCode.ToUpper() == "CHKIN".ToUpper() ? (item.Object.IsClamp ? "clamp.png" : item.Object.VehicleClampImage) : item.Object.VehicleClampImage,
                        LocationID = item.Object.LocationParkingLotID.LocationID.LocationID,
                        LocationParkingLotID = item.Object.LocationParkingLotID.LocationParkingLotID,
                        VehicleTypeID = item.Object.VehicleTypeID.VehicleTypeID,
                        VehicleTypeCode = item.Object.VehicleTypeID.VehicleTypeCode,
                        CreatedOn = item.Object.CreatedOn,
                        UpdatedOn = item.Object.UpdatedOn
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return lstFBLocationLotParkedVehicles;
        }

        [HttpGet]
        [ActionName("getFBParkedVehicleDetails")]
        public FBCustomerParkingSlot FBGet_ParkedVehicleDetails(string id)
        {
            FBCustomerParkingSlot objCustomerParkingSlot = new FBCustomerParkingSlot();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var ParkedVehicles = Task.Run(async () => (await firebase.Child("CustomerParkingSlot").OnceAsync<FBCustomerParkingSlot>())
                                                          .Where(item => item.Key == id)
                                                          .SingleOrDefault()
                                                            ).Result;
                if (ParkedVehicles != null)
                {
                    objCustomerParkingSlot = ParkedVehicles.Object;
                    objCustomerParkingSlot.FBCustomerParkingSlotKey = id;
                }

                VehicleViolationWarning objVehicleViolationWarning = new VehicleViolationWarning();
                objVehicleViolationWarning.RegistrationNumber = objCustomerParkingSlot.CustomerVehicleID.RegistrationNumber;
                objVehicleViolationWarning.VehicleTypeID.VehicleTypeCode = objCustomerParkingSlot.VehicleTypeID.VehicleTypeCode;
                int warnings = Task.Run(async () =>
                                           await GetVehicleWaringCount(objVehicleViolationWarning)
                                         ).Result;
                objCustomerParkingSlot.ViolationWarningCount = warnings;
            }
            catch (Exception ex)
            {

            }
            return objCustomerParkingSlot;
        }
        public async Task<int> GetVehicleWaringCount(VehicleViolationWarning objVehicle)
        {
            int Warnings = 0;
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var ParkedVehicles = (await firebase.Child("VehicleViolationWarning").OnceAsync<VehicleViolationWarning>()).Where(item => item.Object.RegistrationNumber == objVehicle.RegistrationNumber && item.Object.VehicleTypeID.VehicleTypeCode == objVehicle.VehicleTypeID.VehicleTypeCode).SingleOrDefault();
                if (ParkedVehicles != null)
                {
                    Warnings = ParkedVehicles.Object.WarningCount;
                }
            }
            catch (Exception ex)
            {
            }
            return Warnings;
        }


        #region Update Vehicle Parking Status To Overstay (Except Pass vehicles)
        public async void UpdateFBCustomerParkingSlot()
        {
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var normalCheckInRecords = (await firebase
                                            .Child("CustomerParkingSlot")
                                            .OnceAsync<FBCustomerParkingSlot>()).Where(BO => (Convert.ToDateTime(BO.Object.ActualEndTime) <= DateTime.Now) && (BO.Object.StatusID.StatusCode == "CHKIN") && (BO.Object.ApplicationTypeID.ApplicationTypeCode != "P")).ToList();
                if (normalCheckInRecords.Count > 0)
                {
                    for (var data = 0; data < normalCheckInRecords.Count; data++)
                    {
                        var updatedata = normalCheckInRecords[data];
                        var updateRecord = new FBCustomerParkingSlot();
                        updateRecord = updatedata.Object;
                        updateRecord.StatusID.StatusCode = "O";
                        switch (updatedata.Object.VehicleTypeID.VehicleTypeCode)
                        {
                            case "2W":
                                updateRecord.VehicleTypeImage = "bike_orange.png";
                                break;
                            case "4W":
                                updateRecord.VehicleTypeImage = "car_orange.png";
                                break;
                        }
                        updateRecord.BayNumberColor = "#F39C12";
                        updateRecord.VehicleStatusColor = "#F39C12";
                        updateRecord.VehicleClampImage = "clock_orange.png";
                        if (updatedata.Object.IsVehicleClamp)
                        {
                            updateRecord.VehicleClampImage = "clamp.png";
                        }
                        updateRecord.UpdatedOn = DateTime.Now;
                        await firebase
                            .Child("CustomerParkingSlot")
                            .Child(normalCheckInRecords[data].Key).PutAsync(updateRecord);
                    }
                }

                UpdateFBPassCustomerParkingSlot();
            }
            catch (Exception ex)
            {

            }

        }
        public async void UpdateFBPassCustomerParkingSlot()
        {
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var normalCheckInRecords = (await firebase
                                            .Child("CustomerParkingSlot")
                                            .OnceAsync<FBCustomerParkingSlot>()).Where(BO => (BO.Object.StatusID.StatusCode == "CHKIN" || BO.Object.StatusID.StatusCode == "O") && (BO.Object.ApplicationTypeID.ApplicationTypeCode == "P")).ToList();
                if (normalCheckInRecords.Count > 0)
                {
                    for (var data = 0; data < normalCheckInRecords.Count; data++)
                    {
                        var updatedata = normalCheckInRecords[data];
                        FBCustomerParkingSlot passRecord = new FBCustomerParkingSlot();
                        passRecord = updatedata.Object;

                        string RegistrationNumber = passRecord.CustomerVehicleID.RegistrationNumber;

                        // Verify Is Pass Date Expire

                        VMMultiStationCustomerVehiclePass objPassVehicleDetails = Task.Run(async () =>
                                                           await FBGet_VehiclePassDetails(RegistrationNumber)
                                                            ).Result;
                        if (objPassVehicleDetails.CustomerVehiclePassID.CustomerVehicleID != null)
                        {

                            if (!string.IsNullOrEmpty(RegistrationNumber))
                            {
                                DateTime passExpiryDate = Convert.ToDateTime(objPassVehicleDetails.CustomerVehiclePassID.ExpiryDate);
                                DateTime ExpectedEndTime = Convert.ToDateTime(passRecord.ExpectedEndTime);
                                if (DateTime.Now.Date > passExpiryDate.Date)
                                {
                                    passRecord.StatusID.StatusCode = "O";
                                    switch (updatedata.Object.VehicleTypeID.VehicleTypeCode)
                                    {
                                        case "2W":
                                            passRecord.VehicleTypeImage = "bike_orange.png";
                                            break;
                                        case "4W":
                                            passRecord.VehicleTypeImage = "car_orange.png";
                                            break;
                                    }
                                    passRecord.BayNumberColor = "#F39C12";
                                    passRecord.VehicleStatusColor = "#F39C12";
                                    passRecord.VehicleClampImage = "clock_orange.png";
                                    if (updatedata.Object.IsVehicleClamp)
                                    {
                                        passRecord.VehicleClampImage = "clamp.png";
                                    }
                                    passRecord.UpdatedOn = DateTime.Now;
                                    await firebase
                                        .Child("CustomerParkingSlot")
                                        .Child(updatedata.Key).PutAsync(passRecord);
                                }  // IF Expire
                                else
                                {


                                    if (ExpectedEndTime.Date < DateTime.Now.Date)
                                    {
                                        passRecord.ExpectedEndTime = ExpectedEndTime.AddDays(1);
                                        passRecord.ActualEndTime = ExpectedEndTime.AddDays(1);
                                        passRecord.StatusID.StatusCode = "CHKIN";
                                        switch (updatedata.Object.VehicleTypeID.VehicleTypeCode)
                                        {
                                            case "2W":
                                                passRecord.VehicleTypeImage = "bike_black.png";
                                                break;
                                            case "4W":
                                                passRecord.VehicleTypeImage = "car_black.png";
                                                break;
                                        }
                                        passRecord.BayNumberColor = "#444444";
                                        passRecord.VehicleStatusColor = "#444444";
                                        if (updatedata.Object.IsVehicleClamp)
                                        {
                                            passRecord.VehicleClampImage = "clamp.png";
                                        }
                                        else
                                        {
                                            passRecord.VehicleClampImage = "";
                                        }
                                        passRecord.UpdatedOn = DateTime.Now;
                                        await firebase
                                            .Child("CustomerParkingSlot")
                                            .Child(updatedata.Key).PutAsync(passRecord);
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public async Task<VMMultiStationCustomerVehiclePass> FBGet_VehiclePassDetails(string RegistrationNumber)
        {

            VMMultiStationCustomerVehiclePass objVMMultiStationCustomerVehiclePass = new VMMultiStationCustomerVehiclePass();

            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var multipass = (await firebase.Child("VMMultiStationCustomerVehiclePass").OnceAsync<VMMultiStationCustomerVehiclePass>())
                                                      .Where(item => item.Object.CustomerVehiclePassID.CustomerVehicleID.RegistrationNumber == RegistrationNumber)
                                                      .SingleOrDefault();
                if (multipass != null)
                {
                    objVMMultiStationCustomerVehiclePass = multipass.Object;
                }
                else
                {
                    var singlePass = (await firebase.Child("CustomerVehiclePass").OnceAsync<CustomerVehiclePass>())
                                                  .Where(item => item.Object.CustomerVehicleID.RegistrationNumber == RegistrationNumber)
                                                  .SingleOrDefault();
                    if (singlePass != null)
                    {
                        objVMMultiStationCustomerVehiclePass.CustomerVehiclePassID = singlePass.Object;
                    }

                }


            }
            catch (Exception ex)
            {

            }
            return objVMMultiStationCustomerVehiclePass;
        }
        #endregion

        [HttpGet]
        [ActionName("getFBPriceDetails")]
        public FB_Price FBGet_PriceDetails(int LocationParkingLotID, string VehicleTypeCode, int Hours)
        {
            FB_Price fbPriceDetails = new FB_Price();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var PriceDetails = Task.Run(async () => (await firebase.Child("Price").OnceAsync<FB_Price>())
                                .Select(data => new FB_Price()
                                {
                                    ApplicationTypeCode = data.Object.ApplicationTypeCode,
                                    LocationParkingLotID = data.Object.LocationParkingLotID,
                                    VehicleTypeCode = data.Object.VehicleTypeCode,
                                    Duration = data.Object.Duration,
                                    Price = data.Object.Price

                                })
                                .Where(L => L.ApplicationTypeCode == "O"
                                       && L.LocationParkingLotID == LocationParkingLotID
                                       && L.VehicleTypeCode == VehicleTypeCode
                                       && L.Duration == Hours)
                                .SingleOrDefault()
                               ).Result;

                if (PriceDetails != null)
                {
                    fbPriceDetails = PriceDetails;
                }

            }
            catch (Exception ex)
            {

            }
            return fbPriceDetails;
        }


        [HttpGet]
        [ActionName("getFBLocationLotParkingBayDetails")]
        public List<ParkingBay> FBGetLocationLotParkingBayDetails(int LocationParkingLotID, string VehicleTypeCode)
        {
            List<ParkingBay> lstParkingBay = new List<ParkingBay>();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");

                var Bay = Task.Run(async () =>
                               (await firebase.Child("ParkingBay").OnceAsync<FB_ParkingBay>())
                               .Where(L => L.Object.LocationParkingLotID.LocationParkingLotID == LocationParkingLotID && L.Object.VehicleTypeID.VehicleTypeCode == VehicleTypeCode
                                      && L.Object.IsActive == true && L.Object.ParkingBayID != 0).ToList()
                                      ).Result;

                    if (Bay != null && Bay.Count > 0)
                    {
                        lstParkingBay = Bay.Select(item => new ParkingBay
                        {
                            ParkingBayID = item.Object.ParkingBayID,
                            ParkingBayRange = item.Object.ParkingBayRange,
                            LocationParkingLotID = item.Object.LocationParkingLotID.LocationParkingLotID,
                            VehicleTypeID = new VehicleType() { VehicleTypeID = item.Object.VehicleTypeID.VehicleTypeID, VehicleTypeCode = item.Object.VehicleTypeID.VehicleTypeCode },
                            IsActive = item.Object.IsActive
                        }).ToList();
                    }



                



            }
            catch (Exception ex)
            {

            }
            return lstParkingBay;
        }

        [HttpGet]
        [ActionName("getFBVerifyVehicleIsAlredayIn")]
        public APIResponse FBVerifyVehicleIsAlredayIn(string RegistrationNumber)
        {
            APIResponse ObjAPIResponse = new APIResponse();
            string rstMsg = string.Empty;
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var checkInRecords = Task.Run(async () => (await firebase
                                            .Child("CustomerParkingSlot")
                                            .OnceAsync<FBCustomerParkingSlot>())
                                            .Where(BO => (BO.Object.RegistrationNumber == RegistrationNumber)
                                                   && (BO.Object.StatusID.StatusCode.ToUpper() != "FOC" && BO.Object.StatusID.StatusCode.ToUpper() != "CHKOUT"))
                                            .Select(item => new FBCustomerParkingSlot()
                                            {
                                                LocationParkingLotID = item.Object.LocationParkingLotID,
                                                RegistrationNumber = item.Object.RegistrationNumber,
                                                StatusID = item.Object.StatusID
                                            })
                                            .ToList()).Result;
                if (checkInRecords != null && checkInRecords.Count > 0)
                {
                    var normalCheckInRecordsKey = checkInRecords[0];
                    if (normalCheckInRecordsKey != null)
                    {
                        ObjAPIResponse.Object = null;
                        rstMsg = normalCheckInRecordsKey.LocationParkingLotID.LocationID.LocationName + " " + normalCheckInRecordsKey.LocationParkingLotID.LocationParkingLotName;
                        if(!string.IsNullOrEmpty(rstMsg))
                        {
                            ObjAPIResponse.Result = true;
                            ObjAPIResponse.Message = rstMsg;
                        }
                        else
                        {
                            ObjAPIResponse.Result = false;
                            ObjAPIResponse.Message = "No Result Found";
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return ObjAPIResponse;
        }
        
        [HttpGet]
        [ActionName("getFBVehiclePassDetails")]
        public CustomerVehiclePass FBGetVehiclePassDetails(string RegistrationNumber, string NFCCardNumber)
        {
            CustomerVehiclePass objCustomerVehiclePass = new CustomerVehiclePass();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                VMMultiStationCustomerVehiclePass objVMMultiStationCustomerVehiclePass = new VMMultiStationCustomerVehiclePass();

                if (RegistrationNumber != string.Empty && RegistrationNumber != "")
                {
                    var multipass = Task.Run(async () => (await firebase.Child("VMMultiStationCustomerVehiclePass").OnceAsync<VMMultiStationCustomerVehiclePass>())
                                                 .Where(item => item.Object.CustomerVehiclePassID.CustomerVehicleID.RegistrationNumber == RegistrationNumber)
                                                 .SingleOrDefault()
                                                 ).Result;
                    if (multipass != null)
                    {
                        objVMMultiStationCustomerVehiclePass = multipass.Object;
                    }
                    else
                    {
                        var singlePass = Task.Run(async () => (await firebase.Child("CustomerVehiclePass").OnceAsync<CustomerVehiclePass>())
                                                      .Where(item => item.Object.CustomerVehicleID.RegistrationNumber == RegistrationNumber)
                                                      .SingleOrDefault()
                                                      ).Result;
                        if (singlePass != null)
                        {
                            objVMMultiStationCustomerVehiclePass.CustomerVehiclePassID = singlePass.Object;
                        }

                    }

                }
                if (NFCCardNumber != string.Empty && NFCCardNumber != "")
                {
                    var multipass = Task.Run(async () => (await firebase.Child("VMMultiStationCustomerVehiclePass").OnceAsync<VMMultiStationCustomerVehiclePass>())
                                                 .Where(item => item.Object.CustomerVehiclePassID.CardNumber == NFCCardNumber)
                                                 .SingleOrDefault()
                                                 ).Result;
                    if (multipass != null)
                    {
                        objVMMultiStationCustomerVehiclePass = multipass.Object;
                    }
                    else
                    {
                        var singlePass = Task.Run(async () => (await firebase.Child("CustomerVehiclePass").OnceAsync<CustomerVehiclePass>())
                                                      .Where(item => item.Object.CardNumber == NFCCardNumber)
                                                      .SingleOrDefault()
                                                  ).Result;
                        if (singlePass != null)
                        {
                            objVMMultiStationCustomerVehiclePass.CustomerVehiclePassID = singlePass.Object;
                        }

                    }

                }

                if (objVMMultiStationCustomerVehiclePass != null)
                {
                    objCustomerVehiclePass = objVMMultiStationCustomerVehiclePass.CustomerVehiclePassID;

                }
            }
            catch (Exception ex)
            {
            }
            return objCustomerVehiclePass;

        }

        [HttpGet]
        [ActionName("getFBUserLocationMapper")]
        public List<FB_UserLocationMapper> FBGetUserLocationMapper(int UserID, string UserTypeName, int LocationID, int LocationParkingLotID)
        {
            List<FB_UserLocationMapper> lstFB_UserLocationMapper = new List<FB_UserLocationMapper>();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var LocationMapper = Task.Run(async () => (await firebase.Child("UserLocationMapper").OnceAsync<FB_UserLocationMapper>()).Where(L => L.Object.UserID.UserID == UserID && L.Object.IsActive == true)
                                          .OrderByDescending(d => d.Object.LotID.LocationParkingLotName).ToList()
                                          ).Result;

                    if (LocationMapper.Count > 0)
                    {
                        if (UserTypeName.ToUpper() == "OPERATOR".ToUpper())
                        {
                            lstFB_UserLocationMapper = LocationMapper.Select(item => new FB_UserLocationMapper
                            {
                                LotID = item.Object.LotID,
                                LocationID = item.Object.LocationID,
                                IsActive = item.Object.IsActive,
                                UserID = item.Object.UserID
                            }).Where(L => L.LocationID.LocationID == LocationID && L.LotID.LocationParkingLotID == LocationParkingLotID && L.IsActive == true).ToList();
                        }
                        else
                        {
                            lstFB_UserLocationMapper = LocationMapper.Select(item => new FB_UserLocationMapper
                            {
                                LotID = item.Object.LotID,
                                LocationID = item.Object.LocationID,
                                IsActive = item.Object.IsActive,
                                UserID = item.Object.UserID
                            }).ToList();
                        }
                    }

            }
            catch (Exception ex)
            {

            }
            return lstFB_UserLocationMapper;
        }

        [HttpGet]
        [ActionName("getFBParkingLotTiming")]
        public FB_ParkingLotTiming FBGetParkingLotTiming(int LocationParkingLotID)
        {
     
            FB_ParkingLotTiming objlottiming = new FB_ParkingLotTiming();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                var LotTiming = Task.Run(async () => (await firebase.Child("ParkingLotTiming").OnceAsync<FB_ParkingLotTiming>()
                                              ).Where(L => L.Object.LotID.LocationParkingLotID == LocationParkingLotID
                                                           && L.Object.DayOfWeek.ToUpper() == DateTime.Today.DayOfWeek.ToString().ToUpper()
                                                           && L.Object.IsActive == true).SingleOrDefault()
                                           ).Result;
                    if (LotTiming != null)
                    {
                        objlottiming = LotTiming.Object;
                    }

               


            }
            catch (Exception ex)
            {

            }
            return objlottiming;
        }
      
        [HttpGet]
        [ActionName("getFBParkingLotTimingByDayName")]
        public FB_ParkingLotTiming FBGet_ParkingLotTimingByDayName(int LocationParkingLotID, string DayOfWeek)
        {
            FB_ParkingLotTiming objlottiming = new FB_ParkingLotTiming();
            try
            {
                var firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/");
                objlottiming = Task.Run(async () => (await firebase.Child("ParkingLotTiming").OnceAsync<FB_ParkingLotTiming>())
                                           .Select(item => new FB_ParkingLotTiming
                                           {
                                               LotID = item.Object.LotID,
                                               IsActive = item.Object.IsActive,
                                               DayOfWeek = item.Object.DayOfWeek,
                                               LotCloseTime = item.Object.LotCloseTime,
                                               LotOpenTime = item.Object.LotOpenTime
                                           }).Where(L => L.LotID.LocationParkingLotID == LocationParkingLotID && L.IsActive == true && L.DayOfWeek.ToUpper() == DayOfWeek.ToUpper())
                                           .SingleOrDefault()
                                           ).Result;
                                         

            }
            catch (Exception ex)
            {

            }
            return objlottiming;
        }
        #endregion

    }
}
