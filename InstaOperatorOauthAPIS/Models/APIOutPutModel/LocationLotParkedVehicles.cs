using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.APIOutPutModel
{
    public class LocationLotParkedVehicles
    {
        public int CustomerParkingSlotID { get; set; }

        public string FBCustomerParkingSlotKey { get; set; }
        public string VehicleImage { get; set; }
        public string RegistrationNumber { get; set; }
        public string ParkingBayName { get; set; }
        public string ParkingBayRange { get; set; }
        public string BayNumberColor { get; set; }
        public string ApplicationTypeCode { get; set; }
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string VehicleStatusColor { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleClampImage { get; set; }
        public bool IsClamp { get; set; }
        public int LocationID { get; set; }
        public int LocationParkingLotID { get; set; }
        public int VehicleTypeID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public byte[] VehicleIcon { get; set; }
    }
}