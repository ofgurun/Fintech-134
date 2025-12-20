using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteraktifKredi.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();
            
            _logger.LogInformation("User logged out successfully");

            // Login sayfasına yönlendir
            return RedirectToPage("/Auth/Login");
        }
    }
}

