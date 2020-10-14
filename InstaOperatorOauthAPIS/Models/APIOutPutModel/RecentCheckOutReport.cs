using InstaOperatorOauthAPIS.VMModels;
using System.Collections.Generic;
namespace InstaOperatorOauthAPIS.Models.APIOutPutModel
{
    public class RecentCheckOutReport
    {
        public RecentCheckOutReport()
        {
            RecentCheckOutID = new List<VMRecentCheckOuts>();
        }
        public List<VMRecentCheckOuts> RecentCheckOutID { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalEpay { get; set; }
    }
}
