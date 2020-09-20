using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.VMModels
{
   public class VMMultiStationCustomerVehiclePass
    {
        public VMMultiStationCustomerVehiclePass()
        {
            CustomerVehiclePassID = new CustomerVehiclePass();
            LocationID = new List<Location>();
        }
        public CustomerVehiclePass CustomerVehiclePassID { get; set; }
        public List<Location> LocationID { get; set; }
    }
}
