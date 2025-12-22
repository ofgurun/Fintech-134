using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

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
        /// Customer ID
        /// </summary>
        public int CustomerId { get; set; }

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("=== ANA MENÜ PAGE LOADED ===");

            // Get CustomerId from Claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                _logger.LogWarning("Ana Menü accessed without authentication - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            CustomerId = int.Parse(customerIdClaim);

            // Check if KVKK should be shown
            var showKvkk = TempData["ShowKvkk"] as bool?;
            if (showKvkk == true)
            {
                _logger.LogInformation("KVKK modal requested - fetching KVKK text...");

                var kvkkResponse = await _apiService.GetKvkkTextAsync(1);

                if (kvkkResponse.Success && kvkkResponse.Value != null)
                {
                    _logger.LogInformation("✅ KVKK text retrieved - ID: {Id}, Title: {Title}", 
                        kvkkResponse.Value.Id, kvkkResponse.Value.Title);

                    ShowKvkk = true;
                    KvkkTextContent = kvkkResponse.Value.Text;
                    KvkkId = kvkkResponse.Value.Id;
                }
                else
                {
                    _logger.LogError("KVKK text retrieval failed: {Message}", kvkkResponse.Message);
                    // Continue without KVKK modal
                }

                // Clear TempData
                TempData.Remove("ShowKvkk");
            }

            return Page();
        }

        /// <summary>
        /// AJAX Handler for KVKK Approval
        /// </summary>
        public async Task<IActionResult> OnPostApproveKvkkAsync([FromBody] KvkkApprovalDto approvalDto)
        {
            try
            {
                _logger.LogInformation("=== KVKK APPROVAL STARTED === CustomerId: {CustomerId}, KvkkId: {KvkkId}", 
                    approvalDto.CustomerId, approvalDto.KvkkId);

                // Save KVKK approval via API
                var saveRequest = new Models.Api.Auth.KvkkApprovalRequest
                {
                    CustomerId = approvalDto.CustomerId,
                    KvkkId = approvalDto.KvkkId,
                    IsOk = true
                };

                var saveResponse = await _apiService.SaveKvkkApprovalAsync(saveRequest);

                if (!saveResponse.Success)
                {
                    _logger.LogError("KVKK approval save failed: {Message}", saveResponse.Message);
                    return new JsonResult(new { success = false, message = saveResponse.Message ?? "KVKK onayı kaydedilemedi." });
                }

                _logger.LogInformation("✅ KVKK approval saved successfully");

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during KVKK approval");
                return new JsonResult(new { success = false, message = "KVKK onayı sırasında bir hata oluştu." });
            }
        }

    }

    /// <summary>
    /// DTO for KVKK approval AJAX request
    /// </summary>
    public class KvkkApprovalDto
    {
        public int CustomerId { get; set; }
        public int KvkkId { get; set; }
    }
}

