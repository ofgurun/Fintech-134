using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteraktifKredi.Web.Models.Api.Dashboard
{
    // ========================================================================
    // GET Response Models
    // ========================================================================

    /// <summary>
    /// Customer address response model
    /// API Response: { "statusCode": 200, "value": { "cityId": 34, "townId": 12, "address": "...", "customerId": 1, "source": 2 } }
    /// </summary>
    public class AddressResponse
    {
        [JsonPropertyName("cityId")]
        public int? CityId { get; set; }

        [JsonPropertyName("townId")]
        public int? TownId { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("customerId")]
        public long? CustomerId { get; set; }

        [JsonPropertyName("source")]
        public int? Source { get; set; }
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
    /// API Response: { "statusCode": 200, "value": { "titleCompany": "...", "companyPosition": "...", "jobGroupId": 3, "customerWork": 5, "workingYears": 5, "workingMonth": 2 } }
    /// </summary>
    public class JobResponse
    {
        [JsonPropertyName("titleCompany")]
        public string? TitleCompany { get; set; }

        [JsonPropertyName("companyPosition")]
        public string? CompanyPosition { get; set; }

        [JsonPropertyName("jobGroupId")]
        public int? JobGroupId { get; set; }

        [JsonPropertyName("customerWork")]
        public int? CustomerWork { get; set; }

        [JsonPropertyName("workingYears")]
        public int? WorkingYears { get; set; }

        [JsonPropertyName("workingMonth")]
        public int? WorkingMonth { get; set; }
    }

    // ========================================================================
    // POST Request Models
    // ========================================================================

    /// <summary>
    /// Save customer address request model
    /// API Request: { "customerId": 1, "adress": "string", "cityId": 34, "townId": 12, "source": 2 }
    /// </summary>
    public class SaveAddressRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("adress")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("cityId")]
        public int CityId { get; set; }

        [JsonPropertyName("townId")]
        public int TownId { get; set; }

        [JsonPropertyName("source")]
        public int Source { get; set; } = 2; // Sabit değer: 2 = Kişi ekler
    }

    /// <summary>
    /// Save wife information request model
    /// </summary>
    public class SaveWifeInfoRequest
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
    /// Save finance information request model
    /// </summary>
    public class SaveFinanceRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

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
    /// Save job information request model
    /// API Request: { "customerId": 1000849, "customerWork": 5, "jobGroupId": 3, "workingYears": 5, "workingMonth": 2, "titleCompany": "...", "companyPosition": "..." }
    /// </summary>
    public class SaveJobRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("customerWork")]
        public int CustomerWork { get; set; }

        [JsonPropertyName("jobGroupId")]
        public int JobGroupId { get; set; }

        [JsonPropertyName("workingYears")]
        public int WorkingYears { get; set; }

        [JsonPropertyName("workingMonth")]
        public int WorkingMonth { get; set; }

        [JsonPropertyName("titleCompany")]
        public string TitleCompany { get; set; } = string.Empty;

        [JsonPropertyName("companyPosition")]
        public string CompanyPosition { get; set; } = string.Empty;

        // UI-only fields (not sent to API)
        public DateTime? StartDate { get; set; }
        public decimal? MonthlyIncome { get; set; }
    }
}

