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

            _logger.LogInformation("Address Form Data - City: {City}, County: {County}, CustomerId: {CustomerId}", 
                AddressForm.City, AddressForm.County, AddressForm.CustomerId);

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

            _logger.LogInformation("Job Form Data - Company: {Company}, Position: {Position}, CustomerId: {CustomerId}", 
                JobForm.Company, JobForm.Position, JobForm.CustomerId);

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
            CustomerId = long.Parse(customerIdClaim ?? "0");

            _logger.LogInformation("Wife Form Data - WifeName: {WifeName}, MaritalStatus: {MaritalStatus}", 
                WifeForm.WifeName, WifeForm.MaritalStatus);

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
            CustomerId = long.Parse(customerIdClaim ?? "0");
            FinanceForm.CustomerId = CustomerId;

            _logger.LogInformation("Finance Form Data - BankName: {BankName}, HasCar: {HasCar}, CustomerId: {CustomerId}", 
                FinanceForm.BankName, FinanceForm.HasCar, FinanceForm.CustomerId);

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
                    City = addressResponse.Value.City,
                    County = addressResponse.Value.County,
                    District = addressResponse.Value.District,
                    AddressDetail = addressResponse.Value.AddressDetail,
                    ResidenceDuration = addressResponse.Value.ResidenceDuration,
                    HomeType = addressResponse.Value.HomeType
                };
                _logger.LogInformation("Address data loaded");
            }

            // Fetch Job
            var jobResponse = await _apiService.GetJobInfoAsync(CustomerId);
            if (jobResponse.Success && jobResponse.Value != null)
            {
                JobForm = new SaveJobRequest
                {
                    CustomerId = CustomerId,
                    Company = jobResponse.Value.Company,
                    Sector = jobResponse.Value.Sector,
                    Position = jobResponse.Value.Position,
                    StartDate = jobResponse.Value.StartDate,
                    MonthlyIncome = jobResponse.Value.MonthlyIncome,
                    EmploymentType = jobResponse.Value.EmploymentType
                };
                _logger.LogInformation("Job data loaded");
            }

            // Fetch Wife
            var wifeResponse = await _apiService.GetWifeInfoAsync(CustomerId);
            if (wifeResponse.Success && wifeResponse.Value != null)
            {
                WifeForm = new SaveWifeInfoRequest
                {
                    WifeName = wifeResponse.Value.WifeName,
                    WifeTCKN = wifeResponse.Value.WifeTCKN,
                    WifePhone = wifeResponse.Value.WifePhone,
                    MaritalStatus = wifeResponse.Value.MaritalStatus,
                    NumberOfChildren = wifeResponse.Value.NumberOfChildren,
                    IsWifeWorking = wifeResponse.Value.IsWifeWorking
                };
                _logger.LogInformation("Wife data loaded");
            }

            // Fetch Finance
            var financeResponse = await _apiService.GetFinanceInfoAsync(CustomerId);
            if (financeResponse.Success && financeResponse.Value != null)
            {
                FinanceForm = new SaveFinanceRequest
                {
                    CustomerId = CustomerId,
                    HasCar = financeResponse.Value.HasCar,
                    CarValue = financeResponse.Value.CarValue,
                    HasRealEstate = financeResponse.Value.HasRealEstate,
                    RealEstateValue = financeResponse.Value.RealEstateValue,
                    BankName = financeResponse.Value.BankName,
                    AccountBalance = financeResponse.Value.AccountBalance,
                    HasCreditCard = financeResponse.Value.HasCreditCard,
                    CreditCardLimit = financeResponse.Value.CreditCardLimit,
                    MonthlyExpenses = financeResponse.Value.MonthlyExpenses
                };
                _logger.LogInformation("Finance data loaded");
            }
        }
    }
}

