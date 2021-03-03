using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_UserLocationMapper
    {
        public int UserLocationMapperID { get; set; }
        public FB_User UserID { get; set; }
        public FB_Location LocationID { get; set; }
        public FB_Lot LotID { get; set; }
        public bool IsActive { get; set; }
    }
}