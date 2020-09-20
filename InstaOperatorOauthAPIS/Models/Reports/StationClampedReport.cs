using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.Reports
{
    public class StationClampedReport
    {
        public StationClampedReport()
        {
            VehicleTypeID = new VehicleType();
        }
        public VehicleType VehicleTypeID { get; set; }
        public int NoOfClamps { get; set; }
        public decimal Cash { get; set; }
        public decimal EPay { get; set; }
        public string Currency { get; set; }
    }
}