using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api.Auth;

namespace InteraktifKredi.Web.Pages.Auth
{
    /// <summary>
    /// Login page model for user authentication
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LoginModel> _logger;

        /// <summary>
        /// User verification request (TCKN and GSM)
        /// </summary>
        [BindProperty]
        public VerifyUserRequest VerifyRequest { get; set; } = new();

        /// <summary>
        /// Error message to display on the page
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Flag to show KVKK modal
        /// </summary>
        public bool ShowKvkk { get; set; }

        /// <summary>
        /// KVKK text content for modal
        /// </summary>
        public string KvkkTextContent { get; set; } = string.Empty;

        /// <summary>
        /// KVKK ID
        /// </summary>
        public int KvkkId { get; set; }

        /// <summary>
        /// Customer ID (stored temporarily for KVKK approval)
        /// </summary>
        public int StoredCustomerId { get; set; }

        /// <summary>
        /// Temporary Customer ID for binding (testing purposes)
        /// </summary>
        [BindProperty]
        public int TempCustomerId { get; set; }

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="apiService">API service for backend communication</param>
        /// <param name="logger">Logger for tracking operations</param>
        public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        /// <summary>
        /// GET request handler - displays the login form
        /// </summary>
        public void OnGet()
        {
            // Clear any previous TempData
            TempData.Clear();
        }

        /// <summary>
        /// POST request handler - processes login form submission with OTP flow
        /// Flow: VerifyUser → GenerateOTP → SendSMS → Redirect to OTPVerify
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate model state (TCKN and GSM validations)
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login form validation failed");
                return Page();
            }

            try
            {
                _logger.LogInformation("=== LOGIN FLOW STARTED === TCKN: {TCKN}", VerifyRequest.TCKN);

                // ========================================================================
                // STEP 1: Verify User Credentials (TCKN + GSM)
                // ========================================================================
                _logger.LogInformation("Step 1: Verifying user credentials...");
                var verifyResponse = await _apiService.VerifyUserAsync(VerifyRequest);

                if (!verifyResponse.Success || verifyResponse.Value == null)
                {
                    _logger.LogWarning("Step 1 FAILED: User verification failed - {Message} (Status: {StatusCode})", 
                        verifyResponse.Message, verifyResponse.StatusCode);

                    ModelState.AddModelError(string.Empty, 
                        "Giriş bilgileri doğrulanamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol ediniz.");
                    ErrorMessage = verifyResponse.Message ?? "Kullanıcı doğrulanamadı.";

                    return Page();
                }

                _logger.LogInformation("Step 1 SUCCESS: User verified - CustomerId: {CustomerId}, IsNewUser: {IsNewUser}, TCKN: {TCKN}, GSM: {GSM}", 
                    verifyResponse.Value.CustomerId, verifyResponse.Value.IsNewUser, verifyResponse.Value.TCKN, verifyResponse.Value.GSM);

                // ========================================================================
                // STEP 2: Generate OTP Code (For ALL users - KVKK will be shown AFTER OTP)
                // ========================================================================
                _logger.LogInformation("Step 2: Generating OTP code for TCKN: {TCKN}, GSM: {GSM}, IsNewUser: {IsNewUser}", 
                    verifyResponse.Value.TCKN, verifyResponse.Value.GSM, verifyResponse.Value.IsNewUser);

                return await ContinueWithOtpFlowAsync(
                    verifyResponse.Value.CustomerId,
                    verifyResponse.Value.TCKN,
                    verifyResponse.Value.GSM,
                    verifyResponse.Value.IsNewUser
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during login flow");
                
                ModelState.AddModelError(string.Empty, 
                    "Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edip tekrar deneyin.");
                ErrorMessage = "Bağlantı hatası oluştu.";
                
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login flow");
                
                ModelState.AddModelError(string.Empty, 
                    "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                ErrorMessage = "Giriş yapılırken bir hata oluştu. Lütfen tekrar deneyin.";
                
                return Page();
            }
        }

        /// <summary>
        /// Helper method to continue with OTP flow
        /// </summary>
        private async Task<IActionResult> ContinueWithOtpFlowAsync(int customerId, string tckn, string gsm, bool isNewUser)
        {
            try
            {
                // Generate OTP
                var otpResponse = await _apiService.GenerateOtpAsync(tckn, gsm);

                if (!otpResponse.Success || otpResponse.Value == null)
                {
                    _logger.LogError("OTP generation failed - {Message}", otpResponse.Message);

                    ModelState.AddModelError(string.Empty, 
                        "OTP kodu oluşturulamadı. Lütfen tekrar deneyin.");
                    ErrorMessage = otpResponse.Message ?? "OTP oluşturma hatası.";

                    return Page();
                }

                _logger.LogInformation("OTP generated successfully - Code: {OTPCode}", 
                    otpResponse.Value.OTPCode);

                // Send OTP via SMS
                _logger.LogInformation("Sending OTP SMS to GSM: {GSM}", gsm);

                var smsResponse = await _apiService.SendOtpSmsAsync(
                    gsm,
                    otpResponse.Value.OTPCode.ToString()
                );

                if (!smsResponse.Success)
                {
                    _logger.LogError("SMS sending failed - {Message}", smsResponse.Message);

                    ModelState.AddModelError(string.Empty, 
                        "SMS gönderilemedi. Lütfen tekrar deneyin veya telefon numaranızı kontrol edin.");
                    ErrorMessage = smsResponse.Message ?? "SMS gönderme hatası.";

                    return Page();
                }

                _logger.LogInformation("SMS sent successfully");

                // Store user information in TempData for OTP verification page
                TempData["CustomerId"] = customerId;
                TempData["IsNewUser"] = isNewUser;
                TempData["TCKN"] = tckn;
                TempData["GSM"] = gsm;
                
                // Store OTP code for development/testing purposes (as string)
                // TODO: Remove in production for security
                TempData["OTPCode"] = otpResponse.Value.OTPCode.ToString();

                _logger.LogInformation("=== LOGIN FLOW COMPLETED SUCCESSFULLY === Redirecting to OTP Verify");

                // Redirect to OTP verification page
                return RedirectToPage("/Auth/OtpVerify");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during OTP flow");
                
                ModelState.AddModelError(string.Empty, 
                    "OTP oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                ErrorMessage = "OTP hatası oluştu.";
                
                return Page();
            }
        }
    }
}

