using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Models.Api.Loan;
using InteraktifKredi.Web.Services;

namespace InteraktifKredi.Web.Pages.Loan
{
    public class ApplyModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ApplyModel> _logger;

        [BindProperty]
        public LoanApplicationRequest LoanRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public ApplyModel(IApiService apiService, ILogger<ApplyModel> logger)
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
                // TODO: Backend'de kredi başvuru endpoint'i eklendiğinde burada çağrılacak
                // var response = await _apiService.SubmitLoanApplicationAsync(LoanRequest);
                
                // Şimdilik başarılı mesaj gösteriyoruz
                _logger.LogInformation("Loan application submitted: Type={LoanType}, Amount={Amount}, Term={Term}",
                    LoanRequest.LoanType, LoanRequest.LoanAmount, LoanRequest.LoanTerm);

                TempData["SuccessMessage"] = "Kredi başvurunuz başarıyla alınmıştır. Başvuru numaranız: " + DateTime.Now.Ticks;
                return RedirectToPage("/Loan/Result");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during loan application submission");
                ErrorMessage = "Başvuru gönderilirken bir hata oluştu. Lütfen tekrar deneyin.";
                return Page();
            }
        }
    }
}

