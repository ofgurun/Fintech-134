using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InteraktifKredi.Web.Services;
using InteraktifKredi.Web.Models.Api.Dashboard;
using System.Security.Claims;

namespace InteraktifKredi.Web.Pages.Dashboard
{
    public class ProfileModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProfileModel> _logger;

        public ProfileModel(IApiService apiService, ILogger<ProfileModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        // ====================================================================
        // PUBLIC PROPERTIES
        // ====================================================================

        public long CustomerId { get; set; }

        // Address Tab
        [BindProperty]
        public SaveAddressRequest AddressForm { get; set; } = new SaveAddressRequest();

        // Job Tab
        [BindProperty]
        public SaveJobRequest JobForm { get; set; } = new SaveJobRequest();

        // Wife Tab
        [BindProperty]
        public SaveWifeInfoRequest WifeForm { get; set; } = new SaveWifeInfoRequest();

        // Finance Tab
        [BindProperty]
        public SaveFinanceRequest FinanceForm { get; set; } = new SaveFinanceRequest();

        // UI State
        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? ActiveTab { get; set; }

        // ====================================================================
        // GET HANDLER - Load Profile Data
        // ====================================================================

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("=== PROFILE PAGE LOADED ===");

            // Get CustomerId from Claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                _logger.LogWarning("Profile accessed without authentication - redirecting to Login");
                return RedirectToPage("/Auth/Login");
            }

            CustomerId = long.Parse(customerIdClaim);

            // Fetch all profile data from API
            await LoadProfileDataAsync();

