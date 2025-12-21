using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteraktifKredi.Web.Models.Api.Dashboard
{
    // ========================================================================
    // GET Response Models
    // ========================================================================

    /// <summary>
    /// Customer address response model
    /// </summary>
    public class AddressResponse
    {
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("county")]
        public string County { get; set; } = string.Empty;

        [JsonPropertyName("district")]
        public string District { get; set; } = string.Empty;

        [JsonPropertyName("addressDetail")]
        public string AddressDetail { get; set; } = string.Empty;

        [JsonPropertyName("residenceDuration")]
        public int? ResidenceDuration { get; set; }

        [JsonPropertyName("homeType")]
        public string? HomeType { get; set; }
    }

    /// <summary>
    /// Wife information response model
    /// </summary>
    public class WifeInfoResponse
    {
        [JsonPropertyName("wifeName")]
        public string? WifeName { get; set; }

        [JsonPropertyName("wifeTCKN")]
        public string? WifeTCKN { get; set; }

        [JsonPropertyName("wifePhone")]
        public string? WifePhone { get; set; }

        [JsonPropertyName("maritalStatus")]
        public string? MaritalStatus { get; set; }

        [JsonPropertyName("numberOfChildren")]
        public int? NumberOfChildren { get; set; }

        [JsonPropertyName("isWifeWorking")]
        public bool? IsWifeWorking { get; set; }
    }

    /// <summary>
    /// Finance and assets response model
    /// </summary>
    public class FinanceResponse
    {
        [JsonPropertyName("hasCar")]
        public bool? HasCar { get; set; }

        [JsonPropertyName("carValue")]
        public decimal? CarValue { get; set; }

        [JsonPropertyName("hasRealEstate")]
        public bool? HasRealEstate { get; set; }

        [JsonPropertyName("realEstateValue")]
        public decimal? RealEstateValue { get; set; }

        [JsonPropertyName("bankName")]
        public string? BankName { get; set; }

        [JsonPropertyName("accountBalance")]
        public decimal? AccountBalance { get; set; }

        [JsonPropertyName("hasCreditCard")]
        public bool? HasCreditCard { get; set; }

        [JsonPropertyName("creditCardLimit")]
        public decimal? CreditCardLimit { get; set; }

        [JsonPropertyName("monthlyExpenses")]
        public decimal? MonthlyExpenses { get; set; }
    }

    /// <summary>
    /// Job information response model
    /// </summary>
    public class JobResponse
    {
        [JsonPropertyName("company")]
        public string Company { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("sector")]
        public string? Sector { get; set; }

        [JsonPropertyName("employmentType")]
        public string? EmploymentType { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("monthlyIncome")]
        public decimal? MonthlyIncome { get; set; }
    }

    // ========================================================================
    // POST Request Models
    // ========================================================================

    /// <summary>
    /// Save customer address request model
    /// </summary>
    public class SaveAddressRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }

        [JsonPropertyName("district")]
        public string? District { get; set; }

        [JsonPropertyName("addressDetail")]
        public string? AddressDetail { get; set; }

        [JsonPropertyName("residenceDuration")]
        public int? ResidenceDuration { get; set; }

        [JsonPropertyName("homeType")]
        public string? HomeType { get; set; }
    }

    /// <summary>
    /// Save wife information request model
    /// </summary>
    public class SaveWifeInfoRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("maritalStatus")]
        public bool MaritalStatus { get; set; }

        [JsonPropertyName("workWife")]
        public bool WorkWife { get; set; }

        [JsonPropertyName("wifeSalaryAmount")]
        public decimal? WifeSalaryAmount { get; set; }
    }

    /// <summary>
    /// Save finance information request model
    /// </summary>
    public class SaveFinanceRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("workSector")]
        public int WorkSector { get; set; }

        [JsonPropertyName("salaryBank")]
        public string? SalaryBank { get; set; }

        [JsonPropertyName("salaryAmount")]
        public decimal? SalaryAmount { get; set; }

        [JsonPropertyName("carStatus")]
        public bool CarStatus { get; set; }

        [JsonPropertyName("houseStatus")]
        public bool HouseStatus { get; set; }
    }

    /// <summary>
    /// Save job information request model
    /// </summary>
    public class SaveJobRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("company")]
        public string? Company { get; set; }

        [JsonPropertyName("position")]
        public string? Position { get; set; }

        [JsonPropertyName("sector")]
        public string? Sector { get; set; }

        [JsonPropertyName("employmentType")]
        public string? EmploymentType { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("monthlyIncome")]
        public decimal? MonthlyIncome { get; set; }
    }
}

