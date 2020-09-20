using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models
{
    public class LocationLotOperations
    {
        public int LocationLotID { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string FOC { get; set; }
        public string ParkingHours { get; set; }
        public string Cash { get; set; }
        public string Epay { get; set; }
    }
}