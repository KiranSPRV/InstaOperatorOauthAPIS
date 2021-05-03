using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_Lot
    {
        public int LocationParkingLotID { get; set; }        
        public string LocationParkingLotCode { get; set; }
        public string LocationParkingLotName { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public FB_Location LocationID { get; set; }
        public FB_ParkingType ParkingTypeID { get; set; }
        public FB_LotVehicleAvailability LotVehicleAvailabilityID { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }        
        
    }
}