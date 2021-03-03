using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_LotVehicleAvailability
    {
        public int LotVehicleAvailabilityID { get; set; }
        public string LotVehicleAvailabilityCode { get; set; }
        public string LotVehicleAvailabilityName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}