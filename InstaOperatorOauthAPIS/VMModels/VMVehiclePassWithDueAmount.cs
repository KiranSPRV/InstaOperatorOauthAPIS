using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMVehiclePassWithDueAmount
    {
        public CustomerVehiclePass CustomerVehiclePassID { get; set; }
        public decimal VehicleDueAmount { get; set; }
    }
}