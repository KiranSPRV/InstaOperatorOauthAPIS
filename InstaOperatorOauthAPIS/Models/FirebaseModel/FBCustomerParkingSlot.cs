
using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FBCustomerParkingSlot
    {
        public FBCustomerParkingSlot()
        {
            LocationParkingLotID = new LocationParkingLot();
            VehicleTypeID = new VehicleType();
            StatusID = new Status();
            ApplicationTypeID = new ApplicationType();
            CreatedBy = new User();
            UpdatedBy = new User();
            PaymentTypeID = new PaymentType();
            CustomerVehicleID = new CustomerVehicle();
            ViolationReasonID = new ViolationReason();
            FOCReasonID = new ViolationReason();
        }
        public int CustomerParkingSlotID { get; set; }
        public string FBCustomerParkingSlotKey { get; set; }
        public string RegistrationNumber { get; set; }
        public string ParkingBayRange { get; set; }
        public string ParkingBayName { get; set; }
        public int ParkingBayID { get; set; }
        public Status StatusID { get; set; }
        public ApplicationType ApplicationTypeID { get; set; }
        public PaymentType PaymentTypeID { get; set; }
        public int CustomerID { get; set; }
        public CustomerVehicle CustomerVehicleID { get; set; }
        public LocationParkingLot LocationParkingLotID { get; set; }
        public VehicleType VehicleTypeID { get; set; }
        public DateTime? ExpectedStartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string VehicleStatusColor { get; set; }
        public string BayNumberColor { get; set; }
        public string Duration { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal OverStayAmount { get; set; }
        public decimal ExtendAmount { get; set; }
        public decimal ViolationFees { get; set; }

        public string VehicleTypeImage { get; set; }

        public bool IsVehicleClamp { get; set; }
        public string VehicleClampImage { get; set; }

        public User CreatedBy { get; }
        public DateTime CreatedOn { get; set; }
        public User UpdatedBy { get; }
        public DateTime UpdatedOn { get; set; }

        public byte[] ViolationImage { get; set; }
        public byte[] GovernmentVehicleImage { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsClamp { get; set; }
        public bool IsWarning { get; set; }
        public int ReasonID { get; set; }
        public string ReasonName { get; set; }
        public DateTime ViolationStartTime { get; set; }
        public string ViolationTime { get; set; }
        public decimal VehicleImageLottitude { get; set; }
        public decimal VehicleImageLongitude { get; set; }

        public int ViolationWarningCount { get; set; }
        public decimal ClampFees { get; set; }

        public ViolationReason ViolationReasonID { get; set; }

        public ViolationReason FOCReasonID { get; set; }
        public bool IsVehicleClampUpdated { get; set; }
        public bool IsNFCCheckIn { get; set; }
    }
}
