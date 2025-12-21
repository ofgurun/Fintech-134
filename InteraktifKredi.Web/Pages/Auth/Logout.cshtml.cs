using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteraktifKredi.Web.Pages.Auth;

    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

    public async Task<IActionResult> OnPostAsync()
        {
        _logger.LogInformation("User {User} logging out", User.Identity?.Name ?? "Unknown");
            
        // Sign out - clear cookie authentication
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirect to login page
            return RedirectToPage("/Auth/Login");
    }
}
