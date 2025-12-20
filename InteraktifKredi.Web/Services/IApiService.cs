using InteraktifKredi.Web.Models.Api;
using InteraktifKredi.Web.Models.Api.Auth;

namespace InteraktifKredi.Web.Services
{
    /// <summary>
    /// Interface for API service operations
    /// </summary>
    public interface IApiService
    {
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
    }
}

