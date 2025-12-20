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
        /// POST request handler - processes login form submission
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
                _logger.LogInformation("Attempting to verify user with TCKN: {TCKN}", VerifyRequest.TCKN);

                // Call API service to verify user credentials
                var response = await _apiService.VerifyUserAsync(VerifyRequest);

                // Scenario 1: Success - User verified
                if (response.Success && response.Value != null)
                {
                    // Store user information in TempData for next page
                    TempData["CustomerId"] = response.Value.CustomerId;
                    TempData["IsNewUser"] = response.Value.IsNewUser;
                    TempData["TCKN"] = response.Value.TCKN;
                    TempData["GSM"] = response.Value.GSM;

                    _logger.LogInformation("User verification successful for CustomerId: {CustomerId}, IsNewUser: {IsNewUser}", 
                        response.Value.CustomerId, response.Value.IsNewUser);

                    // Redirect to OTP verification page
                    return RedirectToPage("/Auth/OtpVerify");
                }
                // Scenario 2: Failure - Invalid credentials or API error
                else
                {
                    _logger.LogWarning("User verification failed: {Message} (Status: {StatusCode})", 
                        response.Message, response.StatusCode);

                    // Add model error to display in validation summary
                    ModelState.AddModelError(string.Empty, "Giriş bilgileri doğrulanamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol ediniz.");
                    
                    // Also set ErrorMessage for custom display
                    ErrorMessage = response.Message;

                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during login attempt");
                
                ModelState.AddModelError(string.Empty, "Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edip tekrar deneyin.");
                ErrorMessage = "Bağlantı hatası oluştu.";
                
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login attempt");
                
                ModelState.AddModelError(string.Empty, "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                ErrorMessage = "Giriş yapılırken bir hata oluştu. Lütfen tekrar deneyin.";
                
                return Page();
            }
        }
    }
}

