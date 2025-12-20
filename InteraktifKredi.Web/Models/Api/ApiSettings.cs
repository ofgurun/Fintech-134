namespace InteraktifKredi.Web.Models.Api
{
    /// <summary>
    /// Configuration settings for the external Azure Function APIs
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Base URL of the Customers API
        /// Example: https://customers-api.azurewebsites.net
        /// </summary>
        public string CustomersApi { get; set; } = string.Empty;

        /// <summary>
        /// Base URL of the IDC API
        /// Example: https://api-idc.azurewebsites.net
        /// </summary>
        public string IdcApi { get; set; } = string.Empty;

        /// <summary>
        /// Azure Function authentication key (Function Key)
        /// This will be appended as ?code={FunctionKey} to API requests
        /// </summary>
        public string FunctionKey { get; set; } = string.Empty;

        /// <summary>
        /// Bearer token for API authorization
        /// This will be added as Authorization: Bearer {BearerToken} header
        /// </summary>
        public string BearerToken { get; set; } = string.Empty;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int Timeout { get; set; } = 30;
    }
}

