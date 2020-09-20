using InstaOperatorOauthAPIS.Models;
using System.Collections.ObjectModel;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLocationLotParkingReport
    {

        public VMLocationLotParkingReport()
        {
            LotParkingReportList = new ObservableCollection<LotParkingReport>();
        }
        public string LotTotalCheckIn { get; set; }
        public string LotTotalCheckOut { get; set; }
        public string LotTotalFOC { get; set; }
        public string LotRevenueCash { get; set; }
        public string LotRevenueEpay { get; set; }
        public ObservableCollection<LotParkingReport> LotParkingReportList { get;set;}
    }
}