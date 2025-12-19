namespace InteraktifKredi.Web.Models.Api
{
    /// <summary>
    /// Generic wrapper for API responses
    /// </summary>
    /// <typeparam name="T">The type of the response data</typeparam>
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message describing the result of the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The actual response data
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// HTTP status code of the response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ServiceResponse<T> SuccessResponse(T value, string message = "Success", int statusCode = 200)
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Value = value,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a failure response
        /// </summary>
        public static ServiceResponse<T> FailureResponse(string message, int statusCode = 400)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Value = default,
                StatusCode = statusCode
            };
        }
    }
}

