using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIInputModel;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.FirebaseModel;
using InstaOperatorOauthAPIS.VMModels;

namespace InstaOperatorOauthAPIS.Models
{
    public class FBHelper
    {
        FirebaseClient firebase = null;
        public FBHelper()
        {
            firebase = new FirebaseClient("https://parhed-qa-120649-ccec0.firebaseio.com/", new FirebaseOptions
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s)
            }
             );
        }
        public async Task<FBCustomerParkingSlot> GetAllLotParkedVehicles(string Key)
        {
            FBCustomerParkingSlot lstFBLocationLotParkedVehicles = new FBCustomerParkingSlot();

            try
            {
                var ParkedVehicles = (await firebase.Child("CustomerParkingSlot").OnceAsync<FBCustomerParkingSlot>()).ToList();
                lstFBLocationLotParkedVehicles = ParkedVehicles.Where(item => item.Key == Key).SingleOrDefault().Object;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstFBLocationLotParkedVehicles;
        }
        public async void UpdateWarningCountFirebaseCustomerParkingSlot(FBCustomerParkingSlot lstFBLocationLotParkedVehicles, string Key)
        {
            try
            {
                var ParkedVehicles = (await firebase.Child("CustomerParkingSlot").OnceAsync<FBCustomerParkingSlot>()).ToList();
                var resulCustomer = ParkedVehicles.Where(item => item.Key == Key).SingleOrDefault().Object;

                if (lstFBLocationLotParkedVehicles != null && lstFBLocationLotParkedVehicles.RegistrationNumber != "")
                {

                    resulCustomer.IsClamp = lstFBLocationLotParkedVehicles.IsClamp;
                    resulCustomer.IsWarning = lstFBLocationLotParkedVehicles.IsWarning;
                    resulCustomer.ViolationReasonID.ViolationReasonID = lstFBLocationLotParkedVehicles.ViolationReasonID.ViolationReasonID;
                    resulCustomer.UpdatedBy.UserID = lstFBLocationLotParkedVehicles.UpdatedBy.UserID;
                    resulCustomer.ViolationWarningCount = (lstFBLocationLotParkedVehicles.ViolationWarningCount + 1);
                    await firebase
                           .Child("CustomerParkingSlot")
                           .Child(lstFBLocationLotParkedVehicles.FBCustomerParkingSlotKey).PutAsync(resulCustomer);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async void InsertUpdateFirebaseVehicleViolationWarning(VehicleViolationWarning objVehicleViolationWarning, string fbCustomerparingslotKey)
        {
            string resultKey = string.Empty;
            int warnings = 1;
            try
            {
                var vehicleViolationWarning = (await firebase.Child("VehicleViolationWarning").OnceAsync<VehicleViolationWarning>()).ToList();
                if (vehicleViolationWarning != null)
                {
                    var resulCustomer = vehicleViolationWarning
                                                      .Where(item => item.Object.RegistrationNumber == objVehicleViolationWarning.RegistrationNumber
                                                                    && item.Object.VehicleTypeID.VehicleTypeCode == objVehicleViolationWarning.VehicleTypeID.VehicleTypeCode
                                                            )
                                                      .SingleOrDefault();
                    if (resulCustomer == null)
                    {
                        objVehicleViolationWarning.WarningCount = warnings;

                        var customerparigDb = await firebase.Child("VehicleViolationWarning").PostAsync<VehicleViolationWarning>(objVehicleViolationWarning);
                        resultKey = customerparigDb.Key == null ? string.Empty : Convert.ToString(customerparigDb.Key);
                    }
                    else
                    {
                        if (resulCustomer.Object.WarningCount < 3)
                        {
                            var updateRec = resulCustomer.Object;
                            warnings = resulCustomer.Object.WarningCount + 1;
                            updateRec.WarningCount = warnings;
                            updateRec.UpdatedBy = objVehicleViolationWarning.CreatedBy;
                            updateRec.UpdatedOn = DateTime.Now;

                            await firebase
                               .Child("VehicleViolationWarning")
                               .Child(resulCustomer.Key).PutAsync(updateRec);
                        }


                    }


                    var ParkedVehicles = (await firebase.Child("CustomerParkingSlot").OnceAsync<FBCustomerParkingSlot>()).Where(item => item.Key == fbCustomerparingslotKey).SingleOrDefault();
                    if (ParkedVehicles != null)
                    {
                        var resultCustomerParkingSlot = ParkedVehicles.Object;
                        resultCustomerParkingSlot.ViolationWarningCount = warnings;
                        resultCustomerParkingSlot.IsVehicleClampUpdated = true;
                        await firebase
                              .Child("CustomerParkingSlot")
                              .Child(fbCustomerparingslotKey).PutAsync(resultCustomerParkingSlot);
                    }

                }

            }
            catch (Exception ex)
            {

            }

        }
        public async Task<FB_UserDailyLogin> Get_FBUserDailyLogin(string Key)
        {
            string resultKey = string.Empty;
            FB_UserDailyLogin reustFB_UserDailyLogin = null;
            try
            {
                
                var objUserDailyLogin = (await firebase.Child("UserDailyLogin").OnceAsync<FB_UserDailyLogin>()).Where(item => item.Key == Key).SingleOrDefault();
                if (objUserDailyLogin != null)
                {
                    reustFB_UserDailyLogin = objUserDailyLogin.Object;
                }

            }
            catch (Exception ex)
            {

            }
            return reustFB_UserDailyLogin;
        }
        public async Task<FB_User> Get_FBUserDetails(string Key)
        {
            string resultKey = string.Empty;
            FB_User reustFB_User = null;
            try
            {

                var objUserDailyLogin = (await firebase.Child("User").OnceAsync<FB_User>()).Where(item => item.Key == Key).SingleOrDefault();
                if (objUserDailyLogin != null)
                {
                    reustFB_User = objUserDailyLogin.Object;
                }

            }
            catch (Exception ex)
            {

            }
            return reustFB_User;
        }

        #region Firebase Pass Functions
        public async Task<CustomerVehiclePass> Get_FBCustomerVehiclePassDetails(string Key)
        {
            string resultKey = string.Empty;
            CustomerVehiclePass objResultVehicle = new CustomerVehiclePass();
            try
            {

                var objUserDailyLogin = (await firebase.Child("CustomerVehiclePass").OnceAsync<CustomerVehiclePass>()).Where(item => item.Key == Key).SingleOrDefault();
                if (objUserDailyLogin != null)
                {
                    objResultVehicle = objUserDailyLogin.Object;
                }

            }
            catch (Exception ex)
            {

            }
            return objResultVehicle;
        }

        public async Task<VMMultiStationCustomerVehiclePass> Get_FBMultiStationCustomerVehiclePassDetails(string Key)
        {
            string resultKey = string.Empty;
            VMMultiStationCustomerVehiclePass objmspass = new VMMultiStationCustomerVehiclePass();
            try
            {

                var objUserDailyLogin = (await firebase.Child("VMMultiStationCustomerVehiclePass").OnceAsync<VMMultiStationCustomerVehiclePass>()).Where(item => item.Key == Key).SingleOrDefault();
                if (objUserDailyLogin != null)
                {
                    objmspass = objUserDailyLogin.Object;
                }

            }
            catch (Exception ex)
            {

            }
            return objmspass;
        }
        #endregion
    }
}