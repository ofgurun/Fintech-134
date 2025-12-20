using System.ComponentModel.DataAnnotations;

namespace InteraktifKredi.Web.Models.Api.Loan
{
    /// <summary>
    /// Kredi başvuru isteği modeli
    /// </summary>
    public class LoanApplicationRequest
    {
        [Required(ErrorMessage = "Kredi türü seçilmelidir.")]
        [Display(Name = "Kredi Türü")]
        public string LoanType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kredi tutarı girilmelidir.")]
        [Range(10000, 1000000, ErrorMessage = "Kredi tutarı en az 10.000 TL, en fazla 1.000.000 TL olabilir.")]
        [Display(Name = "Kredi Tutarı")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Kredi vadesi seçilmelidir.")]
        [Display(Name = "Kredi Vadesi")]
        public int LoanTerm { get; set; }

        [Display(Name = "Açıklama")]
        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Display(Name = "Meslek")]
        public int? JobId { get; set; }

        [Display(Name = "Kurum Adı")]
        [MaxLength(200, ErrorMessage = "Kurum adı en fazla 200 karakter olabilir.")]
        public string? InstitutionName { get; set; }

        [Display(Name = "Görev")]
        [MaxLength(100, ErrorMessage = "Görev en fazla 100 karakter olabilir.")]
        public string? Position { get; set; }
    }
}

