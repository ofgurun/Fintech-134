using System.Text.Json.Serialization;

namespace InteraktifKredi.Web.Models.Api.Reports;

// ============================================================================
// REPORT LIST - Get All Reports
// ============================================================================

/// <summary>
/// Rapor listesi request modeli
/// </summary>
public class GetReportListRequest
{
    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }
}

/// <summary>
/// Rapor özet bilgisi (Liste için)
/// API'den gelen dummy data: { "reportId": 987684, "reportDate": "2025-12-02 11:51" }
/// </summary>
public class ReportSummary
{
    [JsonPropertyName("reportId")]
    public int Id { get; set; }

    [JsonPropertyName("reportDate")]
    public string ReportDate { get; set; } = string.Empty;

    // UI için computed properties
    public string ReportNumber => $"RPT-{Id}";
    
    public string ReportName => "Kredi Değerlendirme Raporu";
    
    public DateTime CreatedDate
    {
        get
        {
            if (DateTime.TryParse(ReportDate, out var date))
                return date;
            return DateTime.Now;
        }
    }

    public string FormattedDate => CreatedDate.ToString("dd.MM.yyyy HH:mm");

    // Dummy data için status yok, varsayılan "completed"
    public string Status => "completed";

    public string StatusDisplay => "Tamamlandı";

    public string StatusBadgeClass => "report_card__badge--success";

    // Dummy data için score yok
    public int? CreditScore => null;
}

// ============================================================================
// REPORT DETAIL - Get Single Report
// ============================================================================

/// <summary>
/// Rapor detay request modeli
/// </summary>
public class GetReportDetailRequest
{
    [JsonPropertyName("reportId")]
    public int ReportId { get; set; }

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }
}

/// <summary>
/// Rapor detay bilgisi (Modal için)
/// </summary>
public class ReportDetail
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("reportNumber")]
    public string ReportNumber { get; set; } = string.Empty;

    [JsonPropertyName("reportName")]
    public string ReportName { get; set; } = string.Empty;

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("creditScore")]
    public int? CreditScore { get; set; }

    [JsonPropertyName("loanAmount")]
    public decimal? LoanAmount { get; set; }

    [JsonPropertyName("loanTerm")]
    public int? LoanTerm { get; set; }

    [JsonPropertyName("interestRate")]
    public decimal? InterestRate { get; set; }

    [JsonPropertyName("monthlyPayment")]
    public decimal? MonthlyPayment { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("approvedBy")]
    public string ApprovedBy { get; set; } = string.Empty;

    [JsonPropertyName("approvedDate")]
    public DateTime? ApprovedDate { get; set; }
}

