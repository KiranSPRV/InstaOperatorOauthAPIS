using InstaOperatorOauthAPIS.Models.Reports;
using System.Collections.Generic;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLocationLotViolations
    {
        public VMLocationLotViolations()
        {
        }
        public int TotalClamp { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalEPay { get; set; }
        public string Currency { get; set; }
        public List<StationClampedReport> LocationLotViolationReport { get; set; }
    }
}