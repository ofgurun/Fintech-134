using System.ComponentModel.DataAnnotations;

namespace InteraktifKredi.Web.Models.Api.Auth
{
    /// <summary>
    /// Request model for user verification (TCKN and GSM)
    /// </summary>
    public class VerifyUserRequest
    {
        /// <summary>
        /// Turkish Identification Number (TC Kimlik No)
        /// </summary>
        [Required(ErrorMessage = "TCKN zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TCKN 11 karakter olmalıdır.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TCKN sadece rakamlardan oluşmalıdır.")]
        public string TCKN { get; set; } = string.Empty;

        /// <summary>
        /// Mobile phone number (GSM)
        /// </summary>
        [Required(ErrorMessage = "GSM numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [RegularExpression(@"^5\d{9}$", ErrorMessage = "GSM numarası 5 ile başlamalı ve 10 haneli olmalıdır.")]
        public string GSM { get; set; } = string.Empty;
    }
}

