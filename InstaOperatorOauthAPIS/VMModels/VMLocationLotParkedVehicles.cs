using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System.Collections.Generic;

namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMLocationLotParkedVehicles
    {
        public VMLocationLotParkedVehicles()
        {
            CustomerParkingSlotID = new List<LocationLotParkedVehicles>();
        }
        public List<LocationLotParkedVehicles> CustomerParkingSlotID { get; set; }
        public int TotalTwoWheeler { get; set; }
        public int TotalFourWheeler { get; set; }
        public int TotalOutTwoWheeler { get; set; }
        public int TotalOutFourWheeler { get; set; }
    }
}