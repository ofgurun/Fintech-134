using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteraktifKredi.Web.Pages.Loan
{
    public class ResultModel : PageModel
    {
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // TempData'dan mesajları al
            SuccessMessage = TempData["SuccessMessage"]?.ToString();
            ErrorMessage = TempData["ErrorMessage"]?.ToString();
        }
    }
}

