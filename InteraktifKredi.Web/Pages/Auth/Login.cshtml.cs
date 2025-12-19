using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api.Auth;

namespace InteraktifKredi.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public VerifyUserRequest VerifyRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public void OnGet()
        {
            // Page initialization
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _apiService.VerifyUserAsync(VerifyRequest);

                if (response.Success && response.Value != null)
                {
                    // Store user information in session/tempdata
                    TempData["CustomerId"] = response.Value.CustomerId;
                    TempData["IsNewUser"] = response.Value.IsNewUser;

                    _logger.LogInformation("User login successful for CustomerId: {CustomerId}", response.Value.CustomerId);

                    // Redirect to OTP verification page
                    return RedirectToPage("/Auth/OtpVerify");
                }
                else
                {
                    ErrorMessage = response.Message;
                    _logger.LogWarning("User login failed: {Message}", response.Message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt");
                ErrorMessage = "Giriş yapılırken bir hata oluştu. Lütfen tekrar deneyin.";
                return Page();
            }
        }
    }
}

