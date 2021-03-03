using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_ParkingType
    {
        public int ParkingTypeID { get; set; }
        public string ParkingTypeCode { get; set; }
        public string ParkingTypeName { get; set; }
        public string ParkingTypeDesc { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}