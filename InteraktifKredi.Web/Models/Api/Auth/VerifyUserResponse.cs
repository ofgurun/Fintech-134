namespace InteraktifKredi.Web.Models.Api.Auth
{
    /// <summary>
    /// Response model for user verification
    /// </summary>
    public class VerifyUserResponse
    {
        /// <summary>
        /// Unique customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Turkish Identification Number
        /// </summary>
        public string TCKN { get; set; } = string.Empty;

        /// <summary>
        /// Mobile phone number
        /// </summary>
        public string GSM { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this is a new user registration
        /// </summary>
        public bool IsNewUser { get; set; }
    }
}

