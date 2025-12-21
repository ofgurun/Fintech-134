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
/// Ödeme Tarihçesi UI Öğesi (odeme_tarihcesi_ui array içindeki her bir öğe)
/// </summary>
public class OdemeTarihcesiUiItem
{
    [JsonPropertyName("ay")]
    public string Ay { get; set; } = string.Empty;

    [JsonPropertyName("durum")]
    public string Durum { get; set; } = string.Empty;

    [JsonPropertyName("ui_renk")]
    public string UiRenk { get; set; } = string.Empty;

    [JsonPropertyName("ui_ikon")]
    public string? UiIkon { get; set; }
}

/// <summary>
/// Bireysel Kredi Detayı (bireyselDetails array içindeki her bir öğe)
/// </summary>
public class BireyselDetail
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("bkSiraNo")]
    public string BkSiraNo { get; set; } = string.Empty;

    [JsonPropertyName("bkKurumRumuzu")]
    public string BkKurumRumuzu { get; set; } = string.Empty;

    [JsonPropertyName("bkKurumRumuzuKarsiligi")]
    public string? BkKurumRumuzuKarsiligi { get; set; }

    [JsonPropertyName("bkKrediTuru")]
    public string BkKrediTuru { get; set; } = string.Empty;

    [JsonPropertyName("bkAcilisTarihi")]
    public string BkAcilisTarihi { get; set; } = string.Empty;

    [JsonPropertyName("bkKapanisTarihi")]
    public string BkKapanisTarihi { get; set; } = string.Empty;

    [JsonPropertyName("bkKrediTutariLimiti")]
    public string BkKrediTutariLimiti { get; set; } = string.Empty;

    [JsonPropertyName("bkToplamBakiye")]
    public string BkToplamBakiye { get; set; } = string.Empty;

    [JsonPropertyName("bkGecikmedekiBakiye")]
    public string BkGecikmedekiBakiye { get; set; } = string.Empty;

    [JsonPropertyName("bkDovizKodu")]
    public string BkDovizKodu { get; set; } = string.Empty;

    [JsonPropertyName("bkOdemePerformansiTarihcesi")]
    public string BkOdemePerformansiTarihcesi { get; set; } = string.Empty;

    [JsonPropertyName("odeme_tarihcesi_ui")]
    public List<OdemeTarihcesiUiItem>? OdemeTarihcesiUi { get; set; }
}

/// <summary>
/// Rapor detay bilgisi (API Response)
/// API'den gelen format: { "statusCode": 200, "value": { "reportId": 987684, "referansNo": "...", "bkKrediNotu": "1443", ... } }
/// </summary>
public class ReportDetail
{
    // Ana alanlar (API'den gelen - value objesi içindeki)
    [JsonPropertyName("reportDetailsId")]
    public int? ReportDetailsId { get; set; }

    [JsonPropertyName("reportId")]
    public int ReportId { get; set; }

    [JsonPropertyName("referansNo")]
    public string ReferansNo { get; set; } = string.Empty;

    [JsonPropertyName("customerId")]
    public int? CustomerId { get; set; }

    [JsonPropertyName("bireyselId")]
    public int? BireyselId { get; set; }

    // Kredi Notu ve Özet Bilgiler
    [JsonPropertyName("bkKrediNotu")]
    public string BkKrediNotu { get; set; } = string.Empty;

    [JsonPropertyName("bkToplamLimit")]
    public string BkToplamLimit { get; set; } = string.Empty;

    [JsonPropertyName("bkToplamRisk")]
    public string BkToplamRisk { get; set; } = string.Empty;

    [JsonPropertyName("bkToplamKrediliHesapSayisi")]
    public string BkToplamKrediliHesapSayisi { get; set; } = string.Empty;

    [JsonPropertyName("bkBildirimdeBulunanFinansKurulusuSayisi")]
    public string BkBildirimdeBulunanFinansKurulusuSayisi { get; set; } = string.Empty;

    [JsonPropertyName("bkGecikmedekiToplamHesapSayisi")]
    public string BkGecikmedekiToplamHesapSayisi { get; set; } = string.Empty;

    [JsonPropertyName("bkGeciktirdigiBakiyeToplami")]
    public string BkGeciktirdigiBakiyeToplami { get; set; } = string.Empty;

    [JsonPropertyName("bkSonKrediKullandirimTarihi")]
    public string BkSonKrediKullandirimTarihi { get; set; } = string.Empty;

    [JsonPropertyName("bkWorstPaymetStatusEver")]
    public string BkWorstPaymetStatusEver { get; set; } = string.Empty;

    // Kredi Notu Sebep Kodları
    [JsonPropertyName("bkKrediNotuSebepKodu1")]
    public string? BkKrediNotuSebepKodu1 { get; set; }

    [JsonPropertyName("bkKrediNotuSebepKodu2")]
    public string? BkKrediNotuSebepKodu2 { get; set; }

    [JsonPropertyName("bkKrediNotuSebepKodu3")]
    public string? BkKrediNotuSebepKodu3 { get; set; }

    [JsonPropertyName("bkKrediNotuSebepKodu4")]
    public string? BkKrediNotuSebepKodu4 { get; set; }

    // Detay Listeleri
    [JsonPropertyName("bireyselDetails")]
    public List<BireyselDetail> BireyselDetails { get; set; } = new List<BireyselDetail>();

    // Computed properties for UI
    public int Id => ReportId;

    public string DisplayReportNumber => !string.IsNullOrEmpty(ReferansNo) ? ReferansNo : $"RPT-{ReportId}";
    
    public string DisplayReportName => "Kredi Değerlendirme Raporu";
    
    public int? CreditScore
    {
        get
        {
            if (int.TryParse(BkKrediNotu, out var score))
                return score;
            return null;
        }
    }

    public decimal? Limit
    {
        get
        {
            if (decimal.TryParse(BkToplamLimit, out var limit))
                return limit;
            return null;
        }
    }

    public decimal? Risk
    {
        get
        {
            if (decimal.TryParse(BkToplamRisk, out var risk))
                return risk;
            return null;
        }
    }

    public DateTime DisplayCreatedDate => DateTime.Now; // API'de tarih yok, şimdilik şu anki tarih

    public string Description
    {
        get
        {
            var desc = $"Toplam Limit: {Limit?.ToString("C", new System.Globalization.CultureInfo("tr-TR")) ?? "-"}";
            desc += $"\nToplam Risk: {Risk?.ToString("C", new System.Globalization.CultureInfo("tr-TR")) ?? "-"}";
            desc += $"\nKredili Hesap Sayısı: {BkToplamKrediliHesapSayisi}";
            desc += $"\nFinans Kuruluşu Sayısı: {BkBildirimdeBulunanFinansKurulusuSayisi}";
            return desc;
        }
    }

    public string RiskGroup
    {
        get
        {
            var score = CreditScore ?? 0;
            if (score >= 1500) return "Az Riskli";
            if (score >= 1200) return "Orta Riskli";
            if (score >= 900) return "Yüksek Riskli";
            return "Çok Yüksek Riskli";
        }
    }
}

