using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
    public class FB_Location
    {
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationDesc { get; set; }
        public string Address { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}