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
        public string TotalTwoWheelerLotCapacity { get; set; }
        public string TotalThreeWheelerLotCapacity { get; set; }
        public string TotalFourWheelerLotCapacity { get; set; }
        public string TotalHeavyWheelerLotCapacity { get; set; }
        public string TotalTwoWheelerPercentage { get; set; }
        public string TotalThreeWheelerPercentage { get; set; }
        public string TotalFourWheelerPercentage { get; set; }
        public string TotalHeavyWheelerPercentage { get; set; }
        public List<LocationLotOccupancyReport> LocationLotOccupancyReportID { get; set; }
    }
}