namespace InteraktifKredi.Web.Models.Api
{
    /// <summary>
    /// Generic API wrapper response from Azure Function
    /// This wraps all API responses with success flag, status code, and message
    /// </summary>
    /// <typeparam name="T">The type of the actual data in the Value property</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message from the API (e.g., "Bu TCKN'ye ait kayitli GSM: 05****3456")
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The actual data returned by the API
        /// </summary>
        public T? Value { get; set; }
    }
}

