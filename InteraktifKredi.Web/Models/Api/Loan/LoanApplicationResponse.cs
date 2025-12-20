namespace InteraktifKredi.Web.Models.Api.Loan
{
    /// <summary>
    /// Kredi başvuru yanıt modeli
    /// </summary>
    public class LoanApplicationResponse
    {
        public int ApplicationId { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public decimal LoanAmount { get; set; }
        public int LoanTerm { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}

