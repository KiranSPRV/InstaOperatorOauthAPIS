using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstaOperatorOauthAPIS.Models;
using InstaOperatorOauthAPIS.Models.APIOutPutModel;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLoginVerification
    {

        public User LoginVerification(string UserID, string Password)
        {
            User objUser = new User();
            try
            {
                objUser.UserID =1;
                objUser.UserName = "Kiran";
                objUser.Password = "1234";
            }
            catch (Exception ex)
            {
            }
            return objUser;
        }

    }
}