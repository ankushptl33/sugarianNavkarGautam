using System.Collections.Generic;

namespace ApiModels.Models
{
    public class StatusApiFinalResponse
    {
        public bool IsSuccess { get; set; }
        public string MessageId { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string InstRefNo { get; set; }
        public string UTR { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
