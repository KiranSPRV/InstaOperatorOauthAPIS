using InstaOperatorOauthAPIS.Models.Pass;
namespace InstaOperatorOauthAPIS.VMModels
{
    public class VMPassPrice
    {
        public VMPassPrice()
        {
            PassTypeID = new PassTypes();
            PassPriceID = new PassPrice();
        }

        public PassTypes PassTypeID { get; set; }
        public PassPrice PassPriceID { get; set; }
    }
}