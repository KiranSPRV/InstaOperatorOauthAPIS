using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_VehicleType
    {
        public int VehicleTypeID { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleTypeDesc { get; set; }
        public int WheelCount { get; set; }
        public int AxleCount { get; set; }
        public string VehicleTypeIcon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}