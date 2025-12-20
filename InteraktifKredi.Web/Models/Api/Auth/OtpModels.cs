using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteraktifKredi.Web.Models.Api.Auth
{
    /// <summary>
    /// Request model for generating OTP code
    /// </summary>
    public class GenerateOtpRequest
    {
        /// <summary>
        /// Turkish ID Number (TC Kimlik No)
        /// </summary>
        [Required(ErrorMessage = "TCKN gereklidir")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TCKN 11 haneli olmalıdır")]
        [JsonPropertyName("tckn")]
        public string TCKN { get; set; } = string.Empty;

        /// <summary>
        /// Mobile phone number (10 digits without 0)
        /// </summary>
        [Required(ErrorMessage = "GSM numarası gereklidir")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "GSM numarası 10 haneli olmalıdır")]
        [JsonPropertyName("gsm")]
        public string GSM { get; set; } = string.Empty;

        /// <summary>
        /// UTM ID for tracking (default: 5)
        /// </summary>
        [JsonPropertyName("utmId")]
        public string UtmId { get; set; } = "5";
    }

    /// <summary>
    /// Response model for OTP generation
    /// </summary>
    public class GenerateOtpResponse
    {
        /// <summary>
        /// Generated OTP code (as integer from API)
        /// </summary>
        [JsonPropertyName("OTPCode")]
        public int OTPCode { get; set; }

        /// <summary>
        /// OTP ID from the system
        /// </summary>
        [JsonPropertyName("OtpId")]
        public int OtpId { get; set; }

        /// <summary>
        /// SMS verification ID
        /// </summary>
        [JsonPropertyName("SmsVerificationId")]
        public int SmsVerificationId { get; set; }
    }

    /// <summary>
    /// Request model for sending OTP via SMS
    /// </summary>
    public class SendOtpSmsRequest
    {
        /// <summary>
        /// Mobile phone number (10 digits without 0)
        /// </summary>
        [Required(ErrorMessage = "GSM numarası gereklidir")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "GSM numarası 10 haneli olmalıdır")]
        [JsonPropertyName("gsm")]
        public string GSM { get; set; } = string.Empty;

        /// <summary>
        /// OTP code to send
        /// </summary>
        [Required(ErrorMessage = "OTP kodu gereklidir")]
        [JsonPropertyName("otpCode")]
        public string OTPCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for SMS sending
    /// </summary>
    public class SendOtpSmsResponse
    {
        /// <summary>
        /// SMS sent successfully
        /// </summary>
        public bool Sent { get; set; }

        /// <summary>
        /// Message ID from SMS provider
        /// </summary>
        public string? MessageId { get; set; }
    }

    /// <summary>
    /// Request model for verifying OTP code
    /// </summary>
    public class VerifyOtpRequest
    {
        /// <summary>
        /// OTP code to verify
        /// </summary>
        [Required(ErrorMessage = "OTP kodu gereklidir")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP kodu 6 haneli olmalıdır")]
        [JsonPropertyName("otpCode")]
        public string OTPCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for OTP verification
    /// </summary>
    public class VerifyOtpResponse
    {
        /// <summary>
        /// JWT authentication token
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}

