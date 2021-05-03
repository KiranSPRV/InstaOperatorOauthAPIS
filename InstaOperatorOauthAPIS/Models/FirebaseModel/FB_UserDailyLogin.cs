using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_UserDailyLogin
    {
        public int UserDailyLoginID { get; set; }
        public FB_User UserID { get; set; }
        public FB_Lot LocationParkingLotID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public int NoofHours { get; set; }
        public string LoginDeviceID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public FB_User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public FB_User UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
