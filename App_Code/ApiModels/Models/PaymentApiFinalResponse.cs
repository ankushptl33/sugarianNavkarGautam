using System;
using System.Collections.Generic;

namespace ApiModels.Models
{
    public class PaymentApiFinalResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; }
        public string MessageId { get; set; }
        public string PaymentStatusCode { get; set; }
        public string PaymentStatusRemark { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
