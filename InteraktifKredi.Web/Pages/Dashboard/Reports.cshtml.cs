using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api.Reports;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages.Dashboard
{
    public class ReportsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ReportsModel> _logger;

        /// <summary>
        /// List of customer reports
        /// </summary>
        public List<ReportSummary> Reports { get; set; } = new List<ReportSummary>();

        public ReportsModel(IApiService apiService, ILogger<ReportsModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("=== REPORTS PAGE LOADED ===");

            // Get CustomerId from Claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                _logger.LogWarning("Reports accessed without authentication - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            var customerId = int.Parse(customerIdClaim);

            // Fetch Report List from API
            _logger.LogInformation("Fetching reports for CustomerId: {CustomerId}", customerId);
            var reportResponse = await _apiService.GetReportListAsync();

            if (reportResponse.Success && reportResponse.Value != null)
            {
                Reports = reportResponse.Value;
                _logger.LogInformation("✅ {Count} reports retrieved successfully", Reports.Count);
            }
            else
            {
                _logger.LogWarning("Report retrieval failed: {Message}", reportResponse.Message);
                Reports = new List<ReportSummary>(); // Empty list
            }

            return Page();
        }

        /// <summary>
        /// AJAX Handler for Get Report Detail
        /// </summary>
        public async Task<IActionResult> OnGetGetReportDetailAsync(int reportId)
        {
            try
            {
                _logger.LogInformation("=== GET REPORT DETAIL === ReportId: {ReportId}", reportId);

                // Fetch report detail from API
                var reportResponse = await _apiService.GetReportDetailAsync(reportId);

                if (!reportResponse.Success || reportResponse.Value == null)
                {
                    _logger.LogError("Report detail retrieval failed: {Message}", reportResponse.Message);
                    return new JsonResult(new { error = true, message = reportResponse.Message ?? "Rapor bulunamadı." }) { StatusCode = 404 };
                }

                _logger.LogInformation("✅ Report detail retrieved successfully - ID: {Id}", reportResponse.Value.Id);

                return new JsonResult(reportResponse.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report detail");
                return new JsonResult(new { error = true, message = "Rapor detayları alınırken bir hata oluştu." }) { StatusCode = 500 };
            }
        }
    }
}

