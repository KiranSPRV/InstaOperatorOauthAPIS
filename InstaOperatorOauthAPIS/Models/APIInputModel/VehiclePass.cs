namespace InstaOperatorOauthAPIS.Models.APIInputModel
{
    public class VehiclePass
    {
        public string RegistrationNumber { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public int LocationParkingLotID { get; set; }
        public string NFCCardNumber { get; set; }
    }
}