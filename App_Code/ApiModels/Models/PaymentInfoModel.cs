namespace ApiModels.Models
{
    public class PaymentInfoModel
    {
        public string MessageId { get; set; }
        public string MsgSource { get; set; }
        public string ClientCode { get; set; }
        public string BatchRefNmbr { get; set; }
        public string CompanyId { get; set; }
        public string MyProdCode { get; set; }
        public string PayMode { get; set; }
        public string TxnAmnt { get; set; }
        public string AccountNo { get; set; }
        public string DrRefNmbr { get; set; }
        public string DrDesc { get; set; }
        public string PaymentDt { get; set; }
        public string InstDt { get; set; }
        public string InstRefNo { get; set; }
        public string Enrichment { get; set; }
    }
}
