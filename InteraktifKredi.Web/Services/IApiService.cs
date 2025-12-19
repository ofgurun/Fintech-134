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
    }
}

