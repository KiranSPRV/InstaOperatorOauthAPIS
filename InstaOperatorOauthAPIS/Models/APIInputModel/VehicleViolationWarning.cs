using InstaOperatorOauthAPIS.Models.APIOutPutModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaOperatorOauthAPIS.Models.APIInputModel
{
    public class VehicleViolationWarning
    {
        public VehicleViolationWarning()
        {
            VehicleTypeID = new VehicleType();
            CustomerVehicleID = new CustomerVehicle();
        }
        public int VehicleViolationWarningID { get; set; }
        public string RegistrationNumber { get; set; }
        public VehicleType VehicleTypeID { get; set; }
        public CustomerVehicle CustomerVehicleID { get; set; }
        public int WarningCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int StatusID { get; set; }
      

    }
}