            return Page();
        }

        // ====================================================================
        // POST HANDLERS - Save Forms
        // ====================================================================

        /// <summary>
        /// Save Address Information
        /// </summary>
        public async Task<IActionResult> OnPostAddressAsync()
        {
            _logger.LogInformation("=== SAVING ADDRESS INFO ===");

            // Set CustomerId
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CustomerId = long.Parse(customerIdClaim ?? "0");
            AddressForm.CustomerId = CustomerId;

            _logger.LogInformation("Address Form Data - CityId: {CityId}, TownId: {TownId}, Address: {Address}, CustomerId: {CustomerId}", 
                AddressForm.CityId, AddressForm.TownId, AddressForm.Address, AddressForm.CustomerId);

            // Save via API
            var response = await _apiService.SaveCustomerAddressAsync(AddressForm);

            if (response.Success)
            {
                _logger.LogInformation("✅ Address saved successfully");
                SuccessMessage = "Adres bilgileri başarıyla kaydedildi.";
                ActiveTab = "address";
                return RedirectToPage();
            }
            else
            {
                _logger.LogError("Address save failed: {Message}", response.Message);
                ErrorMessage = response.Message ?? "Adres bilgileri kaydedilemedi.";
                ActiveTab = "address";
                await LoadProfileDataAsync();
                return Page();
            }
        }

        /// <summary>
        /// Save Job Information
        /// </summary>
        public async Task<IActionResult> OnPostJobAsync()
        {
            _logger.LogInformation("=== SAVING JOB INFO ===");

            // Set CustomerId
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CustomerId = long.Parse(customerIdClaim ?? "0");
            JobForm.CustomerId = CustomerId;

            _logger.LogInformation("Job Form Data - TitleCompany: {TitleCompany}, CompanyPosition: {CompanyPosition}, JobGroupId: {JobGroupId}, CustomerWork: {CustomerWork}, WorkingYears: {WorkingYears}, WorkingMonth: {WorkingMonth}, CustomerId: {CustomerId}", 
                JobForm.TitleCompany, JobForm.CompanyPosition, JobForm.JobGroupId, JobForm.CustomerWork, JobForm.WorkingYears, JobForm.WorkingMonth, JobForm.CustomerId);

            // Save via API
            var response = await _apiService.SaveJobInfoAsync(JobForm);

            if (response.Success)
            {
                _logger.LogInformation("✅ Job info saved successfully");
                SuccessMessage = "Meslek bilgileri başarıyla kaydedildi.";
                ActiveTab = "job";
                return RedirectToPage();
            }
            else
            {
                _logger.LogError("Job save failed: {Message}", response.Message);
                ErrorMessage = response.Message ?? "Meslek bilgileri kaydedilemedi.";
                ActiveTab = "job";
                await LoadProfileDataAsync();
                return Page();
            }
        }

        /// <summary>
        /// Save Wife Information
        /// </summary>
        public async Task<IActionResult> OnPostWifeAsync()
        {
            _logger.LogInformation("=== SAVING WIFE INFO ===");

            // Set CustomerId
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // TEMPORARY: Use a known working customer ID for testing
            CustomerId = 1000849; // TODO: Remove hardcoded ID after testing
            WifeForm.CustomerId = CustomerId;

            _logger.LogInformation("Wife Form Data - MaritalStatus: {MaritalStatus}, WorkWife: {WorkWife}, WifeSalaryAmount: {WifeSalaryAmount}, CustomerId: {CustomerId}", 
                WifeForm.MaritalStatus, WifeForm.WorkWife, WifeForm.WifeSalaryAmount, WifeForm.CustomerId);

            // Save via API
            var response = await _apiService.SaveWifeInfoAsync(CustomerId, WifeForm);

            if (response.Success)
            {
                _logger.LogInformation("✅ Wife info saved successfully");
                SuccessMessage = "Eş bilgileri başarıyla kaydedildi.";
                ActiveTab = "wife";
                return RedirectToPage();
            }
            else
            {
                _logger.LogError("Wife save failed: {Message}", response.Message);
                ErrorMessage = response.Message ?? "Eş bilgileri kaydedilemedi.";
                ActiveTab = "wife";
                await LoadProfileDataAsync();
                return Page();
            }
        }

        /// <summary>
        /// Save Finance Information
        /// </summary>
        public async Task<IActionResult> OnPostFinanceAsync()
        {
            _logger.LogInformation("=== SAVING FINANCE INFO ===");

            // Set CustomerId
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // TEMPORARY: Use a known working customer ID for testing
            CustomerId = 1000849; // TODO: Remove hardcoded ID after testing
            FinanceForm.CustomerId = CustomerId;

            _logger.LogInformation("Finance Form Data - WorkSector: {WorkSector}, SalaryBank: {SalaryBank}, SalaryAmount: {SalaryAmount}, CarStatus: {CarStatus}, HouseStatus: {HouseStatus}, CustomerId: {CustomerId}", 
                FinanceForm.WorkSector, FinanceForm.SalaryBank, FinanceForm.SalaryAmount, FinanceForm.CarStatus, FinanceForm.HouseStatus, FinanceForm.CustomerId);

            // Save via API
            var response = await _apiService.SaveFinanceInfoAsync(FinanceForm);

            if (response.Success)
            {
                _logger.LogInformation("✅ Finance info saved successfully");
                SuccessMessage = "Varlık bilgileri başarıyla kaydedildi.";
                ActiveTab = "finance";
                return RedirectToPage();
            }
            else
            {
                _logger.LogError("Finance save failed: {Message}", response.Message);
                ErrorMessage = response.Message ?? "Varlık bilgileri kaydedilemedi.";
                ActiveTab = "finance";
                await LoadProfileDataAsync();
                return Page();
            }
        }

        // ====================================================================
        // PRIVATE HELPERS
        // ====================================================================

        private async Task LoadProfileDataAsync()
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerIdClaim))
            {
                return;
            }

            CustomerId = long.Parse(customerIdClaim);

            // Fetch Address
            var addressResponse = await _apiService.GetCustomerAddressAsync(CustomerId);
            if (addressResponse.Success && addressResponse.Value != null)
            {
                AddressForm = new SaveAddressRequest
                {
                    CustomerId = CustomerId,
                    CityId = addressResponse.Value.CityId ?? 0,
                    TownId = addressResponse.Value.TownId ?? 0,
                    Address = addressResponse.Value.Address ?? string.Empty,
                    Source = 2 // Sabit değer: 2 = Kişi ekler
                };
                _logger.LogInformation("Address data loaded - CityId: {CityId}, TownId: {TownId}", 
                    AddressForm.CityId, AddressForm.TownId);
            }

            // Fetch Job
            var jobResponse = await _apiService.GetJobInfoAsync(CustomerId);
            if (jobResponse.Success && jobResponse.Value != null)
            {
                JobForm = new SaveJobRequest
                {
                    CustomerId = CustomerId,
                    TitleCompany = jobResponse.Value.TitleCompany ?? string.Empty,
                    CompanyPosition = jobResponse.Value.CompanyPosition ?? string.Empty,
                    JobGroupId = jobResponse.Value.JobGroupId ?? 0,
                    CustomerWork = jobResponse.Value.CustomerWork ?? 0,
                    WorkingYears = jobResponse.Value.WorkingYears ?? 0,
                    WorkingMonth = jobResponse.Value.WorkingMonth ?? 0
                    // StartDate ve MonthlyIncome UI-only, JavaScript tarafından hesaplanacak
                };
                _logger.LogInformation("Job data loaded - JobGroupId: {JobGroupId}, CustomerWork: {CustomerWork}, WorkingYears: {WorkingYears}, WorkingMonth: {WorkingMonth}",
                    JobForm.JobGroupId, JobForm.CustomerWork, JobForm.WorkingYears, JobForm.WorkingMonth);
            }

            // Fetch Wife - Geçici olarak devre dışı (GET ve POST modelleri farklı)
            // var wifeResponse = await _apiService.GetWifeInfoAsync(CustomerId);
            // if (wifeResponse.Success && wifeResponse.Value != null)
            // {
            //     // TODO: GET API'sinden gelen veriyi POST modeline map et
            //     _logger.LogInformation("Wife data loaded");
            // }

            // Fetch Finance - Geçici olarak devre dışı (GET ve POST modelleri farklı)
            // var financeResponse = await _apiService.GetFinanceInfoAsync(CustomerId);
            // if (financeResponse.Success && financeResponse.Value != null)
            // {
            //     // TODO: GET API'sinden gelen veriyi POST modeline map et
            //     _logger.LogInformation("Finance data loaded");
            // }
        }
    }
}

