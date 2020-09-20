using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.APIOutPutModel
{
    public class VehicleParkingHistorySummary
    {
        public VehicleParkingHistorySummary()
        {

            CustomerVehicleID = new CustomerVehicle();
            CustomerID = new Customer();
            LocationParkingLotID = new LocationParkingLot();
            PaymentTypeID = new PaymentType();
        }
        public CustomerVehicle CustomerVehicleID { get; set; }
        public Customer CustomerID { get; set; }
        public LocationParkingLot LocationParkingLotID { get; set; }
        public decimal ParkingFees { get; set; }
        public string ParkingStartTime { get; set; }
        public string ParkingEndTime { get; set; }
        public User OperatorID { get; set; }
        public PaymentType PaymentTypeID { get; set; }
        public string ParkingTimmingsTextColor { get; set; }
    }
}