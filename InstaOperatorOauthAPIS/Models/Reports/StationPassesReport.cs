using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using InstaOperatorOauthAPIS.Models.Pass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.Reports
{
    public class StationPassesReport
    {
        public StationPassesReport()
        {
            VehicleTypeID = new VehicleType();
            PassID = new PassTypes();
        }
        public VehicleType VehicleTypeID { get; set; }
        public PassTypes PassID { get; set; }
        public int PassesSold { get; set; }
        public int NFCSold { get; set; }
        public decimal PassesCash { get; set; }
        public decimal PassesEPay { get; set; }
        public string Currency { get; set; }
    }
}