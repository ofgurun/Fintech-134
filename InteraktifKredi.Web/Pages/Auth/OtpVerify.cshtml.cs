using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using InteraktifKredi.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages.Auth
{
    /// <summary>
    /// OTP (SMS) verification page model
    /// </summary>
    public class OtpVerifyModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<OtpVerifyModel> _logger;

        /// <summary>
        /// SMS verification code entered by user
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "SMS şifresi gereklidir.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "SMS şifresi 6 haneli olmalıdır.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "SMS şifresi sadece rakamlardan oluşmalıdır.")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Customer ID passed from Login page
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Is this a new user registration?
        /// </summary>
        public bool IsNewUser { get; set; }

        /// <summary>
        /// TCKN for display
        /// </summary>
        public string TCKN { get; set; } = string.Empty;

        /// <summary>
        /// GSM number for display (masked)
        /// </summary>
        public string GSM { get; set; } = string.Empty;

        /// <summary>
        /// Masked GSM for display in UI (0 5XX XXX XX XX format)
        /// </summary>
        public string MaskedGSM => MaskGsmNumber(GSM);

        /// <summary>
        /// Error message to display
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Should KVKK modal be shown?
        /// </summary>
        public bool ShowKvkk { get; set; }

        /// <summary>
        /// KVKK text content (HTML)
        /// </summary>
        public string? KvkkTextContent { get; set; }

        /// <summary>
        /// KVKK document ID
        /// </summary>
        public int KvkkId { get; set; }

        /// <summary>
        /// Stored token from OTP verification
        /// </summary>
        public string? StoredToken { get; set; }

        public OtpVerifyModel(IApiService apiService, ILogger<OtpVerifyModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        /// <summary>
        /// GET request handler - displays OTP form
        /// </summary>
        public IActionResult OnGet()
        {
            _logger.LogInformation("=== OTP VERIFY PAGE LOADED ===");

            // Get data from TempData (passed from Login page)
            if (TempData["CustomerId"] == null || TempData["GSM"] == null)
            {
                // No data from previous page - redirect back to login
                _logger.LogWarning("OTP page accessed without login data - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            // Retrieve session data
            CustomerId = (int)TempData["CustomerId"];
            IsNewUser = TempData["IsNewUser"] != null && (bool)TempData["IsNewUser"];
            TCKN = TempData["TCKN"]?.ToString() ?? string.Empty;
            GSM = TempData["GSM"]?.ToString() ?? string.Empty;

            // Keep TempData for POST
            TempData.Keep();

            _logger.LogInformation("OTP page loaded for CustomerId: {CustomerId}, GSM: {GSM}, IsNewUser: {IsNewUser}", 
                CustomerId, MaskedGSM, IsNewUser);

            // Development/Testing: Log OTP code if available (for testing without SMS)
            if (TempData["OTPCode"] != null)
            {
                var otpCode = TempData["OTPCode"]?.ToString();
                Console.WriteLine("");
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   🔐 TEST OTP CODE (DEV ONLY)                    ║");
                Console.WriteLine("╠═══════════════════════════════════════════════════════════════════╣");
                Console.WriteLine($"║  OTP Code: {otpCode}                                           ║");
                Console.WriteLine($"║  GSM: {MaskedGSM}                                    ║");
                Console.WriteLine("║  ⚠️  This code should NOT be logged in production!              ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("");
                
                _logger.LogInformation("🔐 TEST OTP CODE (DEV ONLY): {OTPCode} for GSM: {GSM}", 
                    otpCode, MaskedGSM);
                
                // Keep OTPCode in TempData for POST
                TempData.Keep("OTPCode");
            }

            return Page();
        }

        /// <summary>
        /// POST request handler - verifies OTP code with API
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("=== OTP VERIFICATION STARTED ===");

            // Restore TempData values
            if (TempData["CustomerId"] == null || TempData["GSM"] == null)
            {
                _logger.LogWarning("OTP POST without session data - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            CustomerId = (int)TempData["CustomerId"];
            IsNewUser = TempData["IsNewUser"] != null && (bool)TempData["IsNewUser"];
            TCKN = TempData["TCKN"]?.ToString() ?? string.Empty;
            GSM = TempData["GSM"]?.ToString() ?? string.Empty;

            // Validate model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("OTP validation failed - invalid model state");
                ErrorMessage = "Lütfen 6 haneli SMS şifresini giriniz.";
                return Page();
            }

            try
            {
                _logger.LogInformation("Verifying OTP code: {Code} for CustomerId: {CustomerId}", 
                    Code, CustomerId);

                // ========================================================================
                // Call API to verify OTP code
                // ========================================================================
                var response = await _apiService.VerifyOtpAsync(Code);

                if (response.Success && response.Value != null)
                {
                    _logger.LogInformation("✅ OTP verified successfully. Token received.");

                    // ====================================================================
                    // Sign In User
                    // ====================================================================
                    await SignInUserAsync(CustomerId, TCKN, GSM, response.Value.Token, IsNewUser);

                    // ====================================================================
                    // Store KVKK flag in TempData for Dashboard
                    // TODO: Production'da if (IsNewUser) olacak, test için if (true)
                    // ====================================================================
                    if (true) // TEST MODE: Show KVKK for all users
                    {
                        TempData["ShowKvkk"] = true;
                        TempData["CustomerId"] = CustomerId;
                        _logger.LogInformation("User will see KVKK modal on Dashboard");
                    }

                    // ====================================================================
                    // Redirect to Dashboard
                    // ====================================================================
                    _logger.LogInformation("Redirecting to Dashboard");
                    return RedirectToPage("/Dashboard/Index");
                }
                else
                {
                    // ====================================================================
                    // OTP Verification Failed
                    // ====================================================================
                    _logger.LogWarning("❌ OTP verification failed: {Message}", response.Message);
                    
                    ModelState.AddModelError(string.Empty, 
                        "Girdiğiniz SMS şifresi hatalı veya süresi dolmuş. Lütfen tekrar deneyin.");
                    ErrorMessage = response.Message ?? "Hatalı veya süresi geçmiş kod.";
                    
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error during OTP verification");
                
                ModelState.AddModelError(string.Empty, 
                    "SMS doğrulama sırasında bir hata oluştu. Lütfen tekrar deneyin.");
                ErrorMessage = "Doğrulama hatası oluştu.";
                
                return Page();
            }
        }

        /// <summary>
        /// AJAX handler to resend OTP SMS
        /// </summary>
        public async Task<IActionResult> OnPostResendOtpAsync()
        {
            _logger.LogInformation("=== RESEND OTP STARTED ===");

            try
            {
                // TRY 1: Get from TempData
                var tckn = TempData["TCKN"]?.ToString();
                var gsm = TempData["GSM"]?.ToString();

                _logger.LogInformation("TempData check - TCKN: {TCKN}, GSM: {GSM}", tckn ?? "NULL", gsm ?? "NULL");

                // TRY 2: If TempData is empty, try to get from current page properties
                if (string.IsNullOrEmpty(tckn) || string.IsNullOrEmpty(gsm))
                {
                    _logger.LogWarning("TempData empty, trying to restore from Request.Form or ViewData");
                    
                    // Get from form hidden fields (we'll add these to the page)
                    tckn = Request.Form["TCKN"].ToString();
                    gsm = Request.Form["GSM"].ToString();
                    
                    _logger.LogInformation("Form check - TCKN: {TCKN}, GSM: {GSM}", tckn ?? "NULL", gsm ?? "NULL");
                }

                if (string.IsNullOrEmpty(tckn) || string.IsNullOrEmpty(gsm))
                {
                    _logger.LogError("❌ Resend OTP failed - missing session data (both TempData and Form are empty)");
                    return new JsonResult(new { success = false, message = "Oturum bilgileri bulunamadı. Lütfen giriş sayfasından tekrar deneyin." });
                }

                TempData.Keep(); // Keep all TempData for next request

                _logger.LogInformation("Resending OTP for TCKN: {TCKN}, GSM: {GSM}", tckn, gsm);

                // Generate new OTP
                var otpResponse = await _apiService.GenerateOtpAsync(tckn, gsm);
                if (!otpResponse.Success || otpResponse.Value == null)
                {
                    _logger.LogError("Failed to generate OTP: {Message}", otpResponse.Message);
                    return new JsonResult(new { success = false, message = "OTP oluşturulamadı." });
                }

                var otpCode = otpResponse.Value.OTPCode.ToString();
                _logger.LogInformation("New OTP generated: {OTPCode}", otpCode);

                // Send SMS
                var smsResponse = await _apiService.SendOtpSmsAsync(gsm, otpCode);
                if (!smsResponse.Success)
                {
                    _logger.LogError("Failed to send OTP SMS: {Message}", smsResponse.Message);
                    return new JsonResult(new { success = false, message = "SMS gönderilemedi." });
                }

                _logger.LogInformation("✅ OTP SMS resent successfully");

                // Store new OTP code in TempData for development
                TempData["OTPCode"] = otpCode;
                TempData["TCKN"] = tckn; // Re-store for next requests
                TempData["GSM"] = gsm;
                TempData.Keep();

                // Log new OTP for development
                Console.WriteLine($"🔐 NEW OTP CODE: {otpCode}");

                return new JsonResult(new { success = true, message = "SMS tekrar gönderildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resending OTP");
                return new JsonResult(new { success = false, message = "Bir hata oluştu." });
            }
        }

        /// <summary>
        /// Masks GSM number for display: 5551112233 -> 0 (5XX) XXX XX 33
        /// </summary>
        private string MaskGsmNumber(string gsm)
        {
            if (string.IsNullOrEmpty(gsm) || gsm.Length != 10)
                return gsm;

            // Format: 0 (5XX) XXX XX 33
            return $"0 ({gsm.Substring(0, 1)}XX) XXX XX {gsm.Substring(8, 2)}";
        }

        /// <summary>
        /// Helper method to sign in user with Cookie Authentication
        /// </summary>
        private async Task SignInUserAsync(int customerId, string tckn, string gsm, string token, bool isNewUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customerId.ToString()),
                new Claim(ClaimTypes.Name, tckn ?? ""),
                new Claim(ClaimTypes.MobilePhone, gsm ?? ""),
                new Claim("Token", token),
                new Claim("IsNewUser", isNewUser.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            _logger.LogInformation("User signed in successfully with Cookie Authentication");
        }
    }
}

