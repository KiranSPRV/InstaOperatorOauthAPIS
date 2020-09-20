using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.APIOutPutModel
{
   public class LocationParkingLot
    {
        public LocationParkingLot()
        {
            LocationID = new Location();
            ParkingBayID = new ParkingBay();
        }
        public int LocationParkingLotID { get; set; }
        public Location LocationID { get; set; }
        public int ParkingTypeID { get; set; }
        public int VehicleTypeID { get; set; }
        public ParkingBay ParkingBayID { get; set; }
        public int ParentLocationParkingLotID { get; set; }
        public string LocationParkingLotCode { get; set; }
        public string LocationParkingLotName { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
