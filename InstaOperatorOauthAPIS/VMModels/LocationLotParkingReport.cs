using InstaOperatorOauthAPIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class LocationLotParkingReport
    {
        public string LotTotalCheckIn { get; set; }
        public string LotTotalCheckOut { get; set; }
        public string LotTotalFOC { get; set; }
        public string LotRevenueCash { get; set; }
        public string LotRevenueEpay { get; set; }
        public ObservableCollection<LotParkingReport> LotParkingReportList { get; set; }
    }

}