using InteraktifKredi.Web.Models.Api;
using InteraktifKredi.Web.Models.Api.Auth;
using InteraktifKredi.Web.Models.Api.Dashboard;
using InteraktifKredi.Web.Models.Api.Reports;

namespace InteraktifKredi.Web.Services
{
    /// <summary>
    /// Interface for API service operations
    /// </summary>
    public interface IApiService
    {
        // ========================================================================
        // Authentication & User Verification
        // ========================================================================

        /// <summary>
        /// Verifies user credentials (TCKN and GSM) with the external API
        /// </summary>
        /// <param name="request">User verification request containing TCKN and GSM</param>
        /// <returns>Service response containing user verification result</returns>
        Task<ServiceResponse<VerifyUserResponse>> VerifyUserAsync(VerifyUserRequest request);

        /// <summary>
        /// Generates OTP code for user authentication
        /// </summary>
        /// <param name="tckn">Turkish ID Number</param>
        /// <param name="gsm">Mobile phone number (10 digits)</param>
        /// <returns>Service response containing OTP code</returns>
        Task<ServiceResponse<GenerateOtpResponse>> GenerateOtpAsync(string tckn, string gsm);

        /// <summary>
        /// Sends OTP code via SMS to user's mobile phone
        /// </summary>
        /// <param name="gsm">Mobile phone number (10 digits)</param>
        /// <param name="otpCode">OTP code to send</param>
        /// <returns>Service response indicating SMS send status</returns>
        Task<ServiceResponse<SendOtpSmsResponse>> SendOtpSmsAsync(string gsm, string otpCode);

        /// <summary>
        /// Verifies OTP code entered by user
        /// </summary>
        /// <param name="otpCode">OTP code to verify</param>
        /// <returns>Service response containing authentication token and user info</returns>
        Task<ServiceResponse<VerifyOtpResponse>> VerifyOtpAsync(string otpCode);

        /// <summary>
        /// Retrieves KVKK text by ID
        /// </summary>
        /// <param name="id">KVKK document ID</param>
        /// <returns>Service response containing KVKK text details</returns>
        Task<ServiceResponse<KvkkTextResponse>> GetKvkkTextAsync(int id);

        /// <summary>
        /// Saves KVKK approval for a customer
        /// </summary>
        /// <param name="request">KVKK approval request</param>
        /// <returns>Service response indicating save status</returns>
        Task<ServiceResponse<bool>> SaveKvkkApprovalAsync(KvkkApprovalRequest request);

        // ========================================================================
        // Dashboard - Reports
        // ========================================================================

        /// <summary>
        /// Retrieves dummy report list
        /// </summary>
        /// <returns>Service response containing list of report summaries</returns>
        Task<ServiceResponse<List<Models.Api.Reports.ReportSummary>>> GetReportListAsync();

        /// <summary>
        /// Retrieves detailed report by ID
        /// </summary>
        /// <param name="reportId">Report ID</param>
        /// <returns>Service response containing report details</returns>
        Task<ServiceResponse<Models.Api.Reports.ReportDetail>> GetReportDetailAsync(int reportId);

        // ========================================================================
        // Dashboard - Customer Profile (GET)
        // ========================================================================

        /// <summary>
        /// Retrieves customer address information
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Service response containing address details</returns>
        Task<ServiceResponse<AddressResponse>> GetCustomerAddressAsync(long customerId);

        /// <summary>
        /// Retrieves customer job information
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Service response containing job details</returns>
        Task<ServiceResponse<JobResponse>> GetJobInfoAsync(long customerId);

        /// <summary>
        /// Retrieves wife/spouse information
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Service response containing wife information</returns>
        Task<ServiceResponse<WifeInfoResponse>> GetWifeInfoAsync(long customerId);

        /// <summary>
        /// Retrieves customer finance and assets information
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Service response containing finance details</returns>
        Task<ServiceResponse<FinanceResponse>> GetFinanceInfoAsync(long customerId);

        // ========================================================================
        // Dashboard - Customer Profile (POST/Save)
        // ========================================================================

        /// <summary>
        /// Saves customer address information
        /// </summary>
        /// <param name="request">Address information to save</param>
        /// <returns>Service response indicating save status</returns>
        Task<ServiceResponse<bool>> SaveCustomerAddressAsync(SaveAddressRequest request);

        /// <summary>
        /// Saves customer job information
        /// </summary>
        /// <param name="request">Job information to save</param>
        /// <returns>Service response indicating save status</returns>
        Task<ServiceResponse<bool>> SaveJobInfoAsync(SaveJobRequest request);

        /// <summary>
        /// Saves wife/spouse information
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <param name="request">Wife information to save</param>
        /// <returns>Service response indicating save status</returns>
        Task<ServiceResponse<bool>> SaveWifeInfoAsync(long customerId, SaveWifeInfoRequest request);

        /// <summary>
        /// Saves customer finance and assets information
        /// </summary>
        /// <param name="request">Finance information to save</param>
        /// <returns>Service response indicating save status</returns>
        Task<ServiceResponse<bool>> SaveFinanceInfoAsync(SaveFinanceRequest request);
    }
}

