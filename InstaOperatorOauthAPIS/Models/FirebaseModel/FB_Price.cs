using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
   public class FB_Price
    {
        public string ApplicationTypeCode { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public int LocationParkingLotID { get; set; }
        public double Price { get; set; }
        public int PriceID { get; set; }
        public string VehicleTypeCode { get; set; }
    }
}
