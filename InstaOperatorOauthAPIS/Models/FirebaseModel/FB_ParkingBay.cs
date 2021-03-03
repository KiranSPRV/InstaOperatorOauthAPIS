using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_ParkingBay
    {
        public int ParkingBayID { get; set; }
        public FB_Lot LocationParkingLotID { get; set; }
        public FB_VehicleType VehicleTypeID { get; set; }
        public int NumberOfBays { get; set; }
        public string ParkingBayRange { get; set; }
        public bool IsActive { get; set; }
    }
}