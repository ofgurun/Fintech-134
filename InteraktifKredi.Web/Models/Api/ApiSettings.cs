namespace InteraktifKredi.Web.Models.Api
{
    /// <summary>
    /// Configuration settings for the external API
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Base URL of the external API
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int Timeout { get; set; } = 30;
    }
}

