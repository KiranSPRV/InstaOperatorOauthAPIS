using InstaOperatorOauthAPIS.Models.Reports;
using System.Collections.Generic;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLocationLotPassReport
    {
        public VMLocationLotPassReport()
        {
            StationPasses = new List<StationPassesReport>();
        }
        public List<StationPassesReport> StationPasses { get; set; }
        public int TotalSold { get; set; }
        public int TotalNFC { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalEPay { get; set; }
        public string Currency { get; set; }
    }
}