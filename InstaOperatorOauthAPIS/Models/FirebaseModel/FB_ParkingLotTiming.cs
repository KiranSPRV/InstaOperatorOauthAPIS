using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_ParkingLotTiming
    {
        public int ParkingLotTimingID { get; set; }
        public string DayOfWeek { get; set; }
        public bool IsActive { get; set; }
        public string LotCloseTime { get; set; }
        public FB_Lot LotID { get; set; }
        public string LotOpenTime { get; set; }
       
    }
}
