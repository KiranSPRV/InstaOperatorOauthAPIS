using System.Collections.Generic;

namespace InstaOperatorOauthAPIS.Models
{
    public class LotParkingReport
    {
        public LotParkingReport()
        {
            LocationLotOperations = new List<LocationLotOperations>();
        }
        public int Id { get; set; }
        public string VehicleType { get; set; }
        public string TotalIn { get; set; }
        public string TotalOut { get; set; }
        public List<LocationLotOperations> LocationLotOperations { get; set; }
        public string Currency { get; set; }
        public string TotalFOC { get; set; }
        public string TotalCash { get; set; }
        public string TotalEpay { get; set; }
        public string OtherIn { get; set; }
        public string OtherOut { get; set; }
        public string _selectedImageType { get; set; }
        public string SelectedImageType { get; set; }
        private bool _isExpandVisible { get; set; }
        private bool _isVisible { get; set; }
        public bool IsVisible { get; set; }



    }
}