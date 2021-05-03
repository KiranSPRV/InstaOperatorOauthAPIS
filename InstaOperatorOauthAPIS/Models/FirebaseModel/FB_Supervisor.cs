using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.FirebaseModel
{
   public class FB_Supervisor
    {
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOperator { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserCode { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
