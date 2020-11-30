using InstaOperatorOauthAPIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLocationLotOccupancyReport
    {
        public VMLocationLotOccupancyReport()
        {
        }
        public string TotalTwoWheelerPercentage { get; set; }
        public string TotalFourWheelerPercentage { get; set; }
        public List<LocationLotOccupancyReport> LocationLotOccupancyReportID { get; set; }
    }
}