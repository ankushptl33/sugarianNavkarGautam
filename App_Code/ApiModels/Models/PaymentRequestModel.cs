
namespace ApiModels.Models
{
    public class PaymentRequestModel
    {
        public PaymentInfoModel PaymentInfo { get; set; }
        public ReceiverBankingDetails ReceiverBankingDetails { get; set; }
    }
}
