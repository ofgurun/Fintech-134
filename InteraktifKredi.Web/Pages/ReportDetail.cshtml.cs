using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api.Reports;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages
{
    public class ReportDetailModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ReportDetailModel> _logger;

        public ReportDetailModel(IApiService apiService, ILogger<ReportDetailModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        /// <summary>
        /// Rapor detay bilgisi
        /// </summary>
        public ReportDetail? ReportDetail { get; set; }

        /// <summary>
        /// Hata mesajı
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// GET Handler - Rapor detayını yükle
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                _logger.LogInformation("=== REPORT DETAIL PAGE LOADED === ID: {Id}", id);
                Console.WriteLine($"[ReportDetail] Page loaded with ID: {id}");

                // Get CustomerId from Claims
                var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerIdClaim))
                {
                    _logger.LogWarning("ReportDetail accessed without authentication - redirecting to Login");
                    Console.WriteLine("[ReportDetail] No authentication - redirecting to Login");
                    return RedirectToPage("/Auth/Login");
                }

                // Validate id parameter (query string'den gelen id parametresi)
                if (!id.HasValue || id.Value <= 0)
                {
                    _logger.LogWarning("ReportDetail accessed without valid id - showing error message");
                    Console.WriteLine($"[ReportDetail] Invalid ID: {id}");
                    ErrorMessage = "Geçersiz rapor ID'si. Lütfen Dashboard'dan tekrar deneyin.";
                    return Page();
                }

                _logger.LogInformation("Fetching report detail for ReportId: {ReportId}", id.Value);
                Console.WriteLine($"[ReportDetail] Fetching report detail for ID: {id.Value}");

                // Fetch report detail from API
                var reportResponse = await _apiService.GetReportDetailAsync(id.Value);

                _logger.LogInformation("API Response - Success: {Success}, HasValue: {HasValue}, Message: {Message}",
                    reportResponse.Success, reportResponse.Value != null, reportResponse.Message);
                Console.WriteLine($"[ReportDetail] API Response - Success: {reportResponse.Success}, HasValue: {reportResponse.Value != null}, Message: {reportResponse.Message}");
                
                // Ham JSON'u Console'a yazdır
                if (!string.IsNullOrEmpty(reportResponse.RawResponse))
                {
                    Console.WriteLine("[ReportDetail] API RESPONSE (Raw JSON):");
                    Console.WriteLine(reportResponse.RawResponse);
                }

                if (!reportResponse.Success)
                {
                    var errorMsg = reportResponse.Message ?? "Rapor detayı getirilemedi.";
                    _logger.LogError("Report detail retrieval failed: {Message}, StatusCode: {StatusCode}",
                        errorMsg, reportResponse.StatusCode);
                    Console.WriteLine($"[ReportDetail] ERROR: {errorMsg} (StatusCode: {reportResponse.StatusCode})");
                    
                    // Ham JSON'u hata mesajına ekle
                    if (!string.IsNullOrEmpty(reportResponse.RawResponse))
                    {
                        ErrorMessage = $"API Hatası: {errorMsg}\n\nGelen Ham Veri:\n{reportResponse.RawResponse}";
                    }
                    else
                    {
                        ErrorMessage = $"API Hatası: {errorMsg}";
                    }
                    return Page();
                }

                if (reportResponse.Value == null)
                {
                    _logger.LogError("Report detail is null even though Success is true");
                    Console.WriteLine("[ReportDetail] ERROR: Response.Value is null");
                    
                    // Ham JSON'u hata mesajına ekle
                    if (!string.IsNullOrEmpty(reportResponse.RawResponse))
                    {
                        ErrorMessage = $"Veri işlenemedi. API'den veri dönmedi.\n\nGelen Ham Veri:\n{reportResponse.RawResponse}";
                    }
                    else
                    {
                        ErrorMessage = "API'den veri dönmedi. Lütfen daha sonra tekrar deneyin.";
                    }
                    return Page();
                }

                ReportDetail = reportResponse.Value;
                _logger.LogInformation("✅ Report detail retrieved successfully - ID: {Id}, ReportId: {ReportId}, CreditScore: {CreditScore}",
                    ReportDetail.Id, ReportDetail.ReportId, ReportDetail.CreditScore);
                Console.WriteLine($"[ReportDetail] SUCCESS - ReportId: {ReportDetail.ReportId}, CreditScore: {ReportDetail.CreditScore}, RiskGroup: {ReportDetail.RiskGroup}");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in OnGetAsync");
                Console.WriteLine($"[ReportDetail] EXCEPTION: {ex.Message}");
                Console.WriteLine($"[ReportDetail] StackTrace: {ex.StackTrace}");
                
                ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                if (ex.InnerException != null)
                {
                    ErrorMessage += $" (İç Hata: {ex.InnerException.Message})";
                    Console.WriteLine($"[ReportDetail] InnerException: {ex.InnerException.Message}");
                }
                
                return Page();
            }
        }
    }
}

