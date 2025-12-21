using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages.Dashboard
{
    public class HelpModel : PageModel
    {
        private readonly ILogger<HelpModel> _logger;

        public HelpModel(ILogger<HelpModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Check authentication
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                _logger.LogWarning("Help Center accessed without authentication - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            return Page();
        }
    }
}

